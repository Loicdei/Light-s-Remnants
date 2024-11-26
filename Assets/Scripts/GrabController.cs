using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    public Transform grabDetect; // Position de détection
    public Transform itemHolder; // Position de maintien
    public float rayDist;        // Distance du raycast pour détecter l'objet
    public float throwForce = 5f; // Force de lancer
    public float verticalThrowMultiplier = 2.0f;
    public float waitingTime = 0.5f;

    private GameObject heldItem = null; // Référence à l'objet tenu
    private bool isHolding = false;     // Indique si on tient un objet
    private Collider2D playerCollider;  // Référence au collider du joueur
    private bool canGrab = true;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>(); // Récupérer le collider du joueur
    }

    void Update()
    {
        if (!canGrab) return;
        // Détecter tous les colliders dans un cercle autour de grabDetect
        Collider2D[] hits = Physics2D.OverlapCircleAll(grabDetect.position, rayDist);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Item")) // Vérifie si l'objet a le tag "Item"
            {
                if (Input.GetKeyDown(KeyCode.F)) // Utiliser E pour saisir
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

        // Si l'objet est tenu, le faire suivre la position de l'itemHolder
        if (isHolding && heldItem != null)
        {
            heldItem.transform.position = itemHolder.position; // Suivre la position du joueur
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

        // Applique une force pour lancer l'objet
        Vector2 throwDirection = new Vector2(transform.localScale.x, verticalThrowMultiplier);
        itemRb.AddForce(throwDirection.normalized * throwForce, ForceMode2D.Impulse);

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