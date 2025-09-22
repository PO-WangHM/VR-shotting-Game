using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, getEnemyValue
{
    public Text XPText;
    public Text LevelText;

    private int currentLevel;
    private float XPToNextLevel;
    private float EnamyXP;
    private float currentXP;
    // Start is called before the first frame update
    void Start()
    {
        // 初始化等级和经验值
        currentLevel = 1;
        XPToNextLevel = 100f;
        EnamyXP = 0f;
        currentXP = 0f;
}

    // Update is called once per frame
    void Update()
    {
        UpdateXP();
    }

    //获取敌人经验值
    public void getXP(float xp)
    {
        EnamyXP = xp;
    }
    //更新经验值
    public void UpdateXP()
    {
        currentXP += EnamyXP;
        //升级判断
        if (currentXP >= XPToNextLevel)
        {
            currentLevel++;
            LevelText.text = "Level: " + currentLevel.ToString();
            currentXP -= XPToNextLevel; // 扣除升级所需经验值，保留多余经验值
            XPToNextLevel *= 1.2f; // 每次升级后，下一等级所需经验值增加50%
            print("Level Up! New Level: " + currentLevel);
        }
        XPText.text = "XP: " + currentXP.ToString();
        EnamyXP = 0f; // 重置敌人经验值，防止持续增加
    }

}
