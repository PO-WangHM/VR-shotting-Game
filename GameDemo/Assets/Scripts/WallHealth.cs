using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour
{
    public float WallHP = 100f;
    public GameObject GameOverPanel;//结束面板展示

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
            GameOverPanel.gameObject.SetActive(true);
        }
    }
}
