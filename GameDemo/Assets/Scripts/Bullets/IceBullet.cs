using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBullet : BulletAttack, OutputIceBulletValues
{

    public float SlowTime = 2f;//����ʱ��
    public float SlowRate = 0.2f;//���ٰٷֱ�
   
    public  float outputST()
    {
        return SlowTime;
    }

    public  float outputSR()
    {
        return SlowRate;
    }
}
