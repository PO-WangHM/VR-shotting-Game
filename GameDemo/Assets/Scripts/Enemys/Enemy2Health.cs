using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Health : Enemy
{

    public override void TakeDamage()
    {
        currentHealth -= damage/2;
        damage = 0; // 重置伤害值，防止持续扣血
    }
}
