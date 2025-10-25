using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Health Bar Reference")]
    public Slider healthSlider;
    public Text healthText;

    [Header("Enemy Settings")]
    public float InitialHealth = 5f; // 敌人初始生命值
    public float IxpValue = 50f; // 击败敌人获得的经验值
    public float IcoinValue = 5; //击败敌人获得金币
    public float IscoreValue = 10; //击败敌人增加得分
    public float distroyTime = 40f;//自动销毁时间
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // 怪物移动速度

    [Header("Audio Setting")]
    public AudioSource audioSource;//音效组件
    public AudioClip[] audioClips;//音效

    protected float xpValue = 0; //怪物当前经验值
    protected float coinValue = 5; //击败敌人获得金币
    protected float scoreValue = 10; //击败敌人增加得分
    protected float currentTurnHealth;//当前轮次的最大生命
    protected float currentHealth; // 当前生命值
    protected float damage = 0f; // 受到的子弹伤害值,未碰到子弹伤害则为0
    private float distroyTimer = 0f;

    // 添加死亡状态标识
    private bool isDead = false;
    private bool hasRotated = false; // 标记是否已经旋转

    //玩家物体信息
    GameObject playerObj;
    GameObject planeObj;

    //轮次信息
    protected int turn = 1;
    private bool ValueisChange = false;

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
        InitializeHealthBar();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // 如果已经死亡，不再执行其他逻辑
        if (isDead) return;

        // 每帧调用受伤方法
        TakeDamage();

        //怪物移动
        Move();

        //获取轮次
        getTurn();
        if (!ValueisChange)
        {
            ValueChange();
            ValueisChange = true;
        }

        //火子弹命中造成持续伤害效果
        if (CT != 0)
        {
            Firetimer1 += Time.deltaTime;
            Firetimer2 += Time.deltaTime;
            if (Firetimer1 >= pertime)
            {
                TakeContinuousDamage();
                Firetimer1 = 0;
            }
            if (Firetimer2 > CT)
            {
                CT = 0;
                CD = 0;
                Firetimer2 = 0;
            }
        }

        //冰子弹命中造成减速效果
        if (ST != 0)
        {
            if (SR != 0)
            {
                SlowDown();
            }

            Icetimer += Time.deltaTime;
            if (Icetimer >= ST)
            {
                ST = 0;
                Icetimer = 0;
                currentspeed = moveSpeed;
            }
        }

        // 当生命值小于等于0时怪物死亡
        distroyTimer += Time.deltaTime;
        if (distroyTimer >= distroyTime)
        {
            currentHealth = 0;
        }
        Die();
    }

    //计算怪物受到伤害
    public virtual void TakeDamage()
    {
        currentHealth -= damage;
        damage = 0; // 重置伤害值，防止持续扣血
        UpdateHealthBar();//更新血条
    }

    //怪物死亡
    public virtual void Die()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;

            // 关闭碰撞体
            Collider collider = GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;

            // 停止移动
            currentspeed = 0f;

            // 播放死亡音效
            audioSource.PlayOneShot(audioClips[4]);

            // 给玩家奖励
            playerObj = GameObject.Find("Player");
            OutputPlayerValue opv = playerObj.gameObject.GetComponent<OutputPlayerValue>();
            if (opv != null)
            {
                opv.getXP(xpValue);
                opv.getCoin(coinValue);
                opv.getScore(scoreValue);
            }

            // 通知生成器
            FindObjectOfType<EnemySpawn>().EnemyDefeated();

            // 执行死亡旋转
            PerformDeathRotation();

            // 1秒后销毁物体
            StartCoroutine(DestroyAfterDelay(1f));
        }
    }

    // 执行死亡旋转
    private void PerformDeathRotation()
    {
        if (!hasRotated)
        {
            // 绕X轴旋转90度
            transform.Rotate(90f, 0f, 0f);
            hasRotated = true;
        }
    }

    // 延迟销毁协程
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    //怪物移动
    public virtual void Move()
    {
        transform.Translate(Vector3.back * currentspeed * Time.deltaTime, Space.World);
    }

    //碰撞检测
    public virtual void OnTriggerEnter(Collider collision)
    {
        // 如果已经死亡，不再处理碰撞
        if (isDead) return;

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
        if (collision.gameObject.name.Contains("NormalBullet"))
        {
            audioSource.PlayOneShot(audioClips[0]);
        }

        ////碰到火属性子弹后获取火子弹属性值
        if (collision.gameObject.name.Contains("FireBullet"))
        {
            audioSource.PlayOneShot(audioClips[1]);
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
            audioSource.PlayOneShot(audioClips[2]);
            OutputIceBulletValues oibv = collision.gameObject.GetComponent<OutputIceBulletValues>();


            if (oibv != null)
            {
                // 通过接口获取冰子弹的属性值
                ST = oibv.outputST();
                SR = oibv.outputSR();
                Icetimer = 0;
            }

        }
        if (collision.gameObject.name.Contains("ElectricBullet"))
        {
            audioSource.PlayOneShot(audioClips[3]);
        }
    }

    //火属性子弹持续伤害效果
    void TakeContinuousDamage()
    {
        currentHealth -= CD;
        UpdateHealthBar();//更新血条
    }

    //冰属性子弹减速效果
    void SlowDown()
    {
        currentspeed = currentspeed * SR;
        SR = 0;
    }

    //怪物血条初始化
    protected virtual void InitializeHealthBar()
    {
        // 如果未手动赋值，尝试自动查找
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / currentTurnHealth;

            // 可选：开始时隐藏血条，受伤时再显示
            // healthSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Health Slider not found on enemy: " + gameObject.name);
        }
    }

    // 更新怪物血条
    protected virtual void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / currentTurnHealth;
        }
        if (healthText != null)
        {
            healthText.text = "" + currentHealth;
        }
    }

    //获取轮次信息
    void getTurn()
    {
        planeObj = GameObject.Find("Plane");
        OutputTurn ot = planeObj.gameObject.GetComponent<OutputTurn>();
        if (ot != null)
        {
            turn = ot.outputTurn();
        }
    }

    //怪物数值更新
    void ValueChange()
    {
        currentTurnHealth = (float)(InitialHealth * (1 + 0.5 * (turn - 1)));
        currentHealth = currentTurnHealth;
        xpValue = (float)(IxpValue * (1 + 0.25 * (turn - 1)));
        coinValue = (float)(IcoinValue * (1 + 0.25 * (turn - 1)));
        scoreValue = (float)(IscoreValue * (1 + 0.25 * (turn - 1)));
    }
}