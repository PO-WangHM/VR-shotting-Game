using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefab; // 怪物预制体
    public Transform[] spawnPoints; // 生成点数组

    [Header("Spawning Settings")]
    public float spawnRate = 1f; // 每秒生成怪物数量
    public int maxEnemies = 10; // 场景中最大怪物数量

    private float timer = 0f;
    private float spawnInterval;
    private int currentEnemyCount = 0;

    void Start()
    {
        // 计算生成间隔
        if (spawnRate > 0)
        {
            spawnInterval = 1f / spawnRate;
        }
        else
        {
            spawnInterval = 1f;
            Debug.LogWarning("Spawn rate cannot be zero or negative. Using default value.");
        }
    }

    void Update()
    {
        // 如果已达到最大怪物数量，停止生成
        if (currentEnemyCount >= maxEnemies) return;

        // 计时并生成怪物
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Enemy prefab or spawn points not set!");
            return;
        }

        // 随机选择一个生成点
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 实例化怪物
        int enemyindex = Random.Range(0, enemyPrefab.Length);
        GameObject enemy = Instantiate(enemyPrefab[enemyindex], spawnPoint.position, spawnPoint.rotation);

        // 增加怪物计数
        currentEnemyCount++;

    }

    
}
