using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : BulletAttack, OutputFireBulletValues
{
    
    public float ICD = 5;//初始持续伤害
    public float continuousTime = 3;//持续伤害时间
    public int CDTimes = 5;//造成持续伤害次数

    private float continuousDamage = 5;//持续伤害值

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
