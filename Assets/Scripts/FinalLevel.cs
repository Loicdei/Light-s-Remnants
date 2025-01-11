using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalLevel : MonoBehaviour
{
    [SerializeField] private SceneAsset targetScene; // Scène cible pour la transition
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D transitionLight; // Lumière pour l'effet
    [SerializeField] private float lightIntensityTarget = 1.5f; // Intensité cible de la lumière
    [SerializeField] private float lightRadiusTarget = 5f; // Rayon cible de la lumière
    [SerializeField] private float lightTransitionSpeed = 1f; // Vitesse de transition de la lumière

    private bool playerInRange = false;
    private bool isTransitioning = false;

    void Start()
    {
        // Initialisation de la lumière à faible intensité
        transitionLight.intensity = 0f;
        transitionLight.pointLightOuterRadius = 0f;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            StartCoroutine(TransitionLevel());
        }

        // Transition progressive de la lumière si elle est en cours de transition
        if (isTransitioning)
        {
            transitionLight.intensity = Mathf.Lerp(transitionLight.intensity, lightIntensityTarget, Time.deltaTime * lightTransitionSpeed);
            transitionLight.pointLightOuterRadius = Mathf.Lerp(transitionLight.pointLightOuterRadius, lightRadiusTarget, Time.deltaTime * lightTransitionSpeed);
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

    private IEnumerator TransitionLevel()
    {
        isTransitioning = true;

        // Début de la séquence de fade
        yield return new WaitForSecondsRealtime(1f); // Attend la fin de l'animation de fade
        SceneManager.LoadSceneAsync(targetScene.name); // Charge la nouvelle scène
    }
}
