using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour, OutputTurn
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefab; // 怪物预制体
    public Transform[] spawnPoints; // 生成点数组

    [Header("Spawning Settings")]
    public float minSpawnInterval = 2f; // 最小生成间隔
    public float maxSpawnInterval = 5f; // 最大生成间隔
    public int maxEnemies = 10; // 场景中最大怪物数量

    [Header("Interval Reduction Settings")]
    public int roundsToReduceInterval = 2; // 每几轮减少一次生成间隔
    public float maxIntervalReduction = 0.5f; // 最大生成间隔减少量
    public float minIntervalReduction = 0.1f; // 最小生成间隔减少量
    public float minMaxInterval = 1f; // 最大生成间隔的最小值
    public float minMinInterval = 0.5f; // 最小生成间隔的最小值

    [Header("Round System")]
    public float breakTime = 10f; // 轮次间隔时间
    public int turns = 1; // 当前轮次

    public Text BreakTimeText;
    public GameObject BreakTimePanel;

    private float timer = 0f;
    private float nextSpawnInterval; // 下一次生成的间隔时间
    private int currentEnemyCount = 0;
    private int enemiesSpawnedThisRound = 0; // 本轮已生成的怪物数量
    private bool isRoundBreak = false; // 是否处于轮次间隔
    private float breakTimer = 0f; // 轮次间隔计时器
    private int breakTimer2 = 0;

    // 第三种怪物相关变量
    private int specialEnemiesToSpawn = 0; // 本轮需要生成的第三种怪物数量
    private int specialEnemiesSpawned = 0; // 本轮已生成的第三种怪物数量
    private bool isSpawningSpecialEnemy = false; // 是否正在生成第三种怪物

    // 生成点管理相关变量
    private List<Transform> activeSpawnPoints = new List<Transform>(); // 当前激活的生成点
    private int initialSpawnPointCount = 3; // 初始生成点数量
    private int maxSpawnPointCount = 6; // 最大生成点数量

    // 初始间隔值（用于重置）
    private float initialMinSpawnInterval;
    private float initialMaxSpawnInterval;

    void Start()
    {
        // 保存初始间隔值
        initialMinSpawnInterval = minSpawnInterval;
        initialMaxSpawnInterval = maxSpawnInterval;

        // 初始化激活的生成点
        InitializeSpawnPoints();

        // 设置初始的生成间隔
        SetRandomSpawnInterval();
        StartNewRound();
    }

    // 初始化生成点
    void InitializeSpawnPoints()
    {
        // 确保有足够的生成点
        if (spawnPoints.Length < initialSpawnPointCount)
        {
            Debug.LogError("Not enough spawn points assigned! Need at least " + initialSpawnPointCount);
            return;
        }

        // 激活初始数量的生成点
        for (int i = 0; i < initialSpawnPointCount; i++)
        {
            activeSpawnPoints.Add(spawnPoints[i]);
        }

        Debug.Log($"Initialized with {activeSpawnPoints.Count} spawn points");
    }

    // 根据轮次更新激活的生成点
    void UpdateActiveSpawnPoints()
    {
        // 每5轮增加一个生成点，直到达到最大值
        int targetSpawnPointCount = initialSpawnPointCount + Mathf.FloorToInt((turns - 1) / 5);
        targetSpawnPointCount = Mathf.Min(targetSpawnPointCount, maxSpawnPointCount);
        targetSpawnPointCount = Mathf.Min(targetSpawnPointCount, spawnPoints.Length);

        // 如果当前激活的生成点数量不等于目标数量，则更新
        if (activeSpawnPoints.Count != targetSpawnPointCount)
        {
            activeSpawnPoints.Clear();

            for (int i = 0; i < targetSpawnPointCount; i++)
            {
                activeSpawnPoints.Add(spawnPoints[i]);
            }

            Debug.Log($"Updated active spawn points to {activeSpawnPoints.Count} (Turn {turns})");
        }
    }

    // 减少生成间隔
    void ReduceSpawnInterval()
    {
        // 检查是否达到减少间隔的轮次
        if (turns > 1 && (turns - 1) % roundsToReduceInterval == 0)
        {
            // 减少最大生成间隔，但不低于最小值
            float newMaxInterval = maxSpawnInterval - maxIntervalReduction;
            maxSpawnInterval = Mathf.Max(newMaxInterval, minMaxInterval);

            // 减少最小生成间隔，但不低于最小值
            float newMinInterval = minSpawnInterval - minIntervalReduction;
            minSpawnInterval = Mathf.Max(newMinInterval, minMinInterval);

            Debug.Log($"Reduced spawn intervals: min={minSpawnInterval}, max={maxSpawnInterval} (Turn {turns})");
        }
    }

    void Update()
    {
        if (isRoundBreak)
        {
            // 轮次间隔计时
            breakTimer += Time.deltaTime;
            BreakTimePanel.gameObject.SetActive(true);
            breakTimer2 = (int)(breakTime - breakTimer) + 1;
            BreakTimeText.text = breakTimer2 + "s";
            if (breakTimer >= breakTime)
            {
                StartNewRound();
                BreakTimePanel.gameObject.SetActive(false);
            }
            return;
        }

        // 如果本轮已达到最大生成数量，检查是否所有怪物都被消灭
        if (enemiesSpawnedThisRound >= maxEnemies)
        {
            if (currentEnemyCount <= 0)
            {
                StartRoundBreak();
            }
            return;
        }

        // 如果已达到最大怪物数量，停止生成
        if (currentEnemyCount >= maxEnemies) return;

        // 检查是否需要生成第三种怪物
        if (specialEnemiesToSpawn > 0 && specialEnemiesSpawned < specialEnemiesToSpawn)
        {
            if (!isSpawningSpecialEnemy)
            {
                // 开始生成第三种怪物
                isSpawningSpecialEnemy = true;
                timer = 0f;
                nextSpawnInterval = 1f; // 第三种怪物生成间隔较短
            }
        }

        // 计时并生成怪物
        timer += Time.deltaTime;
        if (timer >= nextSpawnInterval)
        {
            if (isSpawningSpecialEnemy && specialEnemiesToSpawn > 0 && specialEnemiesSpawned < specialEnemiesToSpawn)
            {
                SpawnSpecialEnemy();
            }
            else
            {
                SpawnEnemy();
            }

            timer = 0f;
            SetRandomSpawnInterval();
        }
    }

    void SetRandomSpawnInterval()
    {
        // 在最小和最大间隔之间随机生成一个时间
        nextSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || enemyPrefab.Length == 0 || activeSpawnPoints.Count == 0)
        {
            Debug.LogWarning("Enemy prefab or spawn points not set!");
            return;
        }

        // 从激活的生成点中随机选择一个
        Transform spawnPoint = activeSpawnPoints[Random.Range(0, activeSpawnPoints.Count)];

        // 根据轮次决定生成哪种怪物
        int enemyIndex = GetEnemyIndexByRound();

        // 实例化怪物
        GameObject enemy = Instantiate(enemyPrefab[enemyIndex], spawnPoint.position, enemyPrefab[enemyIndex].transform.rotation);

        // 增加怪物计数
        currentEnemyCount++;
        enemiesSpawnedThisRound++;
    }

    // 生成第三种怪物
    void SpawnSpecialEnemy()
    {
        if (enemyPrefab == null || enemyPrefab.Length < 3 || activeSpawnPoints.Count == 0)
        {
            Debug.LogWarning("Enemy prefab or spawn points not set!");
            return;
        }

        // 从激活的生成点中随机选择一个
        Transform spawnPoint = activeSpawnPoints[Random.Range(0, activeSpawnPoints.Count)];

        // 实例化第三种怪物（索引为2）
        GameObject enemy = Instantiate(enemyPrefab[2], spawnPoint.position, enemyPrefab[2].transform.rotation);

        // 增加怪物计数
        currentEnemyCount++;
        enemiesSpawnedThisRound++;
        specialEnemiesSpawned++;

        // 检查是否已完成第三种怪物的生成
        if (specialEnemiesSpawned >= specialEnemiesToSpawn)
        {
            isSpawningSpecialEnemy = false;
        }
    }

    // 根据轮次决定生成哪种怪物
    int GetEnemyIndexByRound()
    {
        // 第一轮只生成第一种怪物（索引0）
        if (turns == 1)
        {
            return 0;
        }
        // 第二轮及以后按照7:3的比例生成前两种怪物
        else
        {
            // 生成一个0到1之间的随机数
            float randomValue = Random.Range(0f, 1f);

            // 如果随机数小于0.7，生成第一种怪物（70%概率）
            // 否则生成第二种怪物（30%概率）
            if (randomValue < 0.7f)
            {
                return 0; // 第一种怪物
            }
            else
            {
                return 1; // 第二种怪物
            }
        }
    }

    // 开始新的轮次
    void StartNewRound()
    {
        // 更新激活的生成点
        UpdateActiveSpawnPoints();

        // 减少生成间隔（如果需要）
        ReduceSpawnInterval();

        isRoundBreak = false;
        breakTimer = 0f;
        enemiesSpawnedThisRound = 0;
        specialEnemiesSpawned = 0;
        isSpawningSpecialEnemy = false;

        // 计算本轮需要生成的第三种怪物数量
        CalculateSpecialEnemies();

        Debug.Log($"Round {turns} started! Max enemies: {maxEnemies}, Special enemies: {specialEnemiesToSpawn}, Active spawn points: {activeSpawnPoints.Count}, Spawn intervals: min={minSpawnInterval}, max={maxSpawnInterval}");

        // 重置计时器
        timer = 0f;
        SetRandomSpawnInterval();
    }

    // 计算本轮需要生成的第三种怪物数量
    void CalculateSpecialEnemies()
    {
        if (turns >= 5 && turns % 5 == 0)
        {
            // 每5轮生成一次，每次增加1个
            specialEnemiesToSpawn = turns / 5;
            Debug.Log($"This round will spawn {specialEnemiesToSpawn} special enemies.");
        }
        else
        {
            specialEnemiesToSpawn = 0;
        }
    }

    // 开始轮次间隔
    void StartRoundBreak()
    {
        isRoundBreak = true;
        breakTimer = 0f;

        // 进入下一轮
        turns++;
        maxEnemies += 3; // 每轮增加3个怪物

        Debug.Log($"Round completed! Starting break. Next round: {turns}, Max enemies: {maxEnemies}");
    }

    // 当怪物被消灭时调用此方法
    public void EnemyDefeated()
    {
        currentEnemyCount--;

        // 确保计数不为负数
        if (currentEnemyCount < 0)
            currentEnemyCount = 0;
    }

    public int outputTurn()
    {
        return turns;
    }

    public bool outputRoundBreak()
    {
        return isRoundBreak;
    }

    // 重置生成间隔（可选，用于游戏重置或特殊事件）
    public void ResetSpawnIntervals()
    {
        minSpawnInterval = initialMinSpawnInterval;
        maxSpawnInterval = initialMaxSpawnInterval;
        Debug.Log("Spawn intervals reset to initial values");
    }
}