using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, OutputPlayerValue
{
    public float HP = 100;

    public Text xpText; //����չʾ
    public Text coinText; //���չʾ
    public Text scoreText;//�÷�չʾ
    public Text levelText;//�ȼ�չʾ
    public Text damageText;//�ӵ��˺�չʾ
    public Text hpText;//Ѫ��չʾ
    public Text turnText;//�ִ�չʾ
    public GameObject GameOverPanel;//�������չʾ
    //�������ӵ����չʾ
    public Text continueText;//�����˺�
    public Text continuetimeText;//�����˺�ʱ��

    private float XP = 0;    //����
    private float levelupXP = 100;//�������辭��
    private int Level = 1;  //�ȼ�
    private float Coin = 0;  //���
    private float Score = 0; //�÷�
    private float timer = 0;//��ʱ��
    private float damage = 0;//�ӵ������˺�
    //�������ӵ�����ֵ
    private float continuedamage = 0;
    private float continuetime = 0;

    //�ִ���Ϣ;
    private GameObject planeObj;
    private int turn;
    private bool isRoundBreak = false;

    // Start is called before the first frame update
    void Start()
    {
        planeObj = GameObject.Find("Plane");
    }

    // Update is called once per frame
    void Update()
    {
        Die();
        LevelUp();
        ShowValue();
        Continue();
        getTurn();
    }

    public virtual void OnTriggerEnter(Collider collision)
    {
        // ����Ƿ���ײ������
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HP -= 100;
            FindObjectOfType<EnemySpawn>().EnemyDefeated();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name.Contains("flyobject"))
        {
            HP -= 25;
        }
    }

    void Die()
    {
        if (HP <= 0)
        {
            Time.timeScale = 0f;
            GameOverPanel.gameObject.SetActive(true);
        }
    }

    //����
    void LevelUp()
    {
        if (XP >= levelupXP)
        {
            Level++;
            XP = XP - levelupXP;
            levelupXP = levelupXP + 50;
        }
    }

    //����ʱ�䲻�����ӵ÷��ڽ��
    void Continue()
    {
        // ��������ִμ���������ӽ�Һ͵÷�
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

    //��ֵչʾ
    void ShowValue()
    {
        xpText.text = "XP: " + XP;
        coinText.text = "Coin: " + Coin;
        scoreText.text = "Score: " + Score;
        levelText.text = "Level: " + Level;
        damageText.text = "Damage: " + damage;
        continueText.text = "Continuous Damage: " + continuedamage;
        continuetimeText.text = "Continuous Time: " + continuetime;
        hpText.text = "HP: " + HP;
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

    //��ȡ�ִ�
    void getTurn()
    {
        OutputTurn ot = planeObj.gameObject.GetComponent<OutputTurn>();
        if (ot != null)
        {
            turn = ot.outputTurn();
            isRoundBreak = ot.outputRoundBreak();
        }
    }
}