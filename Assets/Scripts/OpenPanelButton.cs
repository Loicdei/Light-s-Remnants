using UnityEngine;


public class OpenPanelButton : MonoBehaviour
{
    [SerializeField] private PanelType type;

    [SerializeField] private OpenPanelButton onSwitchBackAction;

    private MenuController controller;

    private MenuInput inputs;

    void Start()
    {
      controller = FindObjectOfType<MenuController>();  
      inputs = controller.GetComponent<MenuInput>();
    }

    public void OnClick()
    {
        controller.OpenPanel(type);
        if (onSwitchBackAction != null) inputs.SetBackListener(onSwitchBackAction.OnClick);
        else inputs.SetBackListener();
    }

}
