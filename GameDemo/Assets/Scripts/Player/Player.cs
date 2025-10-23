using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, OutputPlayerValue
{
    [Header("Health Setting")]
    public float HP = 100;
    public float currentHP = 0;

    [Header("Bullet Spawns Setting")]
    public GameObject[] BulletsSpawns;

    [Header("Panel Setting")]
    public Text xpText; //经验展示
    public Text coinText; //金币展示
    public Text scoreText;//得分展示
    public Text levelText;//等级展示
    public Text damageText;//子弹伤害展示
    public Text hpText;//血量展示
    public Text turnText;//轮次展示
    public GameObject GameOverPanel;//结束面板展示
    public GameObject WarnPanel;//商店提示面板
    //火属性子弹面板展示
    public Text continueText;//持续伤害
    public Text continuetimeText;//持续伤害时间

    [Header("Button Setting")]
    public GameObject[] BuyButtons;

    private float XP = 0;    //经验
    private float levelupXP = 100;//升级所需经验
    private int Level = 1;  //等级
    private float Coin = 0;  //金币
    private float Score = 0; //得分
    private float timer = 0;//计时器
    private float damage = 0;//子弹基础伤害
    //火属性子弹属性值
    private float continuedamage = 0;
    private float continuetime = 0;

    //轮次信息;
    private GameObject planeObj;
    private int turn;
    private bool isRoundBreak = false;

    // Start is called before the first frame update
    void Start()
    {
        planeObj = GameObject.Find("Plane");
        currentHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        Die();
        LevelUp();
        ShowValue();
        Continue();
        getTurn();
        //等级提升带来特殊增益
        //10级开启第二枪口
        if (Level==5)
        {
            secondBS();
        }
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        // 检查是否碰撞到敌人
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentHP -= 100;
            FindObjectOfType<EnemySpawn>().EnemyDefeated();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name.Contains("flyobject"))
        {
            currentHP -= 25;
        }
    }

    void Die()
    {
        if (currentHP <= 0)
        {
            Time.timeScale = 0f;
            GameOverPanel.gameObject.SetActive(true);
        }
    }

    //升级
    void LevelUp()
    {
        if (XP >= levelupXP)
        {
            Level++;
            XP = XP - levelupXP;
            levelupXP = levelupXP + 100;
        }
    }

    //根据时间不断增加得分于金币
    void Continue()
    {
        // 如果处于轮次间隔，不增加金币和得分
        if (isRoundBreak)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= 1)
        {
            Score += 10;
            Coin += 5;
            timer = 0;
        }
    }

    //数值展示
    void ShowValue()
    {
        xpText.text = "XP: " + XP;
        coinText.text = "Coin: " + Coin;
        scoreText.text = "Score: " + Score;
        levelText.text = "Level: " + Level;
        damageText.text = "Damage: " + damage;
        continueText.text = "Continuous Damage: " + continuedamage;
        continuetimeText.text = "Continuous Time: " + continuetime;
        hpText.text = "HP: " + currentHP + "/" + HP;
        turnText.text = "Turn: " + turn;
    }

    public void getXP(float xpValue)
    {
        XP += xpValue;
    }

    public void getCoin(float coinValue)
    {
        Coin += coinValue;
    }

    public void getScore(float scoreValue)
    {
        Score += scoreValue;
    }

    public void getDamage(float currentDamageValue)
    {
        damage = currentDamageValue;
    }

    public void getContinueDamage(float countinuousDamage)
    {
        continuedamage = countinuousDamage;
    }

    public void getContinueTime(float countinuousTime)
    {
        continuetime = countinuousTime;
    }

    public float outputLevel()
    {
        return Level;
    }

    //获取轮次
    void getTurn()
    {
        OutputTurn ot = planeObj.gameObject.GetComponent<OutputTurn>();
        if (ot != null)
        {
            turn = ot.outputTurn();
            isRoundBreak = ot.outputRoundBreak();
        }
    }

    //开启第二枪口
    public void secondBS()
    {
        BulletsSpawns[1].gameObject.SetActive(true);
    }

    //购买火子弹
    public void thirdBS()
    {
        if(Coin >= 1000)
        {
            Coin = Coin - 1000;
            BulletsSpawns[2].gameObject.SetActive(true);
            BuyButtons[0].gameObject.SetActive(false);
        }
        else
        {
            WarnPanel.gameObject.SetActive(true);
        }


    }
    public void forethBS()
    {
        if(Coin >= 2000)
        {
            Coin = Coin - 2000;
            BulletsSpawns[3].gameObject.SetActive(true);
            BuyButtons[1].gameObject.SetActive(false);
        }
        else
        {
            WarnPanel.gameObject.SetActive(true);
        }

    }

    public void HPLimitButton()
    {
        if(Coin >= 2000 )
        {
            Coin = Coin - 2000;
            HP += 100;
            currentHP = HP;
            BuyButtons[2].gameObject.SetActive(false);//购买一次后无法购买
        }
        else
        {
            WarnPanel.gameObject.SetActive(true);
        }
    }
    public void HPRecoverButton()
    {
        if (Coin >= 500)
        {
            if(currentHP != HP)
            {
                Coin = Coin - 500;
                if (currentHP + 50 > HP)
                {
                    currentHP = HP;
                }
                else
                {
                    currentHP += 50;
                }
            }
        }
        else
        {
            WarnPanel.gameObject.SetActive(true);
        }
    }

}