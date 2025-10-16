using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawn : MonoBehaviour, OutputTurn
{

    [Header("Enemy Settings")]
    public GameObject[] enemyPrefab; // ����Ԥ����
    public Transform[] spawnPoints; // ���ɵ�����

    [Header("Spawning Settings")]
    public float minSpawnInterval = 2f; // ��С���ɼ��
    public float maxSpawnInterval = 5f; // ������ɼ��
    public int maxEnemies = 10; // ����������������

    [Header("Round System")]
    public float breakTime = 10f; // �ִμ��ʱ��
    public int turns = 1; // ��ǰ�ִ�

    public Text BreakTimeText;
    public GameObject BreakTimePanel;

    private float timer = 0f;
    private float nextSpawnInterval; // ��һ�����ɵļ��ʱ��
    private int currentEnemyCount = 0;
    private int enemiesSpawnedThisRound = 0; // ���������ɵĹ�������
    private bool isRoundBreak = false; // �Ƿ����ִμ��
    private float breakTimer = 0f; // �ִμ����ʱ��
    private int breakTimer2 = 0;

    void Start()
    {
        // ���ó�ʼ�����ɼ��
        SetRandomSpawnInterval();
        StartNewRound();
    }

    void Update()
    {
        if (isRoundBreak)
        {
            // �ִμ����ʱ
            breakTimer += Time.deltaTime;
            BreakTimePanel.gameObject.SetActive(true);
            breakTimer2 = (int)(breakTime - breakTimer)+1;
            BreakTimeText.text = breakTimer2 + "s";
            if (breakTimer >= breakTime)
            {
                StartNewRound();
                BreakTimePanel.gameObject.SetActive(false);
            }
            return;
        }

        // ��������Ѵﵽ�����������������Ƿ����й��ﶼ������
        if (enemiesSpawnedThisRound >= maxEnemies)
        {
            if (currentEnemyCount <= 0)
            {
                StartRoundBreak();
            }
            return;
        }

        // ����Ѵﵽ������������ֹͣ����
        if (currentEnemyCount >= maxEnemies) return;

        // ��ʱ�����ɹ���
        timer += Time.deltaTime;
        if (timer >= nextSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
            SetRandomSpawnInterval();
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

        // �����ִξ����������ֹ���
        int enemyIndex = GetEnemyIndexByRound();

        // ʵ��������
        GameObject enemy = Instantiate(enemyPrefab[enemyIndex], spawnPoint.position, spawnPoint.rotation);

        // ���ӹ������
        currentEnemyCount++;
        enemiesSpawnedThisRound++;
    }

    // �����ִξ����������ֹ���
    int GetEnemyIndexByRound()
    {
        // ��һ��ֻ���ɵ�һ�ֹ������0��
        if (turns == 1)
        {
            return 0;
        }
        // �ڶ��ּ��Ժ���8:2�ı������ɹ���
        else
        {
            // ����һ��0��1֮��������
            float randomValue = Random.Range(0f, 1f);

            // ��������С��0.8�����ɵ�һ�ֹ��80%���ʣ�
            // �������ɵڶ��ֹ��20%���ʣ�
            if (randomValue < 0.7f)
            {
                return 0; // ��һ�ֹ���
            }
            else
            {
                return 1; // �ڶ��ֹ���
            }
        }
    }

    // ��ʼ�µ��ִ�
    void StartNewRound()
    {
        isRoundBreak = false;
        breakTimer = 0f;
        enemiesSpawnedThisRound = 0;

        Debug.Log($"Round {turns} started! Max enemies: {maxEnemies}");

        // ���ü�ʱ��
        timer = 0f;
        SetRandomSpawnInterval();
    }

    // ��ʼ�ִμ��
    void StartRoundBreak()
    {
        isRoundBreak = true;
        breakTimer = 0f;

        // ������һ��
        turns++;
        maxEnemies += 5; // ÿ������5������

        Debug.Log($"Round completed! Starting break. Next round: {turns}, Max enemies: {maxEnemies}");
    }

    // �����ﱻ����ʱ���ô˷���
    public void EnemyDefeated()
    {
        currentEnemyCount--;

        // ȷ��������Ϊ����
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
}