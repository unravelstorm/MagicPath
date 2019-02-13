using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    Grass,
    Winter,
}

public class PlatformSpawner : MonoBehaviour {
    private ManagerVas vars;

    /// <summary>
    /// 初始平台位置
    /// </summary>
    public Vector3 startSpawnPos;
    /// <summary>
    /// 平台生成位置
    /// </summary>
    public Vector3 platformSpawnPos;
    /// <summary>
    /// 生成平台数量
    /// </summary>
    private int platformSpawnCount;
    /// <summary>
    /// 平台是否向左生成
    /// </summary>
    private bool isLeftSpawn = false;
    /// <summary>
    /// 当前游戏主题的正常平台图片
    /// </summary>
    private Sprite curPlatformThemeSprite;
    /// <summary>
    /// 组合平台的类型
    /// </summary>
    private PlatformGroupType platformGroupType;
    /// <summary>
    /// 判断组合平台障碍物所处的方向,0为右，1为左
    /// </summary>
    private int obstacleDir = 0;
    /// <summary>
    /// 钉子方向生成平台的位置
    /// </summary>
    private Vector3 spikeDirPlatformPos;
    /// <summary>
    /// 钉子方向生成平台的数量
    /// </summary>
    private int spikeDirPlatformCount;
    /// <summary>
    /// 是否是钉子方向平台的生成
    /// </summary>
    private bool isSpikeDirPlatformSpawn = false;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
        vars = ManagerVas.GetManagerVas();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }
    private void Start()
    {
        RandomPlatformTheme();
        platformSpawnPos = startSpawnPos;
        platformSpawnCount = 5;
        for (int i = 0; i < 5; i++)
        {
            DecidePath();
        }
        //生成人物
        GameObject go = Instantiate(vars.chacaterPre);
        go.transform.position = new Vector3(0, -1.8f, 0);
    }

    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if(isSpikeDirPlatformSpawn)
        {
            SpikeDirPlatformSpawn();
            return;
        }
        if (platformSpawnCount > 0)
        {
            platformSpawnCount--;
            PlatformSpawn();
        }
        else
        {
            isLeftSpawn = !isLeftSpawn;
            platformSpawnCount = Random.Range(1, 4);
            PlatformSpawn();
        }
    }
    /// <summary>
    /// 平台生成
    /// </summary>
    private void PlatformSpawn()
    {
        obstacleDir = Random.Range(0, 2);
        //生成普通平台
        if (platformSpawnCount > 0)
        {
            NormalPlatformSpawn();
        }
        //生成组合平台
        else if(platformSpawnCount == 0)
        {
            int ran = Random.Range(0, 3);
            switch (ran)
            {
                //生成普通组合平台
                case 0: CommonPlatformGroupSpawn(); break;
                //生成主题组合平台
                case 1:
                    switch (platformGroupType)
                    {
                        case PlatformGroupType.Grass: GrassPlatformGroupSpawn(); break;
                        case PlatformGroupType.Winter: WinterPlatformGroupSpawn(); break;
                        default:break;
                    }
                    break;
                //生成钉子组合平台
                case 2:
                    isSpikeDirPlatformSpawn = true;
                    spikeDirPlatformCount = 4;
                    //钉子是否向左生成
                    bool isSpikeLeftSpawn = !isLeftSpawn;
                    SpikePlatformGroupSpawn(isSpikeLeftSpawn);
                    if (isSpikeLeftSpawn)//钉子向左生成
                    {
                        spikeDirPlatformPos = new Vector3(platformSpawnPos.x - 1.65f, platformSpawnPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        spikeDirPlatformPos = new Vector3(platformSpawnPos.x + 1.65f, platformSpawnPos.y + vars.nextYPos, 0);
                    }
                    break;
                default:break;
            }
        }
        
        if (isLeftSpawn)
        {
            platformSpawnPos = new Vector3(platformSpawnPos.x - vars.nextXPos, platformSpawnPos.y + vars.nextYPos, 0);
        }
        else
        {
            platformSpawnPos = new Vector3(platformSpawnPos.x + vars.nextXPos, platformSpawnPos.y + vars.nextYPos, 0);
        }
    }
    /// <summary>
    /// 随机平台主题
    /// </summary>
    private void RandomPlatformTheme()
    {
        int random = Random.Range(0, vars.platformThemeSpriteList.Count);
        curPlatformThemeSprite = vars.platformThemeSpriteList[random];
        if (random == 2)//平台主题为冬天时
        {
            platformGroupType = PlatformGroupType.Winter;
        }
        else
        {
            platformGroupType = PlatformGroupType.Grass;
        }
    }

    /// <summary>
    /// 普通平台生成(单个)
    /// </summary>
    private void NormalPlatformSpawn()
    {
        GameObject go = Instantiate(vars.normalPlatformPre, transform);
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformManager>().Init(curPlatformThemeSprite);
    }
    /// <summary>
    /// 生成普通组合平台
    /// </summary>
    private void CommonPlatformGroupSpawn()
    {
        int ran = Random.Range(0, vars.commonPlatformGroupList.Count);
        GameObject go = Instantiate(vars.commonPlatformGroupList[ran], transform);
        go.transform.position = platformSpawnPos;
        //go.GetComponent<PlatformManager>().Init(curPlatformThemeSprite);
        go.GetComponent<PlatformManager>().Init(curPlatformThemeSprite, obstacleDir);
    }
    /// <summary>
    /// 生成草地组合平台
    /// </summary>
    private void GrassPlatformGroupSpawn()
    {
        int ran = Random.Range(0, vars.grassPlatformGroupList.Count);
        GameObject go = Instantiate(vars.grassPlatformGroupList[ran], transform);
        go.transform.position = platformSpawnPos;
        //go.GetComponent<PlatformManager>().Init(curPlatformThemeSprite);
        go.GetComponent<PlatformManager>().Init(curPlatformThemeSprite, obstacleDir);
    }
    /// <summary>
    /// 生成冬季组合平台
    /// </summary>
    private void WinterPlatformGroupSpawn()
    {
        int ran = Random.Range(0, vars.winterPlatformGroupList.Count);
        GameObject go = Instantiate(vars.winterPlatformGroupList[ran], transform);
        go.transform.position = platformSpawnPos;
        //go.GetComponent<PlatformManager>().Init(curPlatformThemeSprite);
        go.GetComponent<PlatformManager>().Init(curPlatformThemeSprite, obstacleDir);
    }
    /// <summary>
    /// 生成钉子组合平台
    /// </summary>
    private void SpikePlatformGroupSpawn(bool isSpikeLeftSpawn)
    {
        GameObject temp = null;
        if (isSpikeLeftSpawn)
        {
            temp = Instantiate(vars.SpikePlatformGroupLeft, transform);
        }
        else
        {
            temp = Instantiate(vars.SpikePlatformGroupRight, transform);
        }
        temp.transform.position = platformSpawnPos;
        temp.GetComponent<PlatformManager>().Init(curPlatformThemeSprite);
    }
    /// <summary>
    /// 钉子方向平台生成
    /// 同时也生成原来方向平台
    /// </summary>
    private void SpikeDirPlatformSpawn()
    {
        if (spikeDirPlatformCount > 0)
        {
            spikeDirPlatformCount--;
            for (int i = 0; i < 2; i++)
            {
                GameObject go = Instantiate(vars.normalPlatformPre, transform);
                if (i==0)//原来方向平台生成
                {
                    go.transform.position = platformSpawnPos;
                    if (isLeftSpawn)//钉子生成在右边
                    {
                        platformSpawnPos = new Vector3(platformSpawnPos.x - vars.nextXPos, platformSpawnPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        platformSpawnPos = new Vector3(platformSpawnPos.x + vars.nextXPos, platformSpawnPos.y + vars.nextYPos, 0);
                    }
                }
                else//钉子方向平台生成
                {
                    go.transform.position = spikeDirPlatformPos;
                    if (!isLeftSpawn)
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x - vars.nextXPos, spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        spikeDirPlatformPos = new Vector3(spikeDirPlatformPos.x + vars.nextXPos, spikeDirPlatformPos.y + vars.nextYPos, 0);
                    }
                }
                go.GetComponent<PlatformManager>().Init(curPlatformThemeSprite);
            }
        }
        else
        {
            isSpikeDirPlatformSpawn = false;
            DecidePath();
        }
    }

}
