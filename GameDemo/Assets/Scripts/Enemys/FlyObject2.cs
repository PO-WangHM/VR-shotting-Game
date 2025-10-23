using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyObject2 : MonoBehaviour
{
    private BossEnemy parentBoss;
    private int projectileIndex;

    // �����������������ø������ú�����
    public void SetParentShooter(BossEnemy boss, int index)
    {
        parentBoss = boss;
        projectileIndex = index;
    }

    void OnTriggerEnter(Collider collision)
    {
        // ����Ƿ���ײ�����
        if (collision.gameObject.name == "Player")
        {
            // ֪ͨ�������÷��������������
            if (parentBoss != null)
            {
                parentBoss.ResetProjectile(projectileIndex);
            }
            else
            {
                // ����Ҳ���������ʹ�ñ������÷���
                ResetToPool();
            }
        }
        // �������÷���
        void ResetToPool()
        {
            // ���������������߼��������������򷵻ص������
            gameObject.SetActive(false);
        }
    }
}
