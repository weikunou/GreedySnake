using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 食物生成器
/// </summary>
public class FoodSpawner : MonoBehaviour
{
    /// <summary>
    /// 食物生成器单例
    /// </summary>
    public static FoodSpawner instance;

    /// <summary>
    /// 食物预制体
    /// </summary>
    public GameObject foodPrefab;

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
    /// 生成食物
    /// </summary>
    public void Spawn()
    {
        int x, y;
        int num = 0;

        do
        {
            // 随机生成坐标
            x = (int)Random.Range(0, Grid.instance.width);
            y = (int)Random.Range(0, Grid.instance.height);

            num++;

            if(num > Grid.instance.width * Grid.instance.height)
            {
                // 没有空间放置食物了，游戏胜利
                GameManager.instance.GameWin();
                break;
            }

        } while (Grid.instance.grid[y, x] != 0); // 网格坐标位置已有其他物体，重新随机

        // 实例化食物
        Instantiate(foodPrefab, new Vector3(x, y, 0), Quaternion.identity);

        // 更新网格
        Grid.instance.grid[y, x] = 1;
    }
}
