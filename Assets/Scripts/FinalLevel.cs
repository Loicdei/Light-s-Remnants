using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalLevel : MonoBehaviour
{
    [SerializeField] private SceneAsset targetScene; // Sc�ne cible pour la transition
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D transitionLight; // Lumi�re pour l'effet
    [SerializeField] GrabController grabController;


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
        if (playerInRange && grabController.isHoldingLantern() && Input.GetKeyDown(KeyCode.E) && !isTransitioning)
        {
            StartCoroutine(TransitionLevel());
        }

        // Transition progressive de la lumi�re si elle est en cours de transition
        if (isTransitioning)
        {
            transitionLight.intensity = Mathf.Lerp(transitionLight.intensity, 500f, Time.deltaTime * 1);
            transitionLight.pointLightOuterRadius = Mathf.Lerp(transitionLight.pointLightOuterRadius, 500f, Time.deltaTime * 1);
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
