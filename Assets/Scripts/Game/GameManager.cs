using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    /// <summary>
    /// 游戏是否开始
    /// </summary>
    public bool IsGameStarted { get; set; }
    /// <summary>
    /// 游戏是否结束
    /// </summary>
    public bool IsGameOver { get; set; }

    private void Awake()
    {
        Instance = this;
    }
}
