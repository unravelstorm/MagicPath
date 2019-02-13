using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {
    public SpriteRenderer[] spriteRenderers;
    public GameObject obstacle;
    public void Init(Sprite sprite)
    {
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprite;
        }
    }
    public void Init(Sprite sprite, int dir)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = sprite;
        }
        if(dir == 0)//向右生成障碍物
        {
            if (obstacle != null)
            {
                obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x, obstacle.transform.localPosition.y, obstacle.transform.localPosition.z);
            }
        }
    }
}
