using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : BulletAttack, OutputFireBulletValues
{
    
    public float ICD = 5;//��ʼ�����˺�
    public float continuousTime = 3;//�����˺�ʱ��
    public int CDTimes = 5;//��ɳ����˺�����

    private float continuousDamage = 5;//�����˺�ֵ

    public override void Start()
    {
        base.Start();
        continuousDamage = ICD;
    }

    public override void Update()
    {
        base.Update();
        outputFireValues();
        continueDamgeChange();
    }


    public void outputFireValues()
    {
        playerObj = GameObject.Find("Player");
        OutputPlayerValue opv = playerObj.gameObject.GetComponent<OutputPlayerValue>();
        if(opv!=null)
        {
            opv.getContinueDamage(continuousDamage);
            opv.getContinueTime(continuousTime);

        }
    }

    public float outputCD()
    {
        return continuousDamage;
    }
    public float outputCT()
    {
        return continuousTime;
    }
    public float outputCDT()
    {
        float pertime = continuousTime / (CDTimes+1);
        return pertime;
    }
    public void continueDamgeChange()
    {
        continuousDamage = ICD + (float)(0.5 * (PlayerLevel - 1));
    }
}
