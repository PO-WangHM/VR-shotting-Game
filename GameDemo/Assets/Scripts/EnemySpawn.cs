using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefab; // ����Ԥ����
    public Transform[] spawnPoints; // ���ɵ�����

    [Header("Spawning Settings")]
    public float minSpawnInterval = 2f; // ��С���ɼ��
    public float maxSpawnInterval = 5f; // ������ɼ��
    public int maxEnemies = 10; // ����������������

    private float timer = 0f;
    private float nextSpawnInterval; // ��һ�����ɵļ��ʱ��
    private int currentEnemyCount = 0;

    void Start()
    {
        // ���ó�ʼ�����ɼ��
        SetRandomSpawnInterval();
    }

    void Update()
    {
        // ����Ѵﵽ������������ֹͣ����
        if (currentEnemyCount >= maxEnemies) return;

        // ��ʱ�����ɹ���
        timer += Time.deltaTime;
        if (timer >= nextSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
            SetRandomSpawnInterval(); // Ϊ��һ�����������µ�������
        }
    }

    void SetRandomSpawnInterval()
    {
        // ����С�������֮���������һ��ʱ��
        nextSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || enemyPrefab.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Enemy prefab or spawn points not set!");
            return;
        }

        // ���ѡ��һ�����ɵ�
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ʵ��������
        int enemyIndex = Random.Range(0, enemyPrefab.Length);
        GameObject enemy = Instantiate(enemyPrefab[enemyIndex], spawnPoint.position, spawnPoint.rotation);

        // ���ӹ������
        currentEnemyCount++;
    }


}
