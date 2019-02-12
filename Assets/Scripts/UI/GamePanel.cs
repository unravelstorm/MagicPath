﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {
    private Button btn_Pause;
    private Button btn_Play;
    private Text txt_DiamondCount;
    private Text txt_Score;

    private void Init()
    {
        btn_Pause = transform.Find("btn_pause").GetComponent<Button>();
        btn_Pause.onClick.AddListener(OnPauseButtonClick);
        btn_Play = transform.Find("btn_play").GetComponent<Button>();
        btn_Play.onClick.AddListener(OnPlayButtonClick);
        txt_DiamondCount = transform.Find("Diamond/txt_diamondCount").GetComponent<Text>();
        txt_Score = transform.Find("txt_score").GetComponent<Text>();
        btn_Play.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowGamePanel, Show);
        Init();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePanel, Show);
    }
    /// <summary>
    /// 显示此面板
    /// </summary>
    private void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 暂停按钮点击
    /// </summary>
    private void OnPauseButtonClick()
    {
        btn_Play.gameObject.SetActive(true);
        btn_Pause.gameObject.SetActive(false);
        //游戏暂停

    }
    /// <summary>
    /// 开始按钮点击
    /// </summary>
    private void OnPlayButtonClick()
    {
        btn_Play.gameObject.SetActive(false);
        btn_Pause.gameObject.SetActive(true);
        //继续游戏

    }
}
