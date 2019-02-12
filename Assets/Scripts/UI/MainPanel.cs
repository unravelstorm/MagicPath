using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour {
    private Button btnStart;
    private Button btnShop;
    private Button btnRank;
    private Button btnSound;

    private void Init()
    {
        //绑定按钮
        btnStart = transform.Find("btn_start").GetComponent<Button>();
        //绑定按钮点击函数
        btnStart.onClick.AddListener(OnStartButtonClick);

        btnShop = transform.Find("Buttons/btn_shop").GetComponent<Button>();
        btnShop.onClick.AddListener(OnShopButtonClick);

        btnRank = transform.Find("Buttons/btn_rank").GetComponent<Button>();
        btnRank.onClick.AddListener(OnRankButtonClick);

        btnSound = transform.Find("Buttons/btn_sound").GetComponent<Button>();
        btnSound.onClick.AddListener(OnSoundButtonClick);
    }

    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// 开始按钮点击后调用
    /// </summary>
    private void OnStartButtonClick()
    {
        GameManager.Instance.IsGameStarted = true;
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 商店按钮点击事件
    /// </summary>
    private void OnShopButtonClick()
    {

    }
    /// <summary>
    /// 排行榜按钮点击事件
    /// </summary>
    private void OnRankButtonClick()
    {

    }
    /// <summary>
    /// 声音按钮点击事件  
    /// </summary>
    private void OnSoundButtonClick()
    {

    }
}
