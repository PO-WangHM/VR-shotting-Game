using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour
{
    public float InitialDamageValue = 5f; // 子弹初始伤害

    private float currentDamageValue; // 当前伤害值


    void Start()
    {
        currentDamageValue = InitialDamageValue;
    }
    
    void Update()
    {

    }

    // 当子弹碰撞到敌人时触发
    void OnTriggerEnter(Collider collision)
    {
        // 检查是否碰撞到敌人
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("Bullet hit Enemy");
            // 获取敌人的伤害接口脚本
            getBulletValues gbv = collision.gameObject.GetComponent<getBulletValues>();

            if (gbv != null)
            {
                // 调用接口的TakeDamage方法
                gbv.GetDamageValue(currentDamageValue);
            }

            // 碰撞后销毁子弹
            Destroy(gameObject);
        }
    }
}
