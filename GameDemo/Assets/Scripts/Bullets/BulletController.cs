using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab; // �ӵ�Ԥ����
    public Transform spawnPoint; // �ӵ����ɵ�
    public float bulletSpeed = 20f; // �ӵ��ٶ�
    public float bulletLifetime = 4f; // �ӵ�����ʱ��

    [Header("Shooting Rate")]
    public float shotsPerSecond = 5f; // ÿ�뷢���ӵ��������٣�

    private float timer = 0f;
    private float shootInterval; // ���������������ټ��㣩

    void Start()
    {
        // ���㷢����
        if (shotsPerSecond > 0)
        {
            shootInterval = 1f / shotsPerSecond;
        }
        else
        {
            shootInterval = 0.2f; // Ĭ��ֵ
            Debug.LogWarning("Shots per second cannot be zero or negative. Using default value.");
        }
    }

    void Update()
    {
        // ��ʱ�������ӵ�
        timer += Time.deltaTime;
        if (timer >= shootInterval)
        {
            FireBullet();
            timer = 0f;
        }
    }

    void FireBullet()
    {
        if (bulletPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Bullet prefab or spawn point not set!");
            return;
        }

        // ʵ�����ӵ�
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

        // ����ٶ�
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = spawnPoint.forward * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefab does not have a Rigidbody component!");
        }

        // �����Զ�����
        Destroy(bullet, bulletLifetime);
    }
}
