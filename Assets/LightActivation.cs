using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class LightActivation : MonoBehaviour
{
    public enum AcivationType { WithLight, WithoutLight, Always }
    public AcivationType acivationType = AcivationType.Always;

    public SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private Coroutine fadeCoroutine;

    private void Update()
    {
        HandleAcivationType();
    }

    private void HandleAcivationType()
    {
        switch (acivationType)
        {
            case AcivationType.WithLight:
                if (IsUnderLight())
                {
                    SetObjectActive();
                }
                else
                {
                    SetObjectInactive();
                }
                break;

            case AcivationType.WithoutLight:

                if (IsUnderLight())
                {
                    SetObjectInactive();
                }
                else
                {
                    SetObjectActive();
                }
                break;

            case AcivationType.Always:
                SetObjectActive();
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
        GameObject[] beacons = GameObject.FindGameObjectsWithTag("Beacon");
        foreach (GameObject beacon in beacons)
        {
            Light2D light2D = beacon.GetComponent<Light2D>();
            if (light2D != null && light2D.enabled)
            {
                Vector2 lightPosition = light2D.transform.position;
                float lightRadius = light2D.pointLightOuterRadius; // Rayon externe de la lumi�re

                Vector2 objectSize = boxCollider.size;
                Vector2 objectMin = (Vector2)transform.position - objectSize / 2;
                Vector2 objectMax = (Vector2)transform.position + objectSize / 2;

                // Si la lumi�re est de type ponctuel
                if (light2D.lightType == Light2D.LightType.Point)
                {
                    // V�rifier si la lumi�re affecte la plateforme
                    if (IsCircleOverlappingRectangle(lightPosition, lightRadius, objectMin, objectMax))
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

    private void SetObjectActive()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSpriteAlpha(1f));
        boxCollider.enabled = true;
    }

    private void SetObjectInactive()
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
            float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsed / duration); // Interpolation lin�aire
            spriteRenderer.color = new Color(1, 1, 1, newAlpha); // Appliquer le nouvel alpha
            yield return null; // Attendre la frame suivante
        }

        // Assurer que l'alpha final est bien celui attendu
        spriteRenderer.color = new Color(1, 1, 1, targetAlpha);
        fadeCoroutine = null; // Lib�rer la r�f�rence
    }

}
