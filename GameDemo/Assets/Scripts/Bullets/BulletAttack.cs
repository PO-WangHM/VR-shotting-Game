using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour, OutputBulletValues
{
    public float InitialDamageValue = 5f; // 子弹初始伤害

    private float currentDamageValue; // 当前伤害值


    protected virtual void Start()
    {
        currentDamageValue = InitialDamageValue;
    }

    protected virtual void Update()
    {

    }
    // 当子弹碰撞到敌人时触发
    public virtual void OnTriggerEnter(Collider collision)
    {
        // 检查是否碰撞到敌人
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 碰撞后销毁子弹
            Destroy(gameObject);
            
        }
    }
    public float outputDamage()
    {
        return currentDamageValue;
    }
}
