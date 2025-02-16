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
    [Header("Panels")]
    [SerializeField] private List<MenuPanel> panelsList = new List<MenuPanel>();
    private Dictionary<PanelType, MenuPanel> panelsDict = new Dictionary<PanelType, MenuPanel>();

    [SerializeField] private EventSystem eventController;

    private GameManager manager;
    private Animator fadeSystem;

    private Button[] buttons;
    private Button BtnContinue;

    private bool shouldContinueBeActive = false; // Permet de contrôler l'état réel du bouton Continue

    private void Awake()
    {
        buttons = FindObjectsOfType<Button>();

        GameObject btnObject = GameObject.FindGameObjectWithTag("BtnContinue");
        if (btnObject != null)
        {
            BtnContinue = btnObject.GetComponent<Button>();
            Debug.Log("Bouton Continue trouvé !");
        }
        else
        {
            Debug.LogError("Le bouton Continue n'a pas été trouvé ! Vérifiez le tag.");
        }
    }

    private void Start()
    {
        manager = GameManager.instance;
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();

        string savedScene = PlayerPrefs.GetString("LastLevel", "MenuJouable");
        Debug.Log("Valeur LastLevel dans PlayerPrefs au démarrage : " + savedScene);

        // Détermine si le bouton doit être actif
        shouldContinueBeActive = (savedScene != "MenuJouable");

        // Initialisation des panels
        foreach (var _panel in panelsList)
        {
            if (_panel) panelsDict.Add(_panel.GetPanelType(), _panel);
            _panel.Init(this);
        }

        OpenOnePanel(PanelType.Main);

        // Applique l'état du bouton Continue après toutes les modifications
        SetContinueButtonInteractable(shouldContinueBeActive);
    }

    private void OpenOnePanel(PanelType _type)
    {
        foreach (var _panel in panelsList)
            _panel.ChangeState(false);

        if (_type != PanelType.None)
        {
            panelsDict[_type].ChangeState(true);

            if (panelsDict[_type].firstButton != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(panelsDict[_type].firstButton);
            }
        }
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
            // Vérifie que ce n'est pas BtnContinue pour éviter les conflits
            if (button != BtnContinue)
            {
                button.interactable = interactable;
            }
        }
    }

    private void SetContinueButtonInteractable(bool interactable)
    {
        if (BtnContinue != null)
        {
            BtnContinue.interactable = interactable;
            Debug.Log("Bouton Continue " + (interactable ? "activé !" : "désactivé !"));
        }
        else
        {
            Debug.LogError("Impossible de modifier l'interactivité du bouton Continue car il est null !");
        }
    }

    public IEnumerator SceneChangeCoroutine()
    {
        PlayerPrefs.DeleteKey("LastLevel");

        SetButtonsInteractable(false);
        SetContinueButtonInteractable(false);

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "MenuStaticNew")
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
        StartCoroutine(SceneChangeCoroutineContinue());
    }

    public IEnumerator SceneChangeCoroutineContinue()
    {
        string savedScene = PlayerPrefs.GetString("LastLevel", "MenuJouable");
        Debug.Log("Valeur LastLevel avant chargement : " + savedScene);

        SetButtonsInteractable(false);
        SetContinueButtonInteractable(false);

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "MenuStaticNew")
        {
            Time.timeScale = 0;
            fadeSystem.SetTrigger("FadeIn");
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1;
        }
        SceneManager.LoadSceneAsync(savedScene);
        SetButtonsInteractable(true);
    }

    public void Quit()
    {
        manager.Quit();
    }
}
