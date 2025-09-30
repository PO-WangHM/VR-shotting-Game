using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : BulletAttack,OutputBulletValues
{

    public float SlowTime = 2f;//减速时间
    public float SlowRate = 0.2f;//减速百分比
   
    public override float outputST()
    {
        return SlowTime;
    }

    public override float outputSR()
    {
        return SlowRate;
    }
}
