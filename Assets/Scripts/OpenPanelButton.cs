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
        if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Escape))
        {
            if(controller.GetCurrentPanelType() != PanelType.Main)
            {
                controller.OpenPanel(PanelType.Main);
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
