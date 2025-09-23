using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, getBulletValues
{
    [Header("Enemy Settings")]
    public float InitialHealth = 5f; // 敌人初始生命值
    public float xpValue = 50f; // 击败敌人获得的经验值
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // 怪物移动速度


    protected float currentHealth; // 当前生命值
    protected float damage = 0f; // 受到的子弹伤害值,未碰到子弹伤害则为0

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = InitialHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // 每帧调用受伤方法
        TakeDamage();

        //怪物移动
        Move();

        // 当生命值小于等于0时怪物死亡
        Die();
    }
     
    //获取子弹伤害值
    public void GetDamageValue(float damageValue)
    {
        damage = damageValue;
    }
    //计算怪物受到伤害
    public virtual void TakeDamage()
    {
        currentHealth -= damage;
        damage = 0; // 重置伤害值，防止持续扣血
    }
    public virtual void Die()
    {
        if (currentHealth <= 0)
        {
            // 找到名字为Canvas对象并获取其getEnemyValue脚本组件
            GameObject cavence = GameObject.Find("Canvas");
            if (cavence != null)
            {
                getEnemyValue gev = cavence.GetComponent<getEnemyValue>();
                if (gev != null)
                {
                    // 调用接口的TakeDamage方法
                    gev.getXP(xpValue);
                }
            }
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
       
    }


}
