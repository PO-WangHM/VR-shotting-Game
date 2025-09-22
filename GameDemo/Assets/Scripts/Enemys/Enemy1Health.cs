using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy1Health : MonoBehaviour, getBulletValues
{
    public float InitialHealth = 5f; // ���˳�ʼ����ֵ
    public float xpValue = 50f; // ���ܵ��˻�õľ���ֵ


    private float currentHealth; // ��ǰ����ֵ
    private float damage = 0f; // �ܵ����ӵ��˺�ֵ,�����ӵ��˺���Ϊ0

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = InitialHealth;
    }

    // Update is called once per frame
    void Update()
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
    public void TakeDamage()
    {
        currentHealth -= damage; 
        damage = 0; // �����˺�ֵ����ֹ������Ѫ
    }
    
}
