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
            Debug.LogError("MenuController introuvable ! Vérifiez qu'il est bien dans la scène.");
        }
    }
    private void Update()
    {
        if (type != PanelType.Main && Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Escape))
        {
            controller.OpenPanel(PanelType.Main);

            // Vérifier si un bouton est déjà sélectionné pour éviter la réinitialisation
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(null); // Ne change pas la sélection
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
