using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class PlatformManager : MonoBehaviour
{
    public enum PlatformSize { Small = 16, Medium = 32, Large = 48 }
    public PlatformSize size = PlatformSize.Small;

    public enum PlatformActivation { WithLight, WithoutLight, Always }
    public PlatformActivation platformActivation = PlatformActivation.Always;

    public Sprite smallSprite;
    public Sprite mediumSprite;
    public Sprite largeSprite;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();

        ResizePlatform();
    }

    private void ResizePlatform()
    {
        if (spriteRenderer == null || boxCollider == null) return;

        // D�finir la texture en fonction de la taille
        switch (size)
        {
            case PlatformSize.Small:
                spriteRenderer.sprite = smallSprite;
                break;
            case PlatformSize.Medium:
                spriteRenderer.sprite = mediumSprite;
                break;
            case PlatformSize.Large:
                spriteRenderer.sprite = largeSprite;
                break;
        }

        // Ajuster la largeur en fonction de la taille
        float width = (float)size / 16;

        // Ajuster le BoxCollider
        boxCollider.size = new Vector2(width, 0.5f); // 0.5f pour 8px de hauteur
        boxCollider.offset = Vector2.zero; // Le BoxCollider est centr� sur le transform
    }

    private void Update()
    {
        HandlePlatformActivation();
    }

    private void HandlePlatformActivation()
    {
        switch (platformActivation)
        {
            case PlatformActivation.WithLight:
                // La plateforme est fonctionnelle uniquement en pr�sence de lumi�re des balises
                if (IsUnderLight())
                {
                    SetPlatformActive();
                }
                else
                {
                    SetPlatformInactive();
                }
                break;

            case PlatformActivation.WithoutLight:
                
                // La plateforme est fonctionnelle si il n'y a pas de lumi�re des balises
                if (IsUnderLight())
                {
                    SetPlatformInactive();
                }
                else
                {
                    SetPlatformActive();
                }
                break;

            case PlatformActivation.Always:
                // La plateforme est toujours fonctionnelle
                SetPlatformActive();
                break;
        }
    }

    private bool IsUnderLight()
    {
        // Chercher toutes les balises avec le tag "Beacon"
        GameObject[] beaconObjects = GameObject.FindGameObjectsWithTag("Beacon");

        foreach (GameObject beacon in beaconObjects)
        {
            CircleCollider2D beaconCollider = beacon.GetComponentInChildren<CircleCollider2D>();  // R�cup�rer le collider de la balise
            if (beaconCollider != null)
            {
                // R�cup�rer la position du centre du BoxCollider2D de la plateforme
                Vector2 platformCenter = boxCollider.bounds.center;
                float platformWidth = boxCollider.bounds.size.x;
                float platformHeight = boxCollider.bounds.size.y;

                Vector2 beaconPosition = beaconCollider.transform.position;
                float radius = beaconCollider.radius;

                // V�rifier si la plateforme (le BoxCollider2D) se trouve dans le rayon de la balise
                if (Vector2.Distance(platformCenter, beaconPosition) < (radius + platformWidth / 2))
                {
                    Debug.Log("under light");
                    return true;  // La plateforme est sous la lumi�re
                }
            }
        }

        return false;  // Aucune balise ne touche la plateforme
    }



    private void SetPlatformActive()
    {
        // Rendre la plateforme fonctionnelle, visible et avec des collisions
        boxCollider.enabled = true;
        spriteRenderer.color = Color.white; // R�tablir la couleur originale
    }

    private void SetPlatformInactive()
    {
        // D�sactiver la plateforme, la rendre encore plus transparente et sans collisions
        boxCollider.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 0.1f); // Rendre la plateforme quasi invisible
    }
}
