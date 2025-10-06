using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float HP = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Die();
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        // 检查是否碰撞到敌人
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HP -= 100;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name.Contains ("flyobject"))
        {
            HP -= 50;
        }
    }
    void Die()
    {
        if (HP <= 0)
        {
            Time.timeScale = 0f;
        }
    }
}
