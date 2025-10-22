using UnityEngine;
using UnityEngine.InputSystem;

public class LeftHandButtonHandler : MonoBehaviour
{
    public InputActionReference leftPrimaryButtonAction; // �������ֱ�Primary Button����
    public GameObject pausePanel; // ��Inspector�з���PausePanel

    private void OnEnable()
    {
        // �������붯����ע��ص�����
        leftPrimaryButtonAction.action.Enable();
        leftPrimaryButtonAction.action.performed += OnPrimaryButtonPressed;
    }

    private void OnDisable()
    {
        // �������붯����ȡ��ע��ص�����
        leftPrimaryButtonAction.action.performed -= OnPrimaryButtonPressed;
        leftPrimaryButtonAction.action.Disable();
    }

    private void OnPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        // ������������ʱ���л�PausePanel�ļ���״̬
        if (pausePanel != null)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
    }
}