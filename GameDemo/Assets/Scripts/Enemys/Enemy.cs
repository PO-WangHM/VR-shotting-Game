using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float InitialHealth = 5f; // 敌人初始生命值
    public float xpValue = 50f; // 击败敌人获得的经验值
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // 怪物移动速度


    protected float currentHealth; // 当前生命值
    protected float damage = 0f; // 受到的子弹伤害值,未碰到子弹伤害则为0

    //冰属性子弹参数
    private float currentspeed = 0f;
    private float ST = 0;
    private float SR = 0;
    private float Icetimer;//冰子弹减速计时器

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = InitialHealth;
        currentspeed = moveSpeed;
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

        //减速效果
        if(ST!=0)
        {
            if(SR!= 0)
            {
                SlowDown();
            }

            Icetimer += Time.deltaTime;
            if(Icetimer >= ST)
            {
                ST = 0;
                Icetimer = 0;
                currentspeed = moveSpeed;
            }
        }
        

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
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        transform.Translate(Vector3.back * currentspeed * Time.deltaTime, Space.World);
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // 获取子弹伤害值
            OutputBulletValues obv = collision.gameObject.GetComponent<OutputBulletValues>();

            if (obv != null)
            {
                // 调用接口的TakeDamage方法
                damage = obv.outputDamage();
            }
        }
        if (collision.gameObject.name.Contains("IceBullet"))
        {
            OutputBulletValues obv = collision.gameObject.GetComponent<OutputBulletValues>();

           
            if (obv != null)
            {
                // 调用接口的TakeDamage方法
                ST = obv.outputST();
                SR = obv.outputSR();
                Icetimer = 0;
            }
            
        }

    }

    void SlowDown()
    {
        currentspeed = currentspeed * SR;
        SR = 0;
    }
}
