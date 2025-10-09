using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject[] bulletPrefab; // 子弹预制体
    public Transform spawnPoint; // 子弹生成点
    public float bulletSpeed = 20f; // 子弹速度
    public float bulletLifetime = 4f; // 子弹存在时间

    [Header("Shooting Rate")]
    public float shotsPerSecond = 5f; // 每秒发射子弹数（射速）

    private bool isRoundBreak = false;
    private GameObject planeObj;
    private OutputTurn outputTurnScript;
    private float timer = 0f;
    private float shootInterval; // 发射间隔（根据射速计算）

    void Start()
    {
        // 计算发射间隔
        if (shotsPerSecond > 0)
        {
            shootInterval = 1f / shotsPerSecond;
        }
        else
        {
            shootInterval = 0.2f; // 默认值
            Debug.LogWarning("Shots per second cannot be zero or negative. Using default value.");
        }

        // 初始化时获取一次引用
        InitializeOutputTurnReference();
    }

    void Update()
    {
        // 判断是否处于轮次间隙
        UpdateRoundBreakStatus();

        // 如果处于轮次间隔，停止发射
        if (isRoundBreak)
        {
            return;
        }

        // 计时并发射子弹
        timer += Time.deltaTime;
        if (timer >= shootInterval)
        {
            FireBullet();
            timer = 0f;
        }
    }

    void FireBullet()
    {
        if (bulletPrefab == null || bulletPrefab.Length == 0 || spawnPoint == null)
        {
            Debug.LogWarning("Bullet prefab or spawn point not set!");
            return;
        }

        // 实例化子弹
        GameObject bullet = Instantiate(bulletPrefab[0], spawnPoint.position, spawnPoint.rotation);

        // 添加速度
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = spawnPoint.forward * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefab does not have a Rigidbody component!");
        }

        // 设置自动销毁
        Destroy(bullet, bulletLifetime);
    }

    void InitializeOutputTurnReference()
    {
        planeObj = GameObject.Find("Plane");
        if (planeObj != null)
        {
            outputTurnScript = planeObj.GetComponent<OutputTurn>();
            if (outputTurnScript == null)
            {
                Debug.LogWarning("OutputTurn component not found on Plane object!");
            }
        }
        else
        {
            Debug.LogWarning("Plane object not found in scene!");
        }
    }

    void UpdateRoundBreakStatus()
    {
        // 如果引用为空，尝试重新获取
        if (outputTurnScript == null)
        {
            InitializeOutputTurnReference();
            return;
        }

        // 获取轮次间隔状态
        isRoundBreak = outputTurnScript.outputRoundBreak();
    }
}