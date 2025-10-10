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
            WallHP -= 100;
            FindObjectOfType<EnemySpawn>().EnemyDefeated();
            Destroy(collision.gameObject);
        }
    }
    void Die()
    {
        if (WallHP <= 0)
        {
            Time.timeScale = 0f;
        }
    }
}
