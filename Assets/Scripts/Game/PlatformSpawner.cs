using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// 是否向左生成
    /// </summary>
    private bool isLeftSpawn = false;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }
    private void Start()
    {
        vars = ManagerVas.GetManagerVas();
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
        NormalPlatformSpawn();
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
    /// 普通平台生成
    /// </summary>
    private void NormalPlatformSpawn()
    {
        GameObject go = Instantiate(vars.normalPlatformPre, transform);
        go.transform.position = platformSpawnPos;
    }
}
