using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Shooting Settings")]
    public float launchInterval = 0.5f;    // 单个发射物发射间隔（比普通敌人短）
    public float volleyInterval = 3f;      // 一轮发射完成后的间隔
    public float projectileSpeed = 10f;    // 发射物速度

    [Header("Boss Projectiles")]
    public Transform[] flyObjects;         // 5个子物体引用数组

    private Vector3[] originalLocalPositions;    // 子物体原始本地位置数组
    private Quaternion[] originalLocalRotations; // 子物体原始本地旋转数组
    private bool[] isProjectileActive;           // 发射物是否已激活数组
    private Transform playerTarget;
    private Transform projectileParent;          // 用于存储发射物的临时父级
    private FlyObject2[] flyObjectScripts;       // FlyObject2脚本数组

    public override void Start()
    {
        // 初始化引用
        InitializeReferences();

        // 开始发射协程
        StartCoroutine(LaunchRoutine());

        currentHealth = InitialHealth;
        currentspeed = moveSpeed;

        // 创建一个空对象作为发射物的临时父级
        GameObject tempParent = new GameObject("BossProjectileParent");
        projectileParent = tempParent.transform;
    }

    public override void Update()
    {
        // 首先调用父类的Update方法，保留原有功能
        base.Update();

        // 然后添加新的功能
        //DamageCalculate();
        //outputValues();
    }

    void InitializeReferences()
    {
        // 如果没有手动指定flyObjects，尝试自动查找
        if (flyObjects == null || flyObjects.Length == 0)
        {
            // 查找所有flyobject子物体
            List<Transform> foundObjects = new List<Transform>();
            foreach (Transform child in transform)
            {
                if (child.name.Contains("flyobject"))
                {
                    foundObjects.Add(child);
                }
            }

            if (foundObjects.Count == 0)
            {
                Debug.LogError("未找到任何flyobject子物体！");
                return;
            }

            flyObjects = foundObjects.ToArray();
        }

        // 初始化数组
        originalLocalPositions = new Vector3[flyObjects.Length];
        originalLocalRotations = new Quaternion[flyObjects.Length];
        isProjectileActive = new bool[flyObjects.Length];
        flyObjectScripts = new FlyObject2[flyObjects.Length];

        // 保存原始位置和旋转，获取FlyObject2脚本
        for (int i = 0; i < flyObjects.Length; i++)
        {
            if (flyObjects[i] != null)
            {
                originalLocalPositions[i] = flyObjects[i].localPosition;
                originalLocalRotations[i] = flyObjects[i].localRotation;
                isProjectileActive[i] = false;

                flyObjectScripts[i] = flyObjects[i].GetComponent<FlyObject2>();
                if (flyObjectScripts[i] == null)
                {
                    Debug.LogError($"在 {flyObjects[i].name} 上未找到FlyObject2脚本组件！");
                }
            }
        }

        // 查找玩家对象
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            Debug.Log("Boss成功找到玩家对象: " + playerTarget.name);
        }
        else
        {
            Debug.LogWarning("Boss未找到名为 'Player' 的游戏对象！将定期重试查找...");
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
                Debug.Log("Boss成功找到玩家对象: " + playerTarget.name);
            }
        }
    }

    IEnumerator LaunchRoutine()
    {
        while (true)
        {
            // 等待一轮发射完成后的间隔
            yield return new WaitForSeconds(volleyInterval);

            // 确保玩家存在
            if (playerTarget == null)
            {
                FindPlayer();
                continue;
            }

            // 依次发射所有flyobject
            for (int i = 0; i < flyObjects.Length; i++)
            {
                // 确保发射物存在且未激活
                if (flyObjects[i] != null && !isProjectileActive[i])
                {
                    LaunchProjectile(i);

                    // 等待发射间隔
                    yield return new WaitForSeconds(launchInterval);
                }
            }
        }
    }

    void LaunchProjectile(int index)
    {
        // 再次确认玩家存在和发射物有效
        if (playerTarget == null || flyObjects[index] == null) return;

        isProjectileActive[index] = true;

        // 将发射物从当前父级分离，防止受到父级移动的影响
        flyObjects[index].SetParent(projectileParent);

        // 设置FlyObject2脚本的父级引用
        if (flyObjectScripts[index] != null)
        {
            flyObjectScripts[index].SetParentShooter(this, index);
        }

        // 计算朝向玩家的方向
        Vector3 direction = (playerTarget.position - flyObjects[index].position).normalized;

        // 启动发射协程
        StartCoroutine(MoveProjectile(index, direction));
    }

    IEnumerator MoveProjectile(int index, Vector3 initialDirection)
    {
        // 发射物移动逻辑
        while (isProjectileActive[index] && playerTarget != null && flyObjects[index] != null)
        {
            // 使用初始方向保持直线飞行
            flyObjects[index].position += initialDirection * projectileSpeed * Time.deltaTime;

            // 使发射物朝向移动方向
            if (initialDirection != Vector3.zero)
            {
                flyObjects[index].rotation = Quaternion.LookRotation(initialDirection);
            }

            // 检查是否超出范围或需要重置
            if (Vector3.Distance(flyObjects[index].position, transform.position) > 50f)
            {
                ResetProjectile(index);
                yield break;
            }

            yield return null;
        }

        // 如果玩家在发射过程中消失或发射物被销毁，重置状态
        if (isProjectileActive[index])
        {
            ResetProjectile(index);
        }
    }

    // 重置指定索引的发射物
    public void ResetProjectile(int index)
    {
        if (flyObjects[index] != null)
        {
            // 重置到原始位置
            flyObjects[index].SetParent(transform);
            flyObjects[index].localPosition = originalLocalPositions[index];
            flyObjects[index].localRotation = originalLocalRotations[index];
            isProjectileActive[index] = false;
        }
    }

    // 重置所有发射物
    public void ResetAllProjectiles()
    {
        for (int i = 0; i < flyObjects.Length; i++)
        {
            ResetProjectile(i);
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