using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface OutputPlayerValue
{
    void getXP(float xpValue);
    void getCoin(float coinValue);
    void getScore(float scoreValue);
    void getDamage(float currentDamageValue);
    void getContinueDamage(float countinuousDamage);
    void getContinueTime(float countinuousTime);
    float outputLevel();
}
