using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour, OutputBulletValues
{
    public float InitialDamageValue = 5f; // �ӵ���ʼ�˺�

    protected float currentDamageValue; // ��ǰ�˺�ֵ


    protected virtual void Start()
    {
        currentDamageValue = InitialDamageValue;
    }

    protected virtual void Update()
    {

    }
    // ���ӵ���ײ������ʱ����
    public virtual void OnTriggerEnter(Collider collision)
    {
        // ����Ƿ���ײ������
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ��ײ�������ӵ�
            Destroy(gameObject);
            
        }
    }
    public float outputDamage()
    {
        return currentDamageValue;
    }
}
