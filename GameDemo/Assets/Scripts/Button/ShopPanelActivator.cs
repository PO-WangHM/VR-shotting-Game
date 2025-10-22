using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ShopPanelActivator : MonoBehaviour
{
    public InputActionReference rightPrimaryButtonAction; // 引用右手柄Primary Button动作
    public GameObject shopPanel; // 在Inspector中分配ShopPanel

    private void OnEnable()
    {
        // 启用输入动作并注册回调函数
        rightPrimaryButtonAction.action.Enable();
        rightPrimaryButtonAction.action.performed += OnPrimaryButtonPressed;
    }

    private void OnDisable()
    {
        // 禁用输入动作并取消注册回调函数
        rightPrimaryButtonAction.action.performed -= OnPrimaryButtonPressed;
        rightPrimaryButtonAction.action.Disable();
    }

    private void OnPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        // 当按键被按下时，激活ShopPanel
        if (shopPanel != null)
        {
            Time.timeScale = 0f;
            shopPanel.SetActive(true);
        }
    }
}