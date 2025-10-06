using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyObject : MonoBehaviour
{
    private ShooterEnemy parentShooter;

    // 公共方法，用于设置父级引用
    public void SetParentShooter(ShooterEnemy shooter)
    {
        parentShooter = shooter;
    }

    void OnTriggerEnter(Collider collision)
    {
        // 检查是否碰撞到玩家
        if (collision.gameObject.name == "Player")
        {
            // 通知父级重置发射物，而不是销毁
            if (parentShooter != null)
            {
                parentShooter.ResetProjectile();
            }
            else
            {
                // 如果找不到父级，使用备用重置方法
                ResetToPool();
            }
        }
    }

    // 备用重置方法
    void ResetToPool()
    {
        // 这里可以添加重置逻辑，比如禁用物体或返回到对象池
        gameObject.SetActive(false);
    }
}