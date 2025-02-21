using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public class MenuPanel : MonoBehaviour
{
    [SerializeField] private PanelType type;

    private bool state;

    [Header("Configuration : Navigation")]
    [SerializeField] private Button rightPanel, leftPanel;
    [SerializeField] public GameObject firstButton;

    private Canvas canvas;
    private MenuController controller;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void Init(MenuController _controller)
    {
        controller = _controller;
    }

    private void SetButtonsInteractable(bool active)
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.interactable = active;
            button.gameObject.SetActive(true); // Assure-toi que les boutons sont visibles et activés
        }
    }

    private void UpdateState()
    {
        canvas.enabled = state;
        if (state && firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }

    public void ChangeState()
    {
        state = !state;
        UpdateState();
    }

    public void ChangeState(bool _state)
    {
        state = _state;
        UpdateState();
        SetButtonsInteractable(_state); // Active/désactive uniquement l'interaction des boutons
    }

    #region Getter

    public PanelType GetPanelType() { return type; }

    #endregion
}
