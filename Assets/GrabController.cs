using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GrabController : MonoBehaviour
{
    public Transform grabDetect; // Position de détection
    public Transform itemHolder; // Position de maintien
    public float rayDist;        // Distance du raycast pour détecter l'objet
    public float throwForce = 5f; // Force de lancer
    public float verticalThrowMultiplier = 2.0f;
    public float waitingTime = 0.5f;

    public float enlargedColliderSize = 1.5f;  // Taille du collider agrandie lorsqu'un objet est tenu
    private Vector2 originalColliderSize;

    private GameObject heldItem = null; // Référence à l'objet tenu
    private bool isHolding = false;     // Indique si on tient un objet
    private BoxCollider2D playerCollider;  // Référence au collider du joueur
    private bool canGrab = true;

    public Camera mainCamera;           // Référence à la caméra principale
    public Light2D lanternLight;        // Lumière de la lanterne (via Unity 2D Renderer)
    public float focusZoom;        // Zoom en mode focus (réduit pour un vrai dézoom)
    public float normalZoom;      // Zoom normal
    public float zoomSpeed = 0f;
    private bool isFocusMode = false;   // Indique si le mode focus est activé


    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>(); // Récupérer le collider du joueur
        originalColliderSize = playerCollider.size;
        normalZoom = mainCamera.orthographicSize;
    }

    void Update()
    {
        if (!canGrab) return; // Sortir si le joueur ne peut pas saisir

        // Si un objet est tenu
        if (isHolding && heldItem != null)
        {

            // Vérifier si l'objet tenu est une lanterne
            if (heldItem.CompareTag("Lanterne"))
            {

                // Activer le mode focus avec la touche R
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ToggleFocusMode(true);
                }

                // Désactiver automatiquement le mode focus si le joueur bouge
                if (isFocusMode && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
                {
                    ToggleFocusMode(false);
                }
            }

            // Maintenir l'objet à la position de l'itemHolder
            heldItem.transform.position = itemHolder.position;
            EnlargeCollider();
        }
        else
        {
            RestoreColliderSize(); // Restaurer la taille du collider si aucun objet n'est tenu
        }

        // Détection des objets attrapables autour du joueur
        Collider2D[] hits = Physics2D.OverlapCircleAll(grabDetect.position, rayDist);

        foreach (var hit in hits)
        {
            // Vérifier si un objet est soit un "Item" soit une "Lanterne"
            if (hit.CompareTag("Item") || hit.CompareTag("Lanterne"))
            {
                // Saisir ou lancer l'objet avec la touche E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!isHolding)
                    {
                        GrabItem(hit.gameObject); // Attraper l'objet
                    }
                    else if (heldItem != null)
                    {
                        ThrowItem(); // Lancer l'objet
                    }
                }
            }
        }
    }



    void ToggleFocusMode(bool enable)
    {
        if (enable)
        {
            StartCoroutine(SmoothZoom(normalZoom+3)); // Démarrer le zoom fluide vers le focusZoom
            isFocusMode = true;
        }
        else
        {
            StartCoroutine(SmoothZoom(normalZoom)); // Démarrer le zoom fluide vers le normalZoom
            isFocusMode = false;
        }
    }

    // Coroutine pour effectuer un zoom fluide
    IEnumerator SmoothZoom(float targetZoom)
    {
        float startZoom = mainCamera.orthographicSize; // Zoom de départ
        float elapsedTime = 0f;

        while (elapsedTime < 0.5f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null; // Attendre le prochain frame
        }

        // Assurez-vous que le zoom final est exactement la valeur cible
        mainCamera.orthographicSize = targetZoom;
    }

    void EnlargeCollider()
    {
        if (playerCollider != null)
        {
            playerCollider.size = new Vector2(originalColliderSize.x * enlargedColliderSize, originalColliderSize.y);
        }
    }
    void RestoreColliderSize()
    {
        if (playerCollider != null)
        {
            playerCollider.size = originalColliderSize;
        }
    }

    void GrabItem(GameObject item)
    {
        heldItem = item;
        heldItem.transform.parent = itemHolder;
        heldItem.transform.position = itemHolder.position;
        heldItem.GetComponent<Rigidbody2D>().isKinematic = true; // Désactiver la physique pendant qu'il est tenu

        // Désactiver la collision entre le joueur et l'objet tenu
        Physics2D.IgnoreCollision(heldItem.GetComponent<Collider2D>(), playerCollider, true);

        isHolding = true;
    }

    void ThrowItem()
    {
        // Réactive la physique de l'objet
        Rigidbody2D itemRb = heldItem.GetComponent<Rigidbody2D>();
        itemRb.isKinematic = false;

        // Définis l'objet comme non-parenté au joueur
        heldItem.transform.parent = null;

        // Détermine la direction de lancement
        Vector2 throwDirection = new Vector2(transform.localScale.x, verticalThrowMultiplier).normalized;

        // Applique une vélocité fixe pour garantir une trajectoire constante
        itemRb.velocity = throwDirection * throwForce;

        // Réinitialise les références
        heldItem = null;
        isHolding = false;

        StartCoroutine(WaitBeforeGrab());
    }
    IEnumerator WaitBeforeGrab()
    {
        canGrab = false; 
        yield return new WaitForSeconds(waitingTime); // Attendre
        canGrab = true; 
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(grabDetect.position, rayDist); // hitbox 
    }
    
}