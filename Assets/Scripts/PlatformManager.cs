using System.Collections;
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

    private Coroutine fadeCoroutine;


    private void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();

        ResizePlatform();
    }

    private void ResizePlatform()
    {
        if (spriteRenderer == null || boxCollider == null) return;

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
        boxCollider.offset = Vector2.zero; // Le BoxCollider est centré sur le transform
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

        // Si la distance est inférieure au rayon, il y a chevauchement
        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
        return distanceSquared < (circleRadius * circleRadius);
    }



    private bool IsUnderLight()
    {
        GameObject[] beacons = GameObject.FindGameObjectsWithTag("Beacon");
        foreach (GameObject beacon in beacons)
        {
            Light2D light2D = beacon.GetComponent<Light2D>();
            if (light2D != null && light2D.enabled)
            {
                Vector2 lightPosition = light2D.transform.position;
                float lightRadius = light2D.pointLightOuterRadius; // Rayon externe de la lumière

                // Vérifier la distance entre la lumière et la plateforme
                Vector2 platformPosition = transform.position;

                Vector2 platformSize = boxCollider.size;
                Vector2 platformMin = (Vector2)transform.position - platformSize / 2;
                Vector2 platformMax = (Vector2)transform.position + platformSize / 2;

                // Si la lumière est de type ponctuel
                if (light2D.lightType == Light2D.LightType.Point)
                {
                    // Vérifier si la lumière affecte la plateforme
                    if (IsCircleOverlappingRectangle(lightPosition, lightRadius, platformMin, platformMax))
                    {
                        return true;
                    }
                }
                else if (light2D.lightType == Light2D.LightType.Global)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void SetPlatformActive()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSpriteAlpha(1f));
        boxCollider.enabled = true;
    }

    private void SetPlatformInactive()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSpriteAlpha(0.1f));
        boxCollider.enabled = false;
    }

    private IEnumerator FadeSpriteAlpha(float targetAlpha)
    {
        float currentAlpha = spriteRenderer.color.a;
        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsed / duration); // Interpolation linéaire
            spriteRenderer.color = new Color(1, 1, 1, newAlpha); // Appliquer le nouvel alpha
            yield return null; // Attendre la frame suivante
        }

        // Assurer que l'alpha final est bien celui attendu
        spriteRenderer.color = new Color(1, 1, 1, targetAlpha);
        fadeCoroutine = null; // Libérer la référence
    }

}
