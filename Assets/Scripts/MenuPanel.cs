using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]  

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private PanelType type;

    private bool state;

    [Header("Configuration : Navigation")]
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private Button rightPanel, leftPanel;

    private Canvas canvas;

    private MenuController controller;


    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void init(MenuController _controller){ controller = _controller; }

    private void UpdateState()
    {
        canvas.enabled = state;
        if(state) controller.SetSelectedObject(selectedGameObject, rightPanel, leftPanel);
    }

    //Permet d'ouvrir un Panel lorsque aucun panel n'est ouvert
    public void ChangeState()
    {
        state = !state;
        UpdateState();
    }

    public void ChangeState(bool _state)
    {
        state = _state;
        UpdateState();
    }

    //Permet d'utiliser SerializedField
    #region Getter

    public PanelType GetPanelType() { return type; }

    #endregion
}
