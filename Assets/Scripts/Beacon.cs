using UnityEngine;

public class Beacon : MonoBehaviour
{
    [SerializeField] Sprite beaconOff;
    [SerializeField] Sprite beaconOn;
    [SerializeField] UnityEngine.Rendering.Universal.Light2D beaconLight;

    private bool playerInRange = false;
    private bool isLit = false;
    private SpriteRenderer spriteRenderer;
    public float lightIntensityTarget = 1.5f;
    public float lightRadiusTarget = 5f;
    private GrabController grabController;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = beaconOff;
        beaconLight.intensity = 0f;
        beaconLight.pointLightOuterRadius = 0f;
        grabController = GameObject.FindGameObjectWithTag("Player").GetComponent<GrabController>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && grabController.isHoldingLantern())
        {
            ToggleBeacon();
        }

        // Mise à jour progressive de l'intensité et du rayon si la balise est allumée
        if (isLit)
        {
            beaconLight.intensity = Mathf.Lerp(beaconLight.intensity, lightIntensityTarget, Time.deltaTime);
            beaconLight.pointLightOuterRadius = Mathf.Lerp(beaconLight.pointLightOuterRadius, lightRadiusTarget, Time.deltaTime);
        }
        else
        {
            // Réduction progressive de l'intensité et du rayon si la balise est éteinte
            beaconLight.intensity = Mathf.Lerp(beaconLight.intensity, 0f, Time.deltaTime);
            beaconLight.pointLightOuterRadius = Mathf.Lerp(beaconLight.pointLightOuterRadius, 0f, Time.deltaTime);
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

    private void ToggleBeacon()
    {
        if (isLit)
        {
            TurnOffBeacon();
        }
        else
        {
            TurnOnBeacon();
        }
        DoorController.instance.CheckAllBeaconsLit();
    }

    private void TurnOnBeacon()
    {
        isLit = true;
        spriteRenderer.sprite = beaconOn;
    }

    private void TurnOffBeacon()
    {
        isLit = false;
        spriteRenderer.sprite = beaconOff;
    }

    public bool IsLit()
    {
        return isLit;
    }
}
