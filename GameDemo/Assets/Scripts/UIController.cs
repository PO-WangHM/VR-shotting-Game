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
        // ��ʼ���ȼ��;���ֵ
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

    //��ȡ���˾���ֵ
    public void getXP(float xp)
    {
        EnamyXP = xp;
    }
    //���¾���ֵ
    public void UpdateXP()
    {
        currentXP += EnamyXP;
        //�����ж�
        if (currentXP >= XPToNextLevel)
        {
            currentLevel++;
            LevelText.text = "Level: " + currentLevel.ToString();
            currentXP -= XPToNextLevel; // �۳��������辭��ֵ���������ྭ��ֵ
            XPToNextLevel *= 1.2f; // ÿ����������һ�ȼ����辭��ֵ����50%
            print("Level Up! New Level: " + currentLevel);
        }
        XPText.text = "XP: " + currentXP.ToString();
        EnamyXP = 0f; // ���õ��˾���ֵ����ֹ��������
    }

}
