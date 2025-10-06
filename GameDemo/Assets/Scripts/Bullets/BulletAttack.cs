using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour, OutputBulletValues
{
    public float InitialDamageValue = 5f; // 子弹初始伤害

    protected float currentDamageValue; // 当前伤害值

    //玩家信息
    protected GameObject playerObj;
    private float PlayerLevel = 1;//玩家等级

    public virtual void Start()
    {
        currentDamageValue = InitialDamageValue;
    }

    public virtual void Update()
    {
        DamageCalculate();
    }
    // 当子弹碰撞到敌人时触发
    public virtual void OnTriggerEnter(Collider collision)
    {
        // 检查是否碰撞到敌人
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 碰撞后销毁子弹
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
