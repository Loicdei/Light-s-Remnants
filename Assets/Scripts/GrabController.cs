using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GrabController : MonoBehaviour
{
    public Transform grabDetect; // Position de d�tection
    public Transform itemHolder; // Position de maintien
    public float rayDist;        // Distance du raycast pour d�tecter l'objet
    public float throwForce = 5f; // Force de lancer
    public float verticalThrowMultiplier = 2.0f;
    public float waitingTime = 0.5f;

    public float enlargedColliderSize = 1.5f;  // Taille du collider agrandie lorsqu'un objet est tenu
    private Vector2 originalColliderSize;

    private GameObject heldItem = null; // R�f�rence � l'objet tenu
    public bool isHolding = false;     // Indique si on tient un objet
    private PolygonCollider2D playerCollider;  // R�f�rence au collider du joueur
    private bool canGrab = true;

    public Camera mainCamera;           // R�f�rence � la cam�ra principale
    public Light2D lanternLight;        // Lumi�re de la lanterne (via Unity 2D Renderer)
    public float focusZoom;        // Zoom en mode focus (r�duit pour un vrai d�zoom)
    public float normalZoom;      // Zoom normal
    public float zoomSpeed = 0f;
    private bool isFocusMode = false;   // Indique si le mode focus est activ�

    private bool isHeldItemLantern = false;
    private bool isPaused;
    private Light2D lumiereLanterne;
    private float intensiteBase;
    public float intensiteFocusMode = 10;

    private LayerMask groundLayer;
    private Vector2 forwardDirection;
    private Vector2 backwardDirection;

    void Start()
    {
        playerCollider = GetComponent<PolygonCollider2D>(); // R�cup�rer le collider du joueur
        originalColliderSize = playerCollider.bounds.size;
        normalZoom = mainCamera.orthographicSize;
        groundLayer = LayerMask.GetMask("Ground");
        lumiereLanterne = GameObject.FindGameObjectWithTag("LanternLight").GetComponent<Light2D>();
        intensiteBase = lumiereLanterne.pointLightOuterRadius;
    }


    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (transform.localScale.x < 0) // Si le personnage regarde à gauche
        {
            forwardDirection = Vector2.left;
            backwardDirection = Vector2.right;
        }
        else if (transform.localScale.x > 0) // Si le personnage regarde à droite
        {
            forwardDirection = Vector2.right;
            backwardDirection = Vector2.left;
        }
        if (isPaused)
        {
            return;
        }


        // Si un objet est tenu
        if (isHolding)
        {
            if (heldItem != null)
            {

                // V�rifier si l'objet tenu est une lanterne
                if (heldItem.CompareTag("Lanterne"))
                {
                    // Activer le mode focus avec la touche R
                    if (Input.GetButtonDown("Focus"))
                    {
                        ToggleFocusMode(true);
                    }

                    // D�sactiver automatiquement le mode focus si le joueur bouge
                    if (isFocusMode && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
                    {
                        ToggleFocusMode(false);
                    }
                }
            }
            heldItem.transform.position = itemHolder.position;
        }


        // Saisir ou lancer l'objet avec la touche E ou la mannette X ou Y
        if (Input.GetButtonDown("Grab"))
        {
            GrabAction();
        }

    }

    public void GrabAction() //appelable par le bouton mobile
    {
        if (!canGrab) return; // Sortir si le joueur ne peut pas saisir
        // D�tection des objets attrapables autour du joueur
        Collider2D[] hits = Physics2D.OverlapCircleAll(grabDetect.position, rayDist);

        foreach (var hit in hits)
        {
            // V�rifier si un objet est soit un "Item" soit une "Lanterne"
            if (hit.CompareTag("Item") || hit.CompareTag("Lanterne"))
            {

                if (!isHolding)
                {
                    GrabItem(hit.gameObject); // Attraper l'objet
                }
                else if (heldItem != null)
                {
                    if (!IsThrowBlocked())
                    {
                        ThrowItem();
                    }
                }

            }
        }
    }

    public void SetPauseState(bool paused)
    {
        isPaused = paused;
    }

    void ToggleFocusMode(bool enable)
    {
        if (enable)
        {
            StartCoroutine(SmoothLightRadius(intensiteFocusMode + 3, 0.5f));
            StartCoroutine(SmoothZoom(normalZoom + 3)); // D�marrer le zoom fluide vers le focusZoom
            isFocusMode = true;
        }
        else
        {
            StartCoroutine(SmoothLightRadius(intensiteBase, 0.5f));
            StartCoroutine(SmoothZoom(normalZoom)); // D�marrer le zoom fluide vers le normalZoom
            isFocusMode = false;
        }
    }

    // Coroutine pour effectuer un zoom fluide
    IEnumerator SmoothZoom(float targetZoom)
    {
        float startZoom = mainCamera.orthographicSize; // Zoom de d�part
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

    void GrabItem(GameObject item)
    {
        if (item.CompareTag("Lanterne"))
        {
            isHeldItemLantern = true;
        }
        heldItem = item;
        heldItem.transform.parent = itemHolder;
        heldItem.transform.position = itemHolder.position;
        heldItem.GetComponent<Rigidbody2D>().isKinematic = true; // D�sactiver la physique pendant qu'il est tenu

        // D�sactiver la collision entre le joueur et l'objet tenu
        Physics2D.IgnoreCollision(heldItem.GetComponent<Collider2D>(), playerCollider, true);

        isHolding = true;
    }

    void ThrowItem()
    {

        ToggleFocusMode(false);
        // R�active la physique de l'objet
        Rigidbody2D itemRb = heldItem.GetComponent<Rigidbody2D>();
        itemRb.isKinematic = false;

        // D�finis l'objet comme non-parent� au joueur
        heldItem.transform.parent = null;

        // D�termine la direction de lancement
        Vector2 throwDirection = new Vector2(transform.localScale.x, verticalThrowMultiplier).normalized;

        // Applique une v�locit� fixe pour garantir une trajectoire constante
        itemRb.velocity = throwDirection * throwForce;

        // R�initialise les r�f�rences
        heldItem = null;
        isHolding = false;
        isHeldItemLantern = false;

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
        Gizmos.DrawWireSphere(grabDetect.position, rayDist); // hitbox grab

        //  hitbox des detecteurs de murs
        float raycastDistanceLeft = 0.1f;
        float raycastDistanceRight = 0.3f;

        Vector2 raycastStart = itemHolder.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(raycastStart, raycastStart + forwardDirection * raycastDistanceLeft);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(raycastStart, raycastStart + backwardDirection * raycastDistanceRight);
    }

    public bool isHoldingLantern()
    {
        return isHeldItemLantern;
    }

    bool IsThrowBlocked()
    {

        RaycastHit2D hitLeft = Physics2D.Raycast(itemHolder.position, forwardDirection, 0.1f, groundLayer);
        if (hitLeft.collider != null)
        {
            return true;
        }

        RaycastHit2D hitRight = Physics2D.Raycast(itemHolder.position, backwardDirection, 0.3f, groundLayer);
        if (hitRight.collider != null)
        {
            return true;
        }

        return false;
    }

    IEnumerator SmoothLightRadius(float targetRadius, float duration)
    {
        float startRadius = lumiereLanterne.pointLightOuterRadius;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            lumiereLanterne.pointLightOuterRadius = Mathf.Lerp(startRadius, targetRadius, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Attendre le prochain frame
        }

        // Assurez-vous que la portée finale est exactement la valeur cible
        lumiereLanterne.pointLightOuterRadius = targetRadius;
    }

}