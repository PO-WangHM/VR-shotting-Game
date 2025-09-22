using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour
{
    public float InitialDamageValue = 5f; // �ӵ���ʼ�˺�

    private float currentDamageValue; // ��ǰ�˺�ֵ


    void Start()
    {
        currentDamageValue = InitialDamageValue;
    }
    
    void Update()
    {

    }

    // ���ӵ���ײ������ʱ����
    void OnTriggerEnter(Collider collision)
    {
        // ����Ƿ���ײ������
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("Bullet hit Enemy");
            // ��ȡ���˵��˺��ӿڽű�
            getBulletValues gbv = collision.gameObject.GetComponent<getBulletValues>();

            if (gbv != null)
            {
                // ���ýӿڵ�TakeDamage����
                gbv.GetDamageValue(currentDamageValue);
            }

            // ��ײ�������ӵ�
            Destroy(gameObject);
        }
    }
}
