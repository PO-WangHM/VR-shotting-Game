using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    [Header("ShootingSetting")]
    public float launchInterval = 3f;    // 发射间隔
    public float projectileSpeed = 15f;  // 发射物速度

    [Header("Using")]
    public Transform flyObject;          // 子物体引用

    private Vector3 originalLocalPosition; // 子物体原始本地位置
    private Quaternion originalLocalRotation; // 子物体原始本地旋转
    private bool isProjectileActive = false; // 发射物是否已激活
    private Transform playerTarget;
    private Transform projectileParent;   // 用于存储发射物的临时父级
    private FlyObject flyObjectScript;    // 引用FlyObject脚本

    public override void Start()
    {
        base.Start();
        // 初始化引用
        InitializeReferences();

        // 开始发射协程
        StartCoroutine(LaunchRoutine());


        // 创建一个空对象作为发射物的临时父级
        GameObject tempParent = new GameObject("ProjectileParent");
        projectileParent = tempParent.transform;
    }

    void InitializeReferences()
    {
        // 如果没有手动指定flyObject，尝试自动查找
        if (flyObject == null)
        {
            flyObject = transform.Find("flyobject");
            if (flyObject == null)
            {
                Debug.LogError("未找到子物体 'flyobject'！");
                return;
            }
        }

        // 获取FlyObject脚本组件
        flyObjectScript = flyObject.GetComponent<FlyObject>();
        if (flyObjectScript == null)
        {
            Debug.LogError("未找到FlyObject脚本组件！");
            return;
        }

        // 保存原始位置和旋转
        originalLocalPosition = flyObject.localPosition;
        originalLocalRotation = flyObject.localRotation;

        // 查找玩家对象
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            Debug.Log("成功找到玩家对象: " + playerTarget.name);
        }
        else
        {
            Debug.LogWarning("未找到名为 'Player' 的游戏对象！将定期重试查找...");
            // 如果没找到，稍后重试
            StartCoroutine(RetryFindPlayer());
        }
    }

    IEnumerator RetryFindPlayer()
    {
        while (playerTarget == null)
        {
            yield return new WaitForSeconds(1f); // 每秒重试一次
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                playerTarget = playerObj.transform;
                Debug.Log("成功找到玩家对象: " + playerTarget.name);
            }
        }
    }

    IEnumerator LaunchRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(launchInterval);

            // 确保所有引用都有效且玩家存在
            if (flyObject != null && playerTarget != null && !isProjectileActive)
            {
                LaunchProjectile();
            }
            else if (playerTarget == null)
            {
                // 如果玩家不存在，尝试重新查找
                FindPlayer();
            }
        }
    }

    void LaunchProjectile()
    {
        // 再次确认玩家存在
        if (playerTarget == null) return;

        isProjectileActive = true;

        // 将发射物从当前父级分离，防止受到父级移动的影响
        flyObject.SetParent(projectileParent);

        // 设置FlyObject脚本的父级引用
        if (flyObjectScript != null)
        {
            flyObjectScript.SetParentShooter(this);
        }

        // 计算朝向玩家的方向
        Vector3 direction = (playerTarget.position - flyObject.position).normalized;

        // 启动发射协程
        StartCoroutine(MoveProjectile(direction));
    }

    IEnumerator MoveProjectile(Vector3 initialDirection)
    {
        // 发射物移动逻辑
        while (isProjectileActive && playerTarget != null)
        {
            // 计算当前帧的方向（如果需要持续跟踪玩家）
            Vector3 currentDirection = (playerTarget.position - flyObject.position).normalized;

            // 使用初始方向保持直线飞行
            flyObject.position += initialDirection * projectileSpeed * Time.deltaTime;

            // 使发射物朝向移动方向
            if (initialDirection != Vector3.zero)
            {
                flyObject.rotation = Quaternion.LookRotation(initialDirection);
            }

            // 检查是否超出范围或需要重置
            if (Vector3.Distance(flyObject.position, transform.position) > 50f)
            {
                ResetProjectile();
                yield break;
            }

            yield return null;
        }

        // 如果玩家在发射过程中消失，也重置发射物
        if (isProjectileActive)
        {
            ResetProjectile();
        }
    }

    // 将ResetProjectile改为公共方法，以便FlyObject脚本调用
    public void ResetProjectile()
    {
        if (flyObject != null)
        {
            // 重置到原始位置
            flyObject.SetParent(transform);
            flyObject.localPosition = originalLocalPosition;
            flyObject.localRotation = originalLocalRotation;
            isProjectileActive = false;
        }
    }

    // 当物体被销毁时，清理临时父级
    void OnDestroy()
    {
        if (projectileParent != null)
        {
            Destroy(projectileParent.gameObject);
        }
    }
}