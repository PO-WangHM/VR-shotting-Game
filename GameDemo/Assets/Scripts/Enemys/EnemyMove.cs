using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // �����ƶ��ٶ�

    void Update()
    {
        // ��-z�����ƶ�
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }
}
