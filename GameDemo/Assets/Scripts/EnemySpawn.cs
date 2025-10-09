using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour, OutputTurn
{
    public Text turnText;

    [Header("Enemy Settings")]
    public GameObject[] enemyPrefab; // 怪物预制体
    public Transform[] spawnPoints; // 生成点数组

    [Header("Spawning Settings")]
    public float minSpawnInterval = 2f; // 最小生成间隔
    public float maxSpawnInterval = 5f; // 最大生成间隔
    public int maxEnemies = 10; // 场景中最大怪物数量

    [Header("Round System")]
    public float breakTime = 10f; // 轮次间隔时间
    public int turns = 1; // 当前轮次

    private float timer = 0f;
    private float nextSpawnInterval; // 下一次生成的间隔时间
    private int currentEnemyCount = 0;
    private int enemiesSpawnedThisRound = 0; // 本轮已生成的怪物数量
    private bool isRoundBreak = false; // 是否处于轮次间隔
    private float breakTimer = 0f; // 轮次间隔计时器

    void Start()
    {
        // 设置初始的生成间隔
        SetRandomSpawnInterval();
        StartNewRound();
    }

    void Update()
    {
        if (isRoundBreak)
        {
            // 轮次间隔计时
            breakTimer += Time.deltaTime;
            if (breakTimer >= breakTime)
            {
                StartNewRound();
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

        // 计时并生成怪物
        timer += Time.deltaTime;
        if (timer >= nextSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
            SetRandomSpawnInterval();
        }

        turnText.text = "Turn: " + turns;
    }

    void SetRandomSpawnInterval()
    {
        // 在最小和最大间隔之间随机生成一个时间
        nextSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || enemyPrefab.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Enemy prefab or spawn points not set!");
            return;
        }

        // 随机选择一个生成点
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 实例化怪物
        int enemyIndex = Random.Range(0, enemyPrefab.Length);
        GameObject enemy = Instantiate(enemyPrefab[enemyIndex], spawnPoint.position, spawnPoint.rotation);

        // 增加怪物计数
        currentEnemyCount++;
        enemiesSpawnedThisRound++;
    }

    // 开始新的轮次
    void StartNewRound()
    {
        isRoundBreak = false;
        breakTimer = 0f;
        enemiesSpawnedThisRound = 0;

        Debug.Log($"Round {turns} started! Max enemies: {maxEnemies}");

        // 重置计时器
        timer = 0f;
        SetRandomSpawnInterval();
    }

    // 开始轮次间隔
    void StartRoundBreak()
    {
        isRoundBreak = true;
        breakTimer = 0f;

        // 进入下一轮
        turns++;
        maxEnemies += 10; // 每轮增加10个怪物

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
}