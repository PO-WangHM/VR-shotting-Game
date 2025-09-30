using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : BulletAttack,OutputBulletValues
{

    public float SlowTime = 2f;//����ʱ��
    public float SlowRate = 0.2f;//���ٰٷֱ�
   
    public override float outputST()
    {
        return SlowTime;
    }

    public override float outputSR()
    {
        return SlowRate;
    }
}
