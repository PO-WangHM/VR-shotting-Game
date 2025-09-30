using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBullet : BulletAttack
{
    public int attackTimes = 2;//穿透次数

    

    private int count = 0;//计入碰撞次数
    public override void OnTriggerEnter(Collider collision)
    {
        
        // 检查是否碰撞到敌人
        if (collision.gameObject.CompareTag("Enemy"))
        {
            count++;
            //currentDamageValue = currentDamageValue / 2;
            if (count == attackTimes )
            {
                
                // 碰撞后销毁子弹
                Destroy(gameObject);
            }
            
        }
    }
}
