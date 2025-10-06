using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour, OutputBulletValues
{
    public float InitialDamageValue = 5f; // �ӵ���ʼ�˺�

    protected float currentDamageValue; // ��ǰ�˺�ֵ

    //�����Ϣ
    protected GameObject playerObj;
    private float PlayerLevel = 1;//��ҵȼ�

    public virtual void Start()
    {
        currentDamageValue = InitialDamageValue;
    }

    public virtual void Update()
    {
        DamageCalculate();
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

    public void DamageCalculate()
    {
        playerObj = GameObject.Find("Player");
        OutputPlayerValue opv = playerObj.gameObject.GetComponent<OutputPlayerValue>();
        if(opv!=null)
        {
            PlayerLevel = opv.outputLevel();
        }
        currentDamageValue = InitialDamageValue + (2 * (PlayerLevel - 1));
        opv.getDamage(currentDamageValue);
    }
    public float outputDamage()
    {
        return currentDamageValue;
    }
}
