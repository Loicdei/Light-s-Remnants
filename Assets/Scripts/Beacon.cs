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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = beaconOff;  
        beaconLight.intensity = 0f; 
        beaconLight.pointLightOuterRadius = 0f;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isLit)
        {
            LightBeacon();
        }

        if (isLit)
        {
            beaconLight.intensity = Mathf.Lerp(beaconLight.intensity, lightIntensityTarget, Time.deltaTime);
            beaconLight.pointLightOuterRadius = Mathf.Lerp(beaconLight.pointLightOuterRadius, lightRadiusTarget, Time.deltaTime);
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
        BeaconManager.instance.CheckAllBeaconsLit();  // V�rifier si toutes les balises sont allum�es
    }

    public bool IsLit()
    {
        return isLit;
    }
}
