using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public Vector2 newCameraPosition;
    public float transitionTime = 1f;
    public float newCameraSize = 5; // zoom (de base 5)
    [SerializeField] private CameraFollow cameraFollow; // Référence au script CameraFollow

    private Camera mainCamera; // Référence à la caméra principale
    private float originalCameraSize; // Taille d'origine de la caméra
    private Coroutine moveCameraCoroutine; // Référence à la coroutine de mouvement de la caméra

    private void Start()
    {
        mainCamera = Camera.main;
        originalCameraSize = mainCamera.orthographicSize; // Stockez la taille d'origine ici
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraFollow.SetFollowing(false); // Désactive le suivi de la caméra
            if (moveCameraCoroutine != null)
            {
                StopCoroutine(moveCameraCoroutine); // Stoppe la coroutine si elle est déjà en cours
            }
            moveCameraCoroutine = StartCoroutine(MoveCamera());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (moveCameraCoroutine != null)
            {
                StopCoroutine(moveCameraCoroutine); // Stoppe la coroutine si elle est en cours
            }
            moveCameraCoroutine = StartCoroutine(MoveCameraBack());
            cameraFollow.SetFollowing(true); // Réactive le suivi de la caméra
        }
    }

    private IEnumerator MoveCamera()
    {
        Vector3 originalPosition = mainCamera.transform.position;
        Vector3 newPosition = new Vector3(newCameraPosition.x, newCameraPosition.y, -10);

        // Utiliser la taille actuelle de la caméra comme point de départ
        float startSize = mainCamera.orthographicSize;

        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            // Déplace la caméra
            mainCamera.transform.position = Vector3.Lerp(originalPosition, newPosition, (elapsedTime / transitionTime));

            // Zoom sur la nouvelle taille de la caméra
            mainCamera.orthographicSize = Mathf.Lerp(startSize, newCameraSize, (elapsedTime / transitionTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assurez-vous que la caméra est à la nouvelle position et taille
        mainCamera.transform.position = newPosition;
        mainCamera.orthographicSize = newCameraSize;
    }

    private IEnumerator MoveCameraBack()
    {
        // Utiliser la taille actuelle de la caméra comme point de départ
        float startSize = mainCamera.orthographicSize;

        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            float currentSize = Mathf.Lerp(startSize, originalCameraSize, (elapsedTime / transitionTime));
            mainCamera.orthographicSize = currentSize;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assurez-vous que la taille de la caméra est bien rétablie
        mainCamera.orthographicSize = originalCameraSize;
    }
}
