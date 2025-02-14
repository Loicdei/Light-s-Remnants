using UnityEngine;


public class OpenPanelButton : MonoBehaviour
{
    [SerializeField] private PanelType type;

    private MenuController controller;

    public void OpenpanelOptions()
    {
        if (Input.GetButtonDown("CustomClick"))
        {
            controller.OpenPanel(PanelType.Options);
        }
    }

    public void OpenpanelCredits()
    {
        if (Input.GetButtonDown("CustomClick"))
        {
            controller.OpenPanel(PanelType.Credits);
        }
    }
}
