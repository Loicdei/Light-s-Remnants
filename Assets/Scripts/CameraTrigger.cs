using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public Vector2 newCameraPosition;
    public float transitionTime = 1f;
    public float newCameraSize = 5; // zoom (de base 5)
    [SerializeField] private CameraFollow cameraFollow; // R�f�rence au script CameraFollow

    private Camera mainCamera; // R�f�rence � la cam�ra principale
    private float originalCameraSize; // Taille d'origine de la cam�ra
    private Coroutine moveCameraCoroutine; // R�f�rence � la coroutine de mouvement de la cam�ra

    private void Start()
    {
        mainCamera = Camera.main;
        originalCameraSize = mainCamera.orthographicSize; // Stockez la taille d'origine ici
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraFollow.SetFollowing(false); // D�sactive le suivi de la cam�ra
            if (moveCameraCoroutine != null)
            {
                StopCoroutine(moveCameraCoroutine); // Stoppe la coroutine si elle est d�j� en cours
            }
            moveCameraCoroutine = StartCoroutine(MoveCamera());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Vérifier si le GameObject est actif
   
        if (collision.CompareTag("Player"))
        {
            if (moveCameraCoroutine != null)
            {
                StopCoroutine(moveCameraCoroutine); // Stoppe la coroutine si elle est en cours
            }
            moveCameraCoroutine = StartCoroutine(MoveCameraBack());
            cameraFollow.SetFollowing(true); // R�active le suivi de la cam�ra
        }
    }

    private IEnumerator MoveCamera()
    {
        Vector3 originalPosition = mainCamera.transform.position;
        Vector3 newPosition = new Vector3(newCameraPosition.x, newCameraPosition.y, -10);

        // Utiliser la taille actuelle de la cam�ra comme point de d�part
        float startSize = mainCamera.orthographicSize;

        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            // D�place la cam�ra
            mainCamera.transform.position = Vector3.Lerp(originalPosition, newPosition, (elapsedTime / transitionTime));

            // Zoom sur la nouvelle taille de la cam�ra
            mainCamera.orthographicSize = Mathf.Lerp(startSize, newCameraSize, (elapsedTime / transitionTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assurez-vous que la cam�ra est � la nouvelle position et taille
        mainCamera.transform.position = newPosition;
        mainCamera.orthographicSize = newCameraSize;
    }

    private IEnumerator MoveCameraBack()
    {
        // Utiliser la taille actuelle de la cam�ra comme point de d�part
        float startSize = mainCamera.orthographicSize;

        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            float currentSize = Mathf.Lerp(startSize, originalCameraSize, (elapsedTime / transitionTime));
            mainCamera.orthographicSize = currentSize;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assurez-vous que la taille de la cam�ra est bien r�tablie
        mainCamera.orthographicSize = originalCameraSize;
    }
}
