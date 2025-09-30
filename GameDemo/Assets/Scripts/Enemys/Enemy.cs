using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float InitialHealth = 5f; // ���˳�ʼ����ֵ
    public float xpValue = 50f; // ���ܵ��˻�õľ���ֵ
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // �����ƶ��ٶ�


    protected float currentHealth; // ��ǰ����ֵ
    protected float damage = 0f; // �ܵ����ӵ��˺�ֵ,δ�����ӵ��˺���Ϊ0

    private float currentspeed = 0f;
    private float ST = 0;
    private float SR = 0;
    private float timer;//���ӵ����ټ�ʱ��

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = InitialHealth;
        currentspeed = moveSpeed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        // ÿ֡�������˷���
        TakeDamage();

        //�����ƶ�
        Move();

        // ������ֵС�ڵ���0ʱ��������
        Die();

        //����Ч��
        if(ST!=0)
        {
            if(SR!= 0)
            {
                SlowDown();
            }
            
            timer += Time.deltaTime;
            if(timer>=ST)
            {
                ST = 0;
                timer = 0;
                currentspeed = moveSpeed;
            }
        }
        

    }
     
    //��ȡ�ӵ��˺�ֵ
    public void GetDamageValue(float damageValue)
    {
        damage = damageValue;
    }
    //��������ܵ��˺�
    public virtual void TakeDamage()
    {
        currentHealth -= damage;
        damage = 0; // �����˺�ֵ����ֹ������Ѫ
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
            // ��ȡ�ӵ��˺�ֵ
            OutputBulletValues obv = collision.gameObject.GetComponent<OutputBulletValues>();

            if (obv != null)
            {
                // ���ýӿڵ�TakeDamage����
                damage = obv.outputDamage();
            }
        }
        if (collision.gameObject.name.Contains("IceBullet"))
        {
            OutputBulletValues obv = collision.gameObject.GetComponent<OutputBulletValues>();

           
            if (obv != null)
            {
                // ���ýӿڵ�TakeDamage����
                ST = obv.outputST();
                SR = obv.outputSR();
                timer = 0;
            }
            
        }

    }

    void SlowDown()
    {
        currentspeed = currentspeed * SR;
        SR = 0;
    }
}
