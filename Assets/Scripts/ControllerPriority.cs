using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerPriority : MonoBehaviour
{
    private bool usedMouseLast = false;
    private bool mouseWasHidden = false; // Indique si la souris était cachée
    public GameObject firstButton;
    private void DetectInputMethod()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            usedMouseLast = true;
            if (mouseWasHidden) // Si la souris était cachée, on la remet visible
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                mouseWasHidden = false;
            }
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 ||
            Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
        {
            if (usedMouseLast)
            {
                usedMouseLast = false;
                HideMouseAndSelectButton();
            }
        }
    }

    private void HideMouseAndSelectButton()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mouseWasHidden = true;

        if (firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }
}
