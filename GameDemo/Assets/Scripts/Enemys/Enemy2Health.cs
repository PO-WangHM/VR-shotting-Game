using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Health : MonoBehaviour, getBulletValues
{
    public float InitialHealth = 10; // ���˳�ʼ����ֵ



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
