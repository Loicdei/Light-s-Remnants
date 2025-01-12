using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Door : MonoBehaviour
{
    [SerializeField] Sprite SpriteDoorOpen;
    [SerializeField] Sprite SpriteDoorLocked;
    [SerializeField] Sprite SpriteDoorClosed;
    [SerializeField] SceneAsset scene;
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
        spriteRenderer.sprite = SpriteDoorOpen;
        spriteRenderer.sprite = SpriteDoorLocked;
        spriteRenderer.sprite = SpriteDoorClosed;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || playerInRange && Input.GetAxis("Vertical") > 0)
        {
            if (playerInRange)
            {
                StartCoroutine(OpenDoor());
            }
            else
            {
            // Animation Shake door + sound effect
            }
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
        SceneManager.LoadScene(scene.name);
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
        if (other.CompareTag("Player") && isDoorUnlocked)
        {
            playerInRange = true;
            spriteRenderer.sprite = SpriteDoorOpen;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDoorUnlocked)
        {
            playerInRange = false;
            spriteRenderer.sprite = SpriteDoorClosed;
        }
    }
}
