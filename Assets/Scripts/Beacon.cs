using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField] Sprite beaconOff;
    [SerializeField] Sprite beaconOn;
    [SerializeField] UnityEngine.Rendering.Universal.Light2D beaconLight;
    [SerializeField] CircleCollider2D lightCollider;  // Ajout du CircleCollider2D pour la zone d'effet de la lumière

    private bool playerInRange = false;
    private bool isLit = false;
    private SpriteRenderer spriteRenderer;
    public float lightIntensityTarget = 1.5f;
    public float lightRadiusTarget = 5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = beaconOff;
        beaconLight.intensity = 0f;
        beaconLight.pointLightOuterRadius = 0f;

        // S'assurer que le collider est bien initialisé
        if (lightCollider == null)
        {
            lightCollider = GetComponentInChildren<CircleCollider2D>();  // Récupère le CircleCollider2D s'il n'est pas affecté
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isLit)
        {
            LightBeacon();
        }

        if (isLit)
        {
            // Augmenter l'intensité de la lumière de manière progressive
            beaconLight.intensity = Mathf.Lerp(beaconLight.intensity, lightIntensityTarget, Time.deltaTime);
            beaconLight.pointLightOuterRadius = Mathf.Lerp(beaconLight.pointLightOuterRadius, lightRadiusTarget, Time.deltaTime);

            // Mettre à jour le rayon du collider en fonction du rayon de la lumière
            if (lightCollider != null)
            {
                lightCollider.radius = beaconLight.pointLightOuterRadius;  // Modifier le rayon du collider pour qu'il corresponde à celui de la lumière
            }
        }
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

    private void LightBeacon()
    {
        isLit = true;
        spriteRenderer.sprite = beaconOn;
        BeaconManager.instance.CheckAllBeaconsLit();  // Vérifier si toutes les balises sont allumées
    }

    public bool IsLit()
    {
        return isLit;
    }
}
