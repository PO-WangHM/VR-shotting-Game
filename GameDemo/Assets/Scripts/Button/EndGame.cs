using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

    public void RestartGame()
    {
        Debug.Log("���¿�ʼ��Ϸ...");

        // ���¼��ص�ǰ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // ����ʹ�ó������ƣ����֪���Ļ�����
        // SceneManager.LoadScene("YourSceneName");
    }


    public void ExitGame()
    {
        Debug.Log("�˳���Ϸ��...");

#if UNITY_EDITOR
        // �ڱ༭����ֹͣ����
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �ڹ����汾���˳�Ӧ��
        Application.Quit();
#endif
    }

    public void close()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

}
