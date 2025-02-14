using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDoor : MonoBehaviour
{
    private bool playerInRange;
    private Animator fadeSystem;
    Rigidbody2D playerRb;
    private PlayerController playerController;
    private GrabController grabController;
    private bool isTransitioning = false;
    // Récupérer et sauvegarder le nom du niveau actuel
    string currentScene = SceneManager.GetActiveScene().name;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        grabController = GameObject.FindGameObjectWithTag("Player").GetComponent<GrabController>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }
    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (playerInRange && (Input.GetKeyDown(KeyCode.E) || Input.GetAxis("Vertical") > .5f))
        {
            StartCoroutine(TransitionLevel());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private IEnumerator TransitionLevel()
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
        SceneManager.LoadSceneAsync("MenuJouable");
        yield return new WaitForSecondsRealtime(1f);
        playerRb.simulated = true;
        if (playerController != null)
        {
            playerController.SetPauseState(false);
            grabController.SetPauseState(false);
        }
    }
}
