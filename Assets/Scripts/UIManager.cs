using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 界面
/// </summary>
enum Panel { GamePanel, StartPanel, OverPanel, WinPanel}

/// <summary>
/// UI 管理器
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// UI 管理器单例
    /// </summary>
    public static UIManager instance;

    /// <summary>
    /// 界面
    /// </summary>
    public GameObject[] panels;

    /// <summary>
    /// 分数文本
    /// </summary>
    public Text scoreText;

    /// <summary>
    /// 历史最高分文本
    /// </summary>
    public Text historyScoreText;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 切换界面状态
    /// </summary>
    /// <param name="panel">界面</param>
    /// <param name="state">状态</param>
    public void TogglePanelState(GameObject panel, bool state)
    {
        panel.SetActive(state);
    }
}
