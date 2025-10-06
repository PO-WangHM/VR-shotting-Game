using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyObject : MonoBehaviour
{
    private ShooterEnemy parentShooter;

    // �����������������ø�������
    public void SetParentShooter(ShooterEnemy shooter)
    {
        parentShooter = shooter;
    }

    void OnTriggerEnter(Collider collision)
    {
        // ����Ƿ���ײ�����
        if (collision.gameObject.name == "Player")
        {
            // ֪ͨ�������÷��������������
            if (parentShooter != null)
            {
                parentShooter.ResetProjectile();
            }
            else
            {
                // ����Ҳ���������ʹ�ñ������÷���
                ResetToPool();
            }
        }
    }

    // �������÷���
    void ResetToPool()
    {
        // ���������������߼��������������򷵻ص������
        gameObject.SetActive(false);
    }
}