using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer closeLock;
    [SerializeField] private string targetScene;
    [SerializeField] private Joystick joystick;
    public static DoorController instance;  // Singleton pour un acc�s global
    private Beacon[] beacons;  // Tableau contenant toutes les balises dans la sc�ne
    private bool playerInRange = false;
    private bool isDoorUnlocked = false;
    private Animator doorAnimator; // R�f�rence � l'Animator de la porte
    private Animator fadeSystem;
    private CameraFollow cameraFollow;

    Rigidbody2D playerRb;
    private PlayerController playerController;
    private GrabController grabController;

    void Awake()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        // Cr�e l'instance pour pouvoir l'appeler
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Assigne l'Animator
        doorAnimator = GetComponent<Animator>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        grabController = GameObject.FindGameObjectWithTag("Player").GetComponent<GrabController>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        if (isDoorUnlocked)
        {
            doorAnimator.SetBool("isOpen", playerInRange);

            // Si le joueur est dans la zone et appuie sur une touche, ouvre la porte
            if (playerInRange && (Input.GetButtonDown("EnterDoor") || Input.GetAxis("Vertical") > .5f || joystick.Vertical() > .5f))
            {
                StartCoroutine(TransitionLevel());
                // TODO : Unlock next level
            }
        }
    }

    void Start()
    {
        beacons = FindObjectsOfType<Beacon>();  // Trouver toutes les balises dans la sc�ne
    }

    // Fonction pour v�rifier si toutes les balises sont allum�es
    public void CheckAllBeaconsLit()
    {
        foreach (Beacon beacon in beacons)
        {
            if (!beacon.IsLit())
            {
                isDoorUnlocked = false;
                closeLock.enabled = true;
                return;
            }
        }
        // Tous les beacon ont �t� check, unlock la porte
        isDoorUnlocked = true;
        closeLock.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private bool isTransitioning = false;

    private IEnumerator TransitionLevel()
    {
        if (isTransitioning) yield break; // Ne rien faire si une transition est en cours
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
        SceneManager.LoadSceneAsync(targetScene);
        yield return new WaitForSecondsRealtime(1f);
        playerRb.simulated = true;
        if (playerController != null)
        {
            playerController.SetPauseState(false);
            grabController.SetPauseState(false);
        }
    }
}
