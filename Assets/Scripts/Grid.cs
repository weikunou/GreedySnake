using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网格
/// </summary>
public class Grid : MonoBehaviour
{
    /// <summary>
    /// 网格单例
    /// </summary>
    public static Grid instance;

    /// <summary>
    /// 网格宽度
    /// </summary>
    public int width;

    /// <summary>
    /// 网格高度
    /// </summary>
    public int height;

    /// <summary>
    /// 网格二维数组
    /// </summary>
    public int[,] grid;

    /// <summary>
    /// 边界
    /// </summary>
    public Transform border;
    public Transform borderLeft, borderRight, borderTop, borderBottom;

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
        // 获取四条边界
        borderLeft = border.Find("BorderLeft");
        borderRight = border.Find("BorderRight");
        borderTop = border.Find("BorderTop");
        borderBottom = border.Find("BorderBottom");

        // 获取网格宽度和高度，宽度：61，高度:33
        width = (int)(borderRight.position.x - borderLeft.position.x - borderLeft.localScale.x);
        height = (int)(borderTop.position.y - borderBottom.position.y - borderTop.localScale.y);
        
        // 创建网格二维数组
        // 网格共有 33 行，61 列
        grid = new int[height, width];
    }
}
