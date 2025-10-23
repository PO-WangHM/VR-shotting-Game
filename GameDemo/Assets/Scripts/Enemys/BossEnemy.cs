using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Shooting Settings")]
    public float launchInterval = 0.5f;    // ���������﷢����������ͨ���˶̣�
    public float volleyInterval = 3f;      // һ�ַ�����ɺ�ļ��
    public float projectileSpeed = 10f;    // �������ٶ�

    [Header("Boss Projectiles")]
    public Transform[] flyObjects;         // 5����������������

    private Vector3[] originalLocalPositions;    // ������ԭʼ����λ������
    private Quaternion[] originalLocalRotations; // ������ԭʼ������ת����
    private bool[] isProjectileActive;           // �������Ƿ��Ѽ�������
    private Transform playerTarget;
    private Transform projectileParent;          // ���ڴ洢���������ʱ����
    private FlyObject2[] flyObjectScripts;       // FlyObject2�ű�����

    public override void Start()
    {
        // ��ʼ������
        InitializeReferences();

        // ��ʼ����Э��
        StartCoroutine(LaunchRoutine());

        currentHealth = InitialHealth;
        currentspeed = moveSpeed;

        // ����һ���ն�����Ϊ���������ʱ����
        GameObject tempParent = new GameObject("BossProjectileParent");
        projectileParent = tempParent.transform;
    }

    public override void Update()
    {
        // ���ȵ��ø����Update����������ԭ�й���
        base.Update();

        // Ȼ������µĹ���
        //DamageCalculate();
        //outputValues();
    }

    void InitializeReferences()
    {
        // ���û���ֶ�ָ��flyObjects�������Զ�����
        if (flyObjects == null || flyObjects.Length == 0)
        {
            // ��������flyobject������
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
                Debug.LogError("δ�ҵ��κ�flyobject�����壡");
                return;
            }

            flyObjects = foundObjects.ToArray();
        }

        // ��ʼ������
        originalLocalPositions = new Vector3[flyObjects.Length];
        originalLocalRotations = new Quaternion[flyObjects.Length];
        isProjectileActive = new bool[flyObjects.Length];
        flyObjectScripts = new FlyObject2[flyObjects.Length];

        // ����ԭʼλ�ú���ת����ȡFlyObject2�ű�
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
                    Debug.LogError($"�� {flyObjects[i].name} ��δ�ҵ�FlyObject2�ű������");
                }
            }
        }

        // ������Ҷ���
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            Debug.Log("Boss�ɹ��ҵ���Ҷ���: " + playerTarget.name);
        }
        else
        {
            Debug.LogWarning("Bossδ�ҵ���Ϊ 'Player' ����Ϸ���󣡽��������Բ���...");
            // ���û�ҵ����Ժ�����
            StartCoroutine(RetryFindPlayer());
        }
    }

    IEnumerator RetryFindPlayer()
    {
        while (playerTarget == null)
        {
            yield return new WaitForSeconds(1f); // ÿ������һ��
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                playerTarget = playerObj.transform;
                Debug.Log("Boss�ɹ��ҵ���Ҷ���: " + playerTarget.name);
            }
        }
    }

    IEnumerator LaunchRoutine()
    {
        while (true)
        {
            // �ȴ�һ�ַ�����ɺ�ļ��
            yield return new WaitForSeconds(volleyInterval);

            // ȷ����Ҵ���
            if (playerTarget == null)
            {
                FindPlayer();
                continue;
            }

            // ���η�������flyobject
            for (int i = 0; i < flyObjects.Length; i++)
            {
                // ȷ�������������δ����
                if (flyObjects[i] != null && !isProjectileActive[i])
                {
                    LaunchProjectile(i);

                    // �ȴ�������
                    yield return new WaitForSeconds(launchInterval);
                }
            }
        }
    }

    void LaunchProjectile(int index)
    {
        // �ٴ�ȷ����Ҵ��ںͷ�������Ч
        if (playerTarget == null || flyObjects[index] == null) return;

        isProjectileActive[index] = true;

        // ��������ӵ�ǰ�������룬��ֹ�ܵ������ƶ���Ӱ��
        flyObjects[index].SetParent(projectileParent);

        // ����FlyObject2�ű��ĸ�������
        if (flyObjectScripts[index] != null)
        {
            flyObjectScripts[index].SetParentShooter(this, index);
        }

        // ���㳯����ҵķ���
        Vector3 direction = (playerTarget.position - flyObjects[index].position).normalized;

        // ��������Э��
        StartCoroutine(MoveProjectile(index, direction));
    }

    IEnumerator MoveProjectile(int index, Vector3 initialDirection)
    {
        // �������ƶ��߼�
        while (isProjectileActive[index] && playerTarget != null && flyObjects[index] != null)
        {
            // ʹ�ó�ʼ���򱣳�ֱ�߷���
            flyObjects[index].position += initialDirection * projectileSpeed * Time.deltaTime;

            // ʹ�����ﳯ���ƶ�����
            if (initialDirection != Vector3.zero)
            {
                flyObjects[index].rotation = Quaternion.LookRotation(initialDirection);
            }

            // ����Ƿ񳬳���Χ����Ҫ����
            if (Vector3.Distance(flyObjects[index].position, transform.position) > 50f)
            {
                ResetProjectile(index);
                yield break;
            }

            yield return null;
        }

        // �������ڷ����������ʧ�����ﱻ���٣�����״̬
        if (isProjectileActive[index])
        {
            ResetProjectile(index);
        }
    }

    // ����ָ�������ķ�����
    public void ResetProjectile(int index)
    {
        if (flyObjects[index] != null)
        {
            // ���õ�ԭʼλ��
            flyObjects[index].SetParent(transform);
            flyObjects[index].localPosition = originalLocalPositions[index];
            flyObjects[index].localRotation = originalLocalRotations[index];
            isProjectileActive[index] = false;
        }
    }

    // �������з�����
    public void ResetAllProjectiles()
    {
        for (int i = 0; i < flyObjects.Length; i++)
        {
            ResetProjectile(i);
        }
    }

    // �����屻����ʱ��������ʱ����
    void OnDestroy()
    {
        if (projectileParent != null)
        {
            Destroy(projectileParent.gameObject);
        }
    }
}