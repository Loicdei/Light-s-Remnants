using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalLevel : MonoBehaviour
{
    [SerializeField] private SceneAsset targetScene; // Sc�ne cible pour la transition
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D transitionLight; // Lumi�re pour l'effet
    [SerializeField] private float lightIntensityTarget = 1.5f; // Intensit� cible de la lumi�re
    [SerializeField] private float lightRadiusTarget = 5f; // Rayon cible de la lumi�re
    [SerializeField] private float lightTransitionSpeed = 1f; // Vitesse de transition de la lumi�re

    private bool playerInRange = false;
    private bool isTransitioning = false;

    void Start()
    {
        // Initialisation de la lumi�re � faible intensit�
        transitionLight.intensity = 0f;
        transitionLight.pointLightOuterRadius = 0f;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            StartCoroutine(TransitionLevel());
        }

        // Transition progressive de la lumi�re si elle est en cours de transition
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

        // D�but de la s�quence de fade
        yield return new WaitForSecondsRealtime(1f); // Attend la fin de l'animation de fade
        SceneManager.LoadSceneAsync(targetScene.name); // Charge la nouvelle sc�ne
    }
}
