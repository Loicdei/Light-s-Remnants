using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalLevel : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D transitionLight; // Lumière pour l'effet
    [SerializeField] private GrabController grabController;

    private bool playerInRange = false;
    private bool isTransitionning = false;

    void Start()
    {
        // Initialisation de la lumière à faible intensité
        transitionLight.intensity = 0f;
        transitionLight.pointLightOuterRadius = 0f;
    }

    void Update()
    {
        if (playerInRange && grabController.isHoldingLantern() && Input.GetKeyDown(KeyCode.E) && !isTransitionning)
        {
            isTransitionning = true;
            StartCoroutine(TransitionLevel());
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
        StartCoroutine(TransitionLightEffect());

        yield return new WaitForSeconds(1f); // Attend la fin de l'effet lumineux
        SceneManager.LoadSceneAsync(targetScene); // Charge la nouvelle scène
    }

    private IEnumerator TransitionLightEffect()
    {
        float duration = 1f;
        float startIntensity = 0f;
        float endIntensity = 50f; // Augmentez cette valeur pour un effet plus intense
        float startRadius = 0f;
        float endRadius = 20f; // Augmentez cette valeur pour un rayon plus large

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            transitionLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
            transitionLight.pointLightOuterRadius = Mathf.Lerp(startRadius, endRadius, t);

            yield return null;
        }

        // Assurez-vous que la lumière atteint les valeurs finales
        transitionLight.intensity = endIntensity;
        transitionLight.pointLightOuterRadius = endRadius;
    }

}
