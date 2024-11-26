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
    private bool IsCircleOverlappingRectangle(Vector2 circleCenter, float circleRadius, Vector2 rectMin, Vector2 rectMax)
    {
        // Trouver le point du rectangle le plus proche du centre du cercle
        float closestX = Mathf.Clamp(circleCenter.x, rectMin.x, rectMax.x);
        float closestY = Mathf.Clamp(circleCenter.y, rectMin.y, rectMax.y);

        // Calculer la distance entre ce point et le centre du cercle
        float distanceX = circleCenter.x - closestX;
        float distanceY = circleCenter.y - closestY;

        // Si la distance est inf�rieure au rayon, il y a chevauchement
        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
        return distanceSquared < (circleRadius * circleRadius);
    }



    private bool IsUnderLight()
    {
        // R�cup�rer tous les objets avec le tag "Beacon"
        GameObject[] beacons = GameObject.FindGameObjectsWithTag("Beacon");
        foreach (GameObject beacon in beacons)
        {
            // V�rifier si le Beacon a une Light2D
            Light2D light2D = beacon.GetComponent<Light2D>();
            if (light2D != null && light2D.enabled)
            {
                // V�rifier la port�e de la lumi�re en fonction de son type
                Vector2 lightPosition = light2D.transform.position;
                float lightRadius = light2D.pointLightOuterRadius; // Rayon externe de la lumi�re

                // V�rifier la distance entre la lumi�re et la plateforme
                Vector2 platformPosition = transform.position;

                // Calculer les dimensions de la plateforme
                Vector2 platformSize = boxCollider.size;
                Vector2 platformMin = (Vector2)transform.position - platformSize / 2;
                Vector2 platformMax = (Vector2)transform.position + platformSize / 2;

                // Si la lumi�re est de type ponctuel
                if (light2D.lightType == Light2D.LightType.Point)
                {
                    // V�rifier si la lumi�re affecte la plateforme
                    if (IsCircleOverlappingRectangle(lightPosition, lightRadius, platformMin, platformMax))
                    {
                        return true;
                    }
                }
                else if (light2D.lightType == Light2D.LightType.Global)
                {
                    // Une lumi�re globale affecte toujours la plateforme
                    return true;
                }
                // Si la lumi�re est directionnelle ou autre, il faudra ajuster pour tenir compte de son angle et orientation
            }
        }
        return false; // Aucune lumi�re des balises ne chevauche la plateforme
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
