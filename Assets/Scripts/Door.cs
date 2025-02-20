using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    [SerializeField] Sprite SpriteDoorOpen;
    [SerializeField] Sprite SpriteDoorLocked;
    [SerializeField] Sprite SpriteDoorClosed;
    [SerializeField] private string scene;
    private Animator fadeSystem;

    private bool playerInRange = false;
    public bool isDoorUnlocked = false;
    private SpriteRenderer spriteRenderer;

    private bool isTransitioning = false;
    Rigidbody2D playerRb;
    private PlayerController playerController;
    private GrabController grabController;

    // Clé pour les PlayerPrefs (pour sauver les portes déverrouillées)
    private const string UnlockedDoorsKey = "UnlockedDoors";

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        grabController = GameObject.FindGameObjectWithTag("Player").GetComponent<GrabController>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Vérifie si LastLevel existe
        string lastLevel = PlayerPrefs.GetString("LastLevel", "");

        if (string.IsNullOrEmpty(lastLevel))
        {
            // Aucune progression sauvegardée : verrouille tout sauf "First"
            isDoorUnlocked = (scene == "First");
        }
        else
        {
            // Récupérer l'état des portes déverrouillées depuis PlayerPrefs
            string unlockedDoorsString = PlayerPrefs.GetString(UnlockedDoorsKey, "");
            List<string> unlockedDoors = new List<string>(unlockedDoorsString.Split(','));

            // Si la scène est dans la liste des portes déverrouillées
            if (unlockedDoors.Contains(scene) || lastLevel == scene)
            {
                isDoorUnlocked = true;
            }
        }

        UpdateDoorSprite();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (playerInRange && isDoorUnlocked && (Input.GetKeyDown(KeyCode.E) || Input.GetAxis("Vertical") > .5f))
        {
            StartCoroutine(OpenDoor());
        }
    }

    private System.Collections.IEnumerator OpenDoor()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        if (playerController != null)
        {
            playerController.SetPauseState(true);
            grabController.SetPauseState(true);
        }
        playerRb.simulated = false;
        Time.timeScale = 0;
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;

        // Met à jour le dernier niveau avant de charger la nouvelle scène
        PlayerPrefs.SetString("LastLevel", scene);

        // Sauvegarde les portes déverrouillées
        string unlockedDoorsString = PlayerPrefs.GetString(UnlockedDoorsKey, "");
        List<string> unlockedDoors = new List<string>(unlockedDoorsString.Split(','));

        // Ajoute la porte à la liste des portes déverrouillées si ce n'est pas déjà fait
        if (!unlockedDoors.Contains(scene))
        {
            unlockedDoors.Add(scene);
            PlayerPrefs.SetString(UnlockedDoorsKey, string.Join(",", unlockedDoors));
            PlayerPrefs.Save(); // Sauvegarde immédiatement
        }

        // Charge la scène suivante
        SceneManager.LoadSceneAsync(scene);
        yield return new WaitForSecondsRealtime(1f);

        playerRb.simulated = true;
        if (playerController != null)
        {
            playerController.SetPauseState(false);
            grabController.SetPauseState(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UpdateDoorSprite();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            UpdateDoorSprite();
        }
    }

    private void UpdateDoorSprite()
    {
        if (!isDoorUnlocked)
        {
            spriteRenderer.sprite = SpriteDoorLocked;
        }
        else if (playerInRange)
        {
            spriteRenderer.sprite = SpriteDoorOpen;
        }
        else
        {
            spriteRenderer.sprite = SpriteDoorClosed;
        }
    }
}
