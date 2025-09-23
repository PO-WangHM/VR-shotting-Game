using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, getBulletValues
{
    [Header("Enemy Settings")]
    public float InitialHealth = 5f; // ���˳�ʼ����ֵ
    public float xpValue = 50f; // ���ܵ��˻�õľ���ֵ
    [Header("Movement Settings")]
    public float moveSpeed = 3f; // �����ƶ��ٶ�


    protected float currentHealth; // ��ǰ����ֵ
    protected float damage = 0f; // �ܵ����ӵ��˺�ֵ,δ�����ӵ��˺���Ϊ0

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = InitialHealth;
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
            // �ҵ�����ΪCanvas���󲢻�ȡ��getEnemyValue�ű����
            GameObject cavence = GameObject.Find("Canvas");
            if (cavence != null)
            {
                getEnemyValue gev = cavence.GetComponent<getEnemyValue>();
                if (gev != null)
                {
                    // ���ýӿڵ�TakeDamage����
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
