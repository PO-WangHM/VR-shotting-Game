using UnityEngine;
using UnityEngine.InputSystem;

public class LeftHandButtonHandler : MonoBehaviour
{
    public InputActionReference leftPrimaryButtonAction; // 引用左手柄Primary Button动作
    public GameObject pausePanel; // 在Inspector中分配PausePanel

    private void OnEnable()
    {
        // 启用输入动作并注册回调函数
        leftPrimaryButtonAction.action.Enable();
        leftPrimaryButtonAction.action.performed += OnPrimaryButtonPressed;
    }

    private void OnDisable()
    {
        // 禁用输入动作并取消注册回调函数
        leftPrimaryButtonAction.action.performed -= OnPrimaryButtonPressed;
        leftPrimaryButtonAction.action.Disable();
    }

    private void OnPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        // 当按键被按下时，切换PausePanel的激活状态
        if (pausePanel != null)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
    }
}