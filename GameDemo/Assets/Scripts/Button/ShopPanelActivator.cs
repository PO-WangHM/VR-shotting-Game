using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ShopPanelActivator : MonoBehaviour
{
    public InputActionReference rightPrimaryButtonAction; // �������ֱ�Primary Button����
    public GameObject shopPanel; // ��Inspector�з���ShopPanel

    private void OnEnable()
    {
        // �������붯����ע��ص�����
        rightPrimaryButtonAction.action.Enable();
        rightPrimaryButtonAction.action.performed += OnPrimaryButtonPressed;
    }

    private void OnDisable()
    {
        // �������붯����ȡ��ע��ص�����
        rightPrimaryButtonAction.action.performed -= OnPrimaryButtonPressed;
        rightPrimaryButtonAction.action.Disable();
    }

    private void OnPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        // ������������ʱ������ShopPanel
        if (shopPanel != null)
        {
            Time.timeScale = 0f;
            shopPanel.SetActive(true);
        }
    }
}