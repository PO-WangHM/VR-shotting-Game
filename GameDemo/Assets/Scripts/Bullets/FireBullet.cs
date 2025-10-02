using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : BulletAttack, OutputFireBulletValues
{
    public float countinuousDamage = 5;//�����˺�ֵ
    public float countinuousTime = 3;//�����˺�ʱ��
    public int CDTimes = 5;//��ɳ����˺�����

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
