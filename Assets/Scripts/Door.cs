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

    private bool playerInRange = false;
    public bool isDoorUnlocked = false;
    private SpriteRenderer spriteRenderer;

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
                SceneManager.LoadScene(scene.name);
            }
            else
            {
            // Animation Shake door + sound effect
            }
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
