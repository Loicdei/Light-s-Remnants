using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public enum PanelType
{
    None,
    Main,
    Options,
    Credits,
}

public class MenuController : MonoBehaviour
{
    //Permet d'enregistrer et de faciliter l'ajout de panels depuis Unity (utilisation de List & Dictionary)
    [Header("Panels")]
    [SerializeField] private List<MenuPanel> panelsList = new List<MenuPanel>();
    private Dictionary<PanelType, MenuPanel> panelsDict = new Dictionary<PanelType, MenuPanel>();

    [SerializeField] private EventSystem eventController;

    private GameManager manager;
    private MenuInput inputs;

    private Animator fadeSystem;

    private Button[] buttons;

    private void Start()
    {
        manager = GameManager.instance;
        inputs = GetComponent<MenuInput>();
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();

        //Permet l'ajout de panels dans la liste
        foreach (var _panel in panelsList)
        {
            if (_panel) panelsDict.Add(_panel.GetPanelType(), _panel);
            _panel.init(this);
        }

        //Ouvre par d�faut le Main panel
        OpenOnePanel(PanelType.Main);
    }
    private void Awake()
    {
        buttons = FindObjectsOfType<Button>();
    }

    private void OpenOnePanel(PanelType _type)
    {
        foreach (var _panel in panelsList) _panel.ChangeState(false);

        if (_type != PanelType.None) panelsDict[_type].ChangeState(true);
    }

    public void OpenPanel(PanelType _type)
    {
        OpenOnePanel(_type);
    }
    public void StartSceneChange()
    {
        StartCoroutine(SceneChangeCoroutine());
    }

    private void SetButtonsInteractable(bool interactable)
    {
        foreach (var button in buttons)
        {
            button.interactable = interactable;
        }
    }

    public IEnumerator SceneChangeCoroutine()
    {
        // Désactiver tous les boutons
        SetButtonsInteractable(false);

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "MenuStatic")
        {
            Time.timeScale = 0;
            fadeSystem.SetTrigger("FadeIn");
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1;
        }
        manager.ChangeScene("MenuJouable");
        SetButtonsInteractable(true);
    }

     public void StartSceneChangeContinue()
    {
        StartCoroutine(SceneChangeCoroutine());
    }

    public IEnumerator SceneChangeCoroutineContinue()
    {
        // Désactiver tous les boutons
        SetButtonsInteractable(false);

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "MenuStatic")
        {
            Time.timeScale = 0;
            fadeSystem.SetTrigger("FadeIn");
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1;
        }
        manager.ChangeScene("MenuJouable");
        SetButtonsInteractable(true);
    }

    public void Quit()
    {
        manager.Quit();
    }

    public void SetSelectedObject(GameObject _element, Button _rightPanel, Button _leftPanel)
    {
        eventController.SetSelectedGameObject(_element);

        if (_rightPanel != null) inputs.SetShoulderListener(MenuInput.Side.Right, _rightPanel.onClick.Invoke, _rightPanel.Select);
        if (_leftPanel != null) inputs.SetShoulderListener(MenuInput.Side.Left, _leftPanel.onClick.Invoke, _leftPanel.Select);
    }

}
