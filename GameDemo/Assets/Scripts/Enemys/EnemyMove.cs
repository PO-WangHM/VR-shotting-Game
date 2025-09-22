using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // 怪物移动速度

    void Update()
    {
        // 向-z方向移动
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }
}
