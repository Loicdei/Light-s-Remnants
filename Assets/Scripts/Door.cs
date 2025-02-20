using UnityEngine;
using UnityEngine.SceneManagement;

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

        // Vérifie si la porte a déjà été déverrouillée
        if (PlayerPrefs.GetInt("DoorUnlocked_" + scene, 0) == 1)
        {
            isDoorUnlocked = true;
        }
        else
        {
            string lastLevel = PlayerPrefs.GetString("LastLevel", "");
            if (lastLevel == scene)
            {
                isDoorUnlocked = true;
                PlayerPrefs.SetInt("DoorUnlocked_" + scene, 1);
                PlayerPrefs.Save();
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

        // Sauvegarde du niveau actuel avant de changer de scène
        PlayerPrefs.SetString("LastLevel", scene);
        PlayerPrefs.Save();

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
