using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 游戏管理器单例
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// 贪吃蛇
    /// </summary>
    public Snake snake;

    /// <summary>
    /// 分数
    /// </summary>
    public static int score;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 展示历史最高分
        UIManager.instance.historyScoreText.text = "历史最高分：" + PlayerPrefs.GetInt("HistoryScore");

        if (Camera.main.aspect < 1.77)
        {
            // 根据屏幕宽高比，调整相机的视窗大小
            Camera.main.orthographicSize = 32 / Camera.main.aspect;
        }

        if (PlayerPrefs.GetString("MoveMode", "Other").Equals("Joystick"))
        {
            snake.toggle.isOn = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        Time.timeScale = 1;
        snake.enabled = true;
        FoodSpawner.instance.Spawn();
        score = 0;

        // 隐藏开始界面
        UIManager.instance.TogglePanelState(UIManager.instance.panels[(int)Panel.StartPanel], false);

        // 显示游戏界面
        UIManager.instance.TogglePanelState(UIManager.instance.panels[(int)Panel.GamePanel], true);
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    public void GameOver()
    {
        Time.timeScale = 0;

        // 显示结束界面
        UIManager.instance.TogglePanelState(UIManager.instance.panels[(int)Panel.OverPanel], true);
    }

    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        // 当前分数大于历史最高分
        if(score > PlayerPrefs.GetInt("HistoryScore"))
        {
            // 更新历史最高分
            PlayerPrefs.SetInt("HistoryScore", score);
        }

        // 重新加载场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 游戏胜利
    /// </summary>
    public void GameWin()
    {
        Time.timeScale = 0;

        // 显示胜利界面
        UIManager.instance.TogglePanelState(UIManager.instance.panels[(int)Panel.WinPanel], true);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// 设置蛇的移动模式
    /// </summary>
    /// <param name="mode">模式</param>
    public void SetMoveMode(bool mode)
    {
        snake.isJoystick = mode;
        snake.setOnce = mode;

        if (mode)
        {
            PlayerPrefs.SetString("MoveMode", "Joystick");
        }
        else
        {
            PlayerPrefs.SetString("MoveMode", "Other");
        }
    }
}
