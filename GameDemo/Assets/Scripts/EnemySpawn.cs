using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefab; // 怪物预制体
    public Transform[] spawnPoints; // 生成点数组

    [Header("Spawning Settings")]
    public float minSpawnInterval = 2f; // 最小生成间隔
    public float maxSpawnInterval = 5f; // 最大生成间隔
    public int maxEnemies = 10; // 场景中最大怪物数量

    private float timer = 0f;
    private float nextSpawnInterval; // 下一次生成的间隔时间
    private int currentEnemyCount = 0;

    void Start()
    {
        // 设置初始的生成间隔
        SetRandomSpawnInterval();
    }

    void Update()
    {
        // 如果已达到最大怪物数量，停止生成
        if (currentEnemyCount >= maxEnemies) return;

        // 计时并生成怪物
        timer += Time.deltaTime;
        if (timer >= nextSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
            SetRandomSpawnInterval(); // 为下一次生成设置新的随机间隔
        }
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
    }


}
