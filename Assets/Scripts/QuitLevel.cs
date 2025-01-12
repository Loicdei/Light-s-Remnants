using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitLevel : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private bool playerInRange;
    // Update is called once per frame
    void Start()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Assuming you have a way to check if the player is in range
            if (playerInRange)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MenuJouable");
            }
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
}
