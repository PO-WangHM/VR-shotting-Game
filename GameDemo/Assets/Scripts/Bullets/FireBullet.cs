using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : BulletAttack, OutputFireBulletValues
{
    public float countinuousDamage = 5;//持续伤害值
    public float countinuousTime = 3;//持续伤害时间
    public int CDTimes = 5;//造成持续伤害次数

    public float outputCD()
    {
        return countinuousDamage;
    }
    public float outputCT()
    {
        return countinuousTime;
    }
    public float outputCDT()
    {
        float pertime = countinuousTime / (CDTimes+1);
        return pertime;
    }
}
