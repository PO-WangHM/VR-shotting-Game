using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour
{
    public float WallHP = 100f;

    void Start()
    {
        
    }

    void Update()
    {
        Die();
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        // 检查是否碰撞到敌人
        if (collision.gameObject.CompareTag("Enemy"))
        {
            WallHP -= 10;
        }
    }
    void Die()
    {
        if (WallHP <= 0)
        {

        }
    }
}
