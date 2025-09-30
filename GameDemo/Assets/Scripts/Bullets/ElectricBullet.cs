using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBullet : BulletAttack
{
    public int attackTimes = 2;//��͸����

    

    private int count = 0;//������ײ����
    public override void OnTriggerEnter(Collider collision)
    {
        
        // ����Ƿ���ײ������
        if (collision.gameObject.CompareTag("Enemy"))
        {
            count++;
            //currentDamageValue = currentDamageValue / 2;
            if (count == attackTimes )
            {
                
                // ��ײ�������ӵ�
                Destroy(gameObject);
            }
            
        }
    }
}
