using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer closeLock;
    [SerializeField] private SceneAsset targetScene;
    public static DoorController instance;  // Singleton pour un accès global
    private Beacon[] beacons;  // Tableau contenant toutes les balises dans la scène
    private bool playerInRange = false;
    private bool isDoorUnlocked = false;
    private Animator doorAnimator; // Référence à l'Animator de la porte

    void Awake()
    {
        // Crée l'instance pour pouvoir l'appeler
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
    }

    private void Update()
    {
        if (isDoorUnlocked)
        {
            doorAnimator.SetBool("isOpen", playerInRange);

            // Si le joueur est dans la zone et appuie sur une touche, ouvre la porte
            if (playerInRange && Input.GetAxis("Vertical") > 0)
            {
                SceneManager.LoadScene(targetScene.name);
                // TODO : Unlock next level
                // TODO : transition
            }
        }
    }

    void Start()
    {
        beacons = FindObjectsOfType<Beacon>();  // Trouver toutes les balises dans la scène
    }

    // Fonction pour vérifier si toutes les balises sont allumées
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
        // Tous les beacon ont été check, unlock la porte
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
}
