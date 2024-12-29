using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    private Transform playerSpawn;
    private Animator fadeSystem;
    CameraFollow cameraFollow;

    Rigidbody2D playerRb;
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
    }
    void Start()
    {

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
        playerRb.velocity = new Vector2(0, 0);
        playerRb.simulated = false;
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(1f);
        transform.position = playerSpawn.position;
        cameraFollow.setSmoothTime(0f);
        yield return new WaitForSecondsRealtime(1f);
        cameraFollow.setSmoothTime(0.25f);
        playerRb.simulated = true;
    }
}
