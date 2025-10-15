using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    [Header("ShootingSetting")]
    public float launchInterval = 3f;    // ������
    public float projectileSpeed = 15f;  // �������ٶ�

    [Header("Using")]
    public Transform flyObject;          // ����������

    private Vector3 originalLocalPosition; // ������ԭʼ����λ��
    private Quaternion originalLocalRotation; // ������ԭʼ������ת
    private bool isProjectileActive = false; // �������Ƿ��Ѽ���
    private Transform playerTarget;
    private Transform projectileParent;   // ���ڴ洢���������ʱ����
    private FlyObject flyObjectScript;    // ����FlyObject�ű�

    public override void Start()
    {
        base.Start();
        // ��ʼ������
        InitializeReferences();

        // ��ʼ����Э��
        StartCoroutine(LaunchRoutine());


        // ����һ���ն�����Ϊ���������ʱ����
        GameObject tempParent = new GameObject("ProjectileParent");
        projectileParent = tempParent.transform;
    }

    void InitializeReferences()
    {
        // ���û���ֶ�ָ��flyObject�������Զ�����
        if (flyObject == null)
        {
            flyObject = transform.Find("flyobject");
            if (flyObject == null)
            {
                Debug.LogError("δ�ҵ������� 'flyobject'��");
                return;
            }
        }

        // ��ȡFlyObject�ű����
        flyObjectScript = flyObject.GetComponent<FlyObject>();
        if (flyObjectScript == null)
        {
            Debug.LogError("δ�ҵ�FlyObject�ű������");
            return;
        }

        // ����ԭʼλ�ú���ת
        originalLocalPosition = flyObject.localPosition;
        originalLocalRotation = flyObject.localRotation;

        // ������Ҷ���
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            Debug.Log("�ɹ��ҵ���Ҷ���: " + playerTarget.name);
        }
        else
        {
            Debug.LogWarning("δ�ҵ���Ϊ 'Player' ����Ϸ���󣡽��������Բ���...");
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
                Debug.Log("�ɹ��ҵ���Ҷ���: " + playerTarget.name);
            }
        }
    }

    IEnumerator LaunchRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(launchInterval);

            // ȷ���������ö���Ч����Ҵ���
            if (flyObject != null && playerTarget != null && !isProjectileActive)
            {
                LaunchProjectile();
            }
            else if (playerTarget == null)
            {
                // �����Ҳ����ڣ��������²���
                FindPlayer();
            }
        }
    }

    void LaunchProjectile()
    {
        // �ٴ�ȷ����Ҵ���
        if (playerTarget == null) return;

        isProjectileActive = true;

        // ��������ӵ�ǰ�������룬��ֹ�ܵ������ƶ���Ӱ��
        flyObject.SetParent(projectileParent);

        // ����FlyObject�ű��ĸ�������
        if (flyObjectScript != null)
        {
            flyObjectScript.SetParentShooter(this);
        }

        // ���㳯����ҵķ���
        Vector3 direction = (playerTarget.position - flyObject.position).normalized;

        // ��������Э��
        StartCoroutine(MoveProjectile(direction));
    }

    IEnumerator MoveProjectile(Vector3 initialDirection)
    {
        // �������ƶ��߼�
        while (isProjectileActive && playerTarget != null)
        {
            // ���㵱ǰ֡�ķ��������Ҫ����������ң�
            Vector3 currentDirection = (playerTarget.position - flyObject.position).normalized;

            // ʹ�ó�ʼ���򱣳�ֱ�߷���
            flyObject.position += initialDirection * projectileSpeed * Time.deltaTime;

            // ʹ�����ﳯ���ƶ�����
            if (initialDirection != Vector3.zero)
            {
                flyObject.rotation = Quaternion.LookRotation(initialDirection);
            }

            // ����Ƿ񳬳���Χ����Ҫ����
            if (Vector3.Distance(flyObject.position, transform.position) > 50f)
            {
                ResetProjectile();
                yield break;
            }

            yield return null;
        }

        // �������ڷ����������ʧ��Ҳ���÷�����
        if (isProjectileActive)
        {
            ResetProjectile();
        }
    }

    // ��ResetProjectile��Ϊ�����������Ա�FlyObject�ű�����
    public void ResetProjectile()
    {
        if (flyObject != null)
        {
            // ���õ�ԭʼλ��
            flyObject.SetParent(transform);
            flyObject.localPosition = originalLocalPosition;
            flyObject.localRotation = originalLocalRotation;
            isProjectileActive = false;
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