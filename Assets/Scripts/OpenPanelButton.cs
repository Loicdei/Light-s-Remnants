using UnityEngine;
using UnityEngine.EventSystems;


public class OpenPanelButton : MonoBehaviour
{
    [SerializeField] private PanelType type;

    private MenuController controller;
    private void Start()
    {
        controller = FindObjectOfType<MenuController>();
        if (controller == null)
        {
            Debug.LogError("MenuController introuvable ! V�rifiez qu'il est bien dans la sc�ne.");
        }
    }
    private void Update()
    {
        if (type != PanelType.Main && Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Escape))
        {
            controller.OpenPanel(PanelType.Main);

            // V�rifier si un bouton est d�j� s�lectionn� pour �viter la r�initialisation
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(null); // Ne change pas la s�lection
            }
        }
    }

    public void OpenpanelOptions()
    {
        controller.OpenPanel(PanelType.Options);
    }

    public void OpenpanelCredits()
    {
        controller.OpenPanel(PanelType.Credits);
    }

    public void OpenMainPanel()
    {
        controller.OpenPanel(PanelType.Main);
    }

}
