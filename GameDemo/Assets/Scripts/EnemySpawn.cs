using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefab; // ����Ԥ����
    public Transform[] spawnPoints; // ���ɵ�����

    [Header("Spawning Settings")]
    public float spawnRate = 1f; // ÿ�����ɹ�������
    public int maxEnemies = 10; // ����������������

    private float timer = 0f;
    private float spawnInterval;
    private int currentEnemyCount = 0;

    void Start()
    {
        // �������ɼ��
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
        // ����Ѵﵽ������������ֹͣ����
        if (currentEnemyCount >= maxEnemies) return;

        // ��ʱ�����ɹ���
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

        // ���ѡ��һ�����ɵ�
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ʵ��������
        int enemyindex = Random.Range(0, enemyPrefab.Length);
        GameObject enemy = Instantiate(enemyPrefab[enemyindex], spawnPoint.position, spawnPoint.rotation);

        // ���ӹ������
        currentEnemyCount++;

    }

    
}
