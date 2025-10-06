using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float InitialHealth = 5f; // 敌人初始生命值
    public float xpValue = 50f; // 击败敌人获得的经验值
    public float coinValue = 5; //击败敌人获得金币
    public float scoreValue = 10; //击败敌人增加得分
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // 怪物移动速度


    protected float currentHealth; // 当前生命值
    protected float damage = 0f; // 受到的子弹伤害值,未碰到子弹伤害则为0

    //玩家物体信息
    GameObject playerObj; 
    

    //接收火属性子弹属性参数
    protected float CD = 0; //持续伤害值
    protected float CT = 0; //持续伤害持续时间
    protected float pertime = 0;//多少时间造成一次持续伤害
    protected float Firetimer1 = 0;//火子弹持续伤害计时器
    protected float Firetimer2 = 0;//火子弹持续时长计时器

    //接收冰属性子弹属性参数
    protected float currentspeed = 0f;//怪物当前速度
    protected float ST = 0;   //减速效果持续时间
    protected float SR = 0;   //减速倍率
    protected float Icetimer = 0;//冰子弹减速计时器

    // Start is called before the first frame update
    public virtual void Start()
    {
        currentHealth = InitialHealth;
        currentspeed = moveSpeed;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
        // 每帧调用受伤方法
        TakeDamage();

        //怪物移动
        Move();

        // 当生命值小于等于0时怪物死亡
        Die();

        //火子弹命中造成持续伤害效果
        if(CT != 0)
        {
            Firetimer1 += Time.deltaTime;
            Firetimer2 += Time.deltaTime;
            if(Firetimer1 >= pertime)
            {
                
                
                TakeContinuousDamage();
                Firetimer1 = 0;
            }
            if(Firetimer2 > CT)
            {
                CT = 0;
                CD = 0;
                Firetimer2 = 0;
            }
        }

        //冰子弹命中造成减速效果
        if(ST != 0)
        {
            if(SR != 0)
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
     
    //计算怪物受到伤害
    public virtual void TakeDamage()
    {
        currentHealth -= damage;
        damage = 0; // 重置伤害值，防止持续扣血
    }

    //怪物死亡
    public virtual void Die()
    {
        if (currentHealth <= 0)
        {
            playerObj = GameObject.Find("Player");
            OutputPlayerValue opv = playerObj.gameObject.GetComponent<OutputPlayerValue>();
            if(opv!=null)
            {
                opv.getXP(xpValue);
                opv.getCoin(coinValue);
                opv.getScore(scoreValue);
            }
            Destroy(gameObject);
        }
    }

    //怪物移动
    public virtual void Move()
    {
        transform.Translate(Vector3.back * currentspeed * Time.deltaTime, Space.World);
    }

    //碰撞检测
    public virtual void OnTriggerEnter(Collider collision)
    {
        //碰到子弹后获取子弹属性值
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // 通过接口子弹伤害值
            OutputBulletValues obv = collision.gameObject.GetComponent<OutputBulletValues>();

            if (obv != null)
            {
               
                damage = obv.outputDamage();
            }
        }

        ////碰到火属性子弹后获取火子弹属性值
        if (collision.gameObject.name.Contains("FireBullet"))
        {
            OutputFireBulletValues ofbv = collision.gameObject.GetComponent<OutputFireBulletValues>();
            if (ofbv != null)
            {
                // 通过接口获取火子弹的属性值
                CD = ofbv.outputCD();
                CT = ofbv.outputCT();
                pertime = ofbv.outputCDT();
                Firetimer2 = 0;
            }
        }
        
        //碰到冰属性子弹后获取冰子弹属性值
        if (collision.gameObject.name.Contains("IceBullet"))
        {
            OutputIceBulletValues oibv = collision.gameObject.GetComponent<OutputIceBulletValues>();

           
            if (oibv != null)
            {
                // 通过接口获取冰子弹的属性值
                ST = oibv.outputST();
                SR = oibv.outputSR();
                Icetimer = 0;
            }
            
        }

    }

    //火属性子弹持续伤害效果
    void TakeContinuousDamage()
    {
        currentHealth -= CD;  
    }

    //冰属性子弹减速效果
    void SlowDown()
    {
        currentspeed = currentspeed * SR;
        SR = 0;
    }
}
