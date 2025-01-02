using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class LightActivation : MonoBehaviour
{
    public enum ActivationType { WithLight, WithoutLight }
    public ActivationType activationType = ActivationType.WithLight;

    private SpriteRenderer spriteRenderer;
    private new Collider2D collider2D; // Support pour plusieurs types de colliders

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        if (collider2D == null)
        {
            Debug.LogError("No Collider2D found on the object!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No Sprite found on the object!");
        }
    }

    private void Update()
    {
        HandleActivationType();
    }

    private void HandleActivationType()
    {
        switch (activationType)
        {
            case ActivationType.WithLight:
                if (IsUnderLight())
                {
                    SetObjectActive();
                }
                else
                {
                    SetObjectInactive();
                }
                break;

            case ActivationType.WithoutLight:
                if (IsUnderLight())
                {
                    SetObjectInactive();
                }
                else
                {
                    SetObjectActive();
                }
                break;
        }
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

                if (light2D.lightType == Light2D.LightType.Point)
                {
                    // Vérifier si la lumière affecte la plateforme
                    if (IsColliderOverlappingLight(lightPosition, lightRadius))
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

    private bool IsColliderOverlappingLight(Vector2 lightPosition, float lightRadius)
    {
        if (collider2D is BoxCollider2D boxCollider)
        {
            Vector2 objectSize = boxCollider.size;
            Vector2 objectMin = (Vector2)transform.position - objectSize / 2;
            Vector2 objectMax = (Vector2)transform.position + objectSize / 2;

            return IsCircleOverlappingRectangle(lightPosition, lightRadius, objectMin, objectMax);
        }
        else if (collider2D is CircleCollider2D circleCollider)
        {
            Vector2 circlePosition = (Vector2)transform.position + circleCollider.offset;
            float circleRadius = circleCollider.radius;

            return Vector2.Distance(lightPosition, circlePosition) < (lightRadius + circleRadius);
        }
        else if (collider2D is EdgeCollider2D edgeCollider)
        {
            Vector2[] points = edgeCollider.points;
            Vector2 edgePosition = (Vector2)transform.position;

            foreach (var point in points)
            {
                Vector2 worldPoint = edgePosition + point;
                if (Vector2.Distance(lightPosition, worldPoint) < lightRadius)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsCircleOverlappingRectangle(Vector2 circleCenter, float circleRadius, Vector2 rectMin, Vector2 rectMax)
    {
        float closestX = Mathf.Clamp(circleCenter.x, rectMin.x, rectMax.x);
        float closestY = Mathf.Clamp(circleCenter.y, rectMin.y, rectMax.y);

        float distanceX = circleCenter.x - closestX;
        float distanceY = circleCenter.y - closestY;

        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
        return distanceSquared < (circleRadius * circleRadius);
    }

    private void SetObjectActive()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSpriteAlpha(1f));
        collider2D.enabled = true;
    }

    private void SetObjectInactive()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSpriteAlpha(0.1f));
        collider2D.enabled = false;
    }

    private IEnumerator FadeSpriteAlpha(float targetAlpha)
    {
        float currentAlpha = spriteRenderer.color.a;
        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsed / duration);
            spriteRenderer.color = new Color(1, 1, 1, newAlpha);
            yield return null;
        }

        spriteRenderer.color = new Color(1, 1, 1, targetAlpha);
        fadeCoroutine = null;
    }
}
