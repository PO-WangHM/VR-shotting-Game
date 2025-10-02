using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : BulletAttack, OutputIceBulletValues
{

    public float SlowTime = 2f;//减速时间
    public float SlowRate = 0.2f;//减速百分比
   
    public  float outputST()
    {
        return SlowTime;
    }

    public  float outputSR()
    {
        return SlowRate;
    }
}
