using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, getBulletValues
{
    public float InitialHealth = 5f; // ���˳�ʼ����ֵ
    public float xpValue = 50f; // ���ܵ��˻�õľ���ֵ


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

        // ÿ֡��鵱ǰ����ֵ,������ֵС�ڵ���0ʱ���ٵ���
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
}
