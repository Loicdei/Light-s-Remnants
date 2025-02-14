using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public class MenuPanel : MonoBehaviour
{
    [SerializeField] private PanelType type;

    private bool state;
    private bool usedMouseLast = false;
    private bool mouseWasHidden = false; // Indique si la souris était cachée

    [Header("Configuration : Navigation")]
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private Button rightPanel, leftPanel;

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

    private void Update()
    {
        DetectInputMethod();
    }

    private void UpdateState()
    {
        canvas.enabled = state;
        if (state) controller.SetSelectedObject(selectedGameObject, rightPanel, leftPanel);
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
    }

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

        if (selectedGameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(selectedGameObject);
        }
    }

    #region Getter

    public PanelType GetPanelType() { return type; }

    #endregion
}
