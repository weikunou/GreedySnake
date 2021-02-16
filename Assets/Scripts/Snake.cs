using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 贪吃蛇
/// </summary>
public class Snake : MonoBehaviour
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed;
    
    /// <summary>
    /// 蛇身预制体
    /// </summary>
    public GameObject bodyPrefab;

    /// <summary>
    /// 蛇头方向
    /// </summary>
    Vector3 direction;

    /// <summary>
    /// 蛇头上次移动的方向
    /// </summary>
    Vector3 lastDirection;

    /// <summary>
    /// 蛇头第一次改变的方向
    /// </summary>
    Vector3 firstDirection;

    /// <summary>
    /// 记录第一次按键的方向
    /// </summary>
    bool first = true;

    /// <summary>
    /// 蛇身列表
    /// </summary>
    List<Transform> bodyList = new List<Transform>();

    /// <summary>
    /// 蛇头是否吃到食物
    /// </summary>
    bool ate;

    void Start()
    {
        // 速度越大，间隔越短
        float interval = 1 / speed;

        // 每隔一段时间调用 Move 进行移动
        InvokeRepeating(nameof(Move), 0, interval);
    }

    void Update()
    {
        // 改变蛇头方向
        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");

        // 检测到按键输入
        if(h !=0 || v != 0)
        {
            // 限制蛇头不能往相反方向、斜方向移动
            if ((direction.x + h != 0 || direction.y + v != 0) && (h != v && h + v != 0))
            {
                // 改变蛇头的方向
                direction.x = h;
                direction.y = v;

                // 记录第一次按键的方向，该方向不会是相反方向
                if (first)
                {
                    first = false;
                    firstDirection.x = h;
                    firstDirection.y = v;
                }
            }
        }
    }

    void Move()
    {
        Vector3 lastPos = transform.position; // 记录蛇头移动前的位置

        // 移动前，检查下一步的方向和上次的方向是否正好相反
        if(direction.x + lastDirection.x == 0 || direction.y + lastDirection.y == 0)
        {
            // 将下一步的方向调整为第一次按键的方向
            direction.x = firstDirection.x;
            direction.y = firstDirection.y;
        }

        // 可以重新记录第一次按键的方向
        first = true;

        transform.Translate(direction); // 移动蛇头

        lastDirection = direction; // 记录蛇头上一次移动的方向

        // 当蛇头吃到食物
        if (ate)
        {
            // 生成新的蛇身
            GameObject body = Instantiate(bodyPrefab, lastPos, Quaternion.identity);

            // 将新的蛇身插入到蛇身列表的前面
            bodyList.Insert(0, body.transform);

            // 可以生成下一个食物
            FoodSpawner.instance.Spawn();

            // 增加分数
            GameManager.score++;
            UIManager.instance.scoreText.text = "分数：" + GameManager.score;

            ate = false;
        }
        // 蛇没有吃到食物，但是有身体
        else if (bodyList.Count > 0)
        {
            Vector3 lastBodyPos = bodyList.Last().position;

            // 把蛇身的最后一格移动到蛇头移动前的位置
            bodyList.Last().position = lastPos;

            // 更新蛇身列表
            bodyList.Insert(0, bodyList.Last());
            bodyList.RemoveAt(bodyList.Count - 1);

            Grid.instance.grid[Mathf.RoundToInt(lastBodyPos.y), Mathf.RoundToInt(lastBodyPos.x)] = 0;
        }
        // 蛇既没有吃到食物，也没有身体
        else
        {
            Grid.instance.grid[Mathf.RoundToInt(lastPos.y), Mathf.RoundToInt(lastPos.x)] = 0;
        }

        if(transform.position.y >= 0 && transform.position.y <= Grid.instance.height - 1 && transform.position.x >= 0 && transform.position.x <= Grid.instance.width - 1)
        {
            // 更新网格
            Grid.instance.grid[Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.x)] = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 碰撞到食物
        if (collision.CompareTag("Food"))
        {
            // 销毁食物
            Destroy(collision.gameObject);

            ate = true;
        }
        // 碰撞到边界、蛇身
        else
        {
            // 游戏结束
            GameManager.instance.GameOver();
        }
    }
}
