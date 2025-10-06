using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float InitialHealth = 5f; // ���˳�ʼ����ֵ
    public float xpValue = 50f; // ���ܵ��˻�õľ���ֵ
    public float coinValue = 5; //���ܵ��˻�ý��
    public float scoreValue = 10; //���ܵ������ӵ÷�
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // �����ƶ��ٶ�


    protected float currentHealth; // ��ǰ����ֵ
    protected float damage = 0f; // �ܵ����ӵ��˺�ֵ,δ�����ӵ��˺���Ϊ0

    //���������Ϣ
    GameObject playerObj; 
    

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
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
        // ÿ֡�������˷���
        TakeDamage();

        //�����ƶ�
        Move();

        // ������ֵС�ڵ���0ʱ��������
        Die();

        //���ӵ�������ɳ����˺�Ч��
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

        //���ӵ�������ɼ���Ч��
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
     
    //��������ܵ��˺�
    public virtual void TakeDamage()
    {
        currentHealth -= damage;
        damage = 0; // �����˺�ֵ����ֹ������Ѫ
    }

    //��������
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

    //�����ƶ�
    public virtual void Move()
    {
        transform.Translate(Vector3.back * currentspeed * Time.deltaTime, Space.World);
    }

    //��ײ���
    public virtual void OnTriggerEnter(Collider collision)
    {
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

        ////�����������ӵ����ȡ���ӵ�����ֵ
        if (collision.gameObject.name.Contains("FireBullet"))
        {
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
            OutputIceBulletValues oibv = collision.gameObject.GetComponent<OutputIceBulletValues>();

           
            if (oibv != null)
            {
                // ͨ���ӿڻ�ȡ���ӵ�������ֵ
                ST = oibv.outputST();
                SR = oibv.outputSR();
                Icetimer = 0;
            }
            
        }

    }

    //�������ӵ������˺�Ч��
    void TakeContinuousDamage()
    {
        currentHealth -= CD;  
    }

    //�������ӵ�����Ч��
    void SlowDown()
    {
        currentspeed = currentspeed * SR;
        SR = 0;
    }
}
