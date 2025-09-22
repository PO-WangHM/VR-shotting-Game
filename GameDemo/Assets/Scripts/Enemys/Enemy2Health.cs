using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Health : MonoBehaviour, getBulletValues
{
    public float InitialHealth = 10; // 敌人初始生命值



    private float currentHealth; // 当前生命值
    private float damage = 0f; // 受到的子弹伤害值,碰到子弹伤害则为0

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = InitialHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // 每帧调用受伤方法
        TakeDamage();

        // 每帧检查当前生命值,当生命值小于等于0时销毁敌人
        if (currentHealth <= 0)
        {
            Destroy(gameObject);

        }
    }
    //获取子弹伤害值
    public void GetDamageValue(float damageValue)
    {
        damage = damageValue;
    }
    //计算怪物受到伤害
    public void TakeDamage()
    {
        currentHealth -= damage;
        damage = 0; // 重置伤害值，防止持续扣血
    }
}
