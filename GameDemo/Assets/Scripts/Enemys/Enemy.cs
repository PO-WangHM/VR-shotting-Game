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
    public float InitialHealth = 5f; // ���˳�ʼ����ֵ
    public float IxpValue = 50f; // ���ܵ��˻�õľ���ֵ
    public float IcoinValue = 5; //���ܵ��˻�ý��
    public float IscoreValue = 10; //���ܵ������ӵ÷�
    public float distroyTime = 40f;//�Զ�����ʱ��
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // �����ƶ��ٶ�

    [Header("Audio Setting")]
    public AudioSource audioSource;//��Ч���
    public AudioClip[] audioClips;//��Ч

    protected float xpValue = 0; //���ﵱǰ����ֵ
    protected float coinValue = 5; //���ܵ��˻�ý��
    protected float scoreValue = 10; //���ܵ������ӵ÷�
    protected float currentTurnHealth;//��ǰ�ִε��������
    protected float currentHealth; // ��ǰ����ֵ
    protected float damage = 0f; // �ܵ����ӵ��˺�ֵ,δ�����ӵ��˺���Ϊ0
    private float distroyTimer = 0f;

    // �������״̬��ʶ
    private bool isDead = false;
    private bool hasRotated = false; // ����Ƿ��Ѿ���ת

    //���������Ϣ
    GameObject playerObj;
    GameObject planeObj;

    //�ִ���Ϣ
    protected int turn = 1;
    private bool ValueisChange = false;

    //���ջ������ӵ����Բ���
    protected float CD = 0; //�����˺�ֵ
    protected float CT = 0; //�����˺�����ʱ��
    protected float pertime = 0;//����ʱ�����һ�γ����˺�
    protected float Firetimer1 = 0;//���ӵ������˺���ʱ��
    protected float Firetimer2 = 0;//���ӵ�����ʱ����ʱ��

    //���ձ������ӵ����Բ���
    protected float currentspeed = 0f;//���ﵱǰ�ٶ�
    protected float ST = 0;   //����Ч������ʱ��
    protected float SR = 0;   //���ٱ���
    protected float Icetimer = 0;//���ӵ����ټ�ʱ��

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
        // ����Ѿ�����������ִ�������߼�
        if (isDead) return;

        // ÿ֡�������˷���
        TakeDamage();

        //�����ƶ�
        Move();

        //��ȡ�ִ�
        getTurn();
        if (!ValueisChange)
        {
            ValueChange();
            ValueisChange = true;
        }

        //���ӵ�������ɳ����˺�Ч��
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

        //���ӵ�������ɼ���Ч��
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

        // ������ֵС�ڵ���0ʱ��������
        distroyTimer += Time.deltaTime;
        if (distroyTimer >= distroyTime)
        {
            currentHealth = 0;
        }
        Die();
    }

    //��������ܵ��˺�
    public virtual void TakeDamage()
    {
        currentHealth -= damage;
        damage = 0; // �����˺�ֵ����ֹ������Ѫ
        UpdateHealthBar();//����Ѫ��
    }

    //��������
    public virtual void Die()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;

            // �ر���ײ��
            Collider collider = GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;

            // ֹͣ�ƶ�
            currentspeed = 0f;

            // ����������Ч
            audioSource.PlayOneShot(audioClips[4]);

            // ����ҽ���
            playerObj = GameObject.Find("Player");
            OutputPlayerValue opv = playerObj.gameObject.GetComponent<OutputPlayerValue>();
            if (opv != null)
            {
                opv.getXP(xpValue);
                opv.getCoin(coinValue);
                opv.getScore(scoreValue);
            }

            // ֪ͨ������
            FindObjectOfType<EnemySpawn>().EnemyDefeated();

            // ִ��������ת
            PerformDeathRotation();

            // 1�����������
            StartCoroutine(DestroyAfterDelay(1f));
        }
    }

    // ִ��������ת
    private void PerformDeathRotation()
    {
        if (!hasRotated)
        {
            // ��X����ת90��
            transform.Rotate(90f, 0f, 0f);
            hasRotated = true;
        }
    }

    // �ӳ�����Э��
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    //�����ƶ�
    public virtual void Move()
    {
        transform.Translate(Vector3.back * currentspeed * Time.deltaTime, Space.World);
    }

    //��ײ���
    public virtual void OnTriggerEnter(Collider collision)
    {
        // ����Ѿ����������ٴ�����ײ
        if (isDead) return;

        //�����ӵ����ȡ�ӵ�����ֵ
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // ͨ���ӿ��ӵ��˺�ֵ
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

        ////�����������ӵ����ȡ���ӵ�����ֵ
        if (collision.gameObject.name.Contains("FireBullet"))
        {
            audioSource.PlayOneShot(audioClips[1]);
            OutputFireBulletValues ofbv = collision.gameObject.GetComponent<OutputFireBulletValues>();
            if (ofbv != null)
            {
                // ͨ���ӿڻ�ȡ���ӵ�������ֵ
                CD = ofbv.outputCD();
                CT = ofbv.outputCT();
                pertime = ofbv.outputCDT();
                Firetimer2 = 0;
            }
        }

        //�����������ӵ����ȡ���ӵ�����ֵ
        if (collision.gameObject.name.Contains("IceBullet"))
        {
            audioSource.PlayOneShot(audioClips[2]);
            OutputIceBulletValues oibv = collision.gameObject.GetComponent<OutputIceBulletValues>();


            if (oibv != null)
            {
                // ͨ���ӿڻ�ȡ���ӵ�������ֵ
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

    //�������ӵ������˺�Ч��
    void TakeContinuousDamage()
    {
        currentHealth -= CD;
        UpdateHealthBar();//����Ѫ��
    }

    //�������ӵ�����Ч��
    void SlowDown()
    {
        currentspeed = currentspeed * SR;
        SR = 0;
    }

    //����Ѫ����ʼ��
    protected virtual void InitializeHealthBar()
    {
        // ���δ�ֶ���ֵ�������Զ�����
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / currentTurnHealth;

            // ��ѡ����ʼʱ����Ѫ��������ʱ����ʾ
            // healthSlider.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Health Slider not found on enemy: " + gameObject.name);
        }
    }

    // ���¹���Ѫ��
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

    //��ȡ�ִ���Ϣ
    void getTurn()
    {
        planeObj = GameObject.Find("Plane");
        OutputTurn ot = planeObj.gameObject.GetComponent<OutputTurn>();
        if (ot != null)
        {
            turn = ot.outputTurn();
        }
    }

    //������ֵ����
    void ValueChange()
    {
        currentTurnHealth = (float)(InitialHealth * (1 + 0.5 * (turn - 1)));
        currentHealth = currentTurnHealth;
        xpValue = (float)(IxpValue * (1 + 0.25 * (turn - 1)));
        coinValue = (float)(IcoinValue * (1 + 0.25 * (turn - 1)));
        scoreValue = (float)(IscoreValue * (1 + 0.25 * (turn - 1)));
    }
}