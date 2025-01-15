using System.Collections;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    private Transform playerSpawn;
    private Animator fadeSystem;
    private CameraFollow cameraFollow;

    Rigidbody2D playerRb;
    private PlayerController playerController;
    private GrabController grabController;

    private void Awake()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        if (cameras.Length > 0)
        {
            cameraFollow = cameras[0].GetComponent<CameraFollow>(); // on espere que la premiere est celle que l'on souhaite
        }
        else
        {
            Debug.LogError("No GameObjects found with the tag 'MainCamera'");
        }
        playerRb = GetComponent<Rigidbody2D>();
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        grabController = GetComponent<GrabController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            StartCoroutine(Respawn(collision));
        }
    }
    private IEnumerator Respawn(Collider2D collision)
    {
        if (playerController != null)
        {
            playerController.SetPauseState(true);
            grabController.SetPauseState(true);
        }
        GameObject lanterne = GameObject.FindGameObjectWithTag("Lanterne");
        playerRb.simulated = false;
        playerRb.velocity = new Vector2(0,0);
        Time.timeScale = 0;
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        Vector3 newPlayerSpawn = playerSpawn.position;
        newPlayerSpawn.y += 1f;
        transform.position = newPlayerSpawn;
        if (!grabController.isHolding)
        {
            lanterne.transform.position = newPlayerSpawn;
        }
        cameraFollow.setSmoothTime(0f);
        yield return new WaitForSecondsRealtime(1f);
        cameraFollow.setSmoothTime(0.25f);
        playerRb.simulated = true;
        if (playerController != null)
        {
            playerController.SetPauseState(false);
            grabController.SetPauseState(false);
        }
    }
}