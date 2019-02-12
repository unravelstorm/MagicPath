﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "CreateManagerVasContainer")]
public class ManagerVas : ScriptableObject {
    /// <summary>
    /// 获取此类
    /// </summary>
    /// <returns></returns>
    public static ManagerVas GetManagerVas()
    {
        return Resources.Load<ManagerVas>("ManagerVasContainer");
    }
    /// <summary>
    /// 获取背景图片
    /// </summary>
    public List<Sprite> bgThemeSpriteList = new List<Sprite>();
    /// <summary>
    /// 人物预制体
    /// </summary>
    public GameObject chacaterPre;
    /// <summary>
    /// 普通平台预制体
    /// </summary>
    public GameObject normalPlatformPre;
    /// <summary>
    /// 下一个平台所产生的位移
    /// </summary>
    public float nextXPos = 0.554f, nextYPos = 0.645f;
}
