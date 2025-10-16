using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

    public void RestartGame()
    {
        Debug.Log("重新开始游戏...");

        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // 或者使用场景名称（如果知道的话）：
        // SceneManager.LoadScene("YourSceneName");
    }


    public void ExitGame()
    {
        Debug.Log("退出游戏中...");

#if UNITY_EDITOR
        // 在编辑器中停止播放
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在构建版本中退出应用
        Application.Quit();
#endif
    }

    public void close()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

}
