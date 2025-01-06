using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkipNonInteractableButton : MonoBehaviour
{
    private float navigationCooldown = 0.3f; // Temps minimum entre deux navigations
    private float lastNavigationTime = 0f;  // Moment de la derni�re navigation
    private float verticalInputThreshold = 0.5f; // Seuil pour d�tecter une direction verticale

    void Update()
    {
        // V�rifier les entr�es clavier, manette et D-pad
        float verticalInput = Input.GetAxis("Vertical");

        // D�tecter les mouvements vers le bas ou le haut
        bool isDown = Input.GetKeyDown(KeyCode.DownArrow) || verticalInput < -verticalInputThreshold;
        bool isUp = Input.GetKeyDown(KeyCode.UpArrow) || verticalInput > verticalInputThreshold;

        // Navigation bas
        if (isDown && Time.time - lastNavigationTime > navigationCooldown)
        {
            lastNavigationTime = Time.time;
            NavigateDown();
        }

        // Navigation haut
        if (isUp && Time.time - lastNavigationTime > navigationCooldown)
        {
            lastNavigationTime = Time.time;
            NavigateUp();
        }
    }

    private void NavigateDown()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) { return; }

        Selectable currentSelectable = current.GetComponent<Selectable>();
        if (currentSelectable == null) { return; }

        // Trouver le prochain �l�ment interactif en bas
        Selectable nextSelectable = currentSelectable.FindSelectableOnDown();
        while (nextSelectable != null && !nextSelectable.interactable)
        {
            nextSelectable = nextSelectable.FindSelectableOnDown();
        }

        // S�lectionner l'�l�ment trouv�
        if (nextSelectable != null)
        {
            EventSystem.current.SetSelectedGameObject(nextSelectable.gameObject);
        }
    }

    private void NavigateUp()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) { return; }

        Selectable currentSelectable = current.GetComponent<Selectable>();
        if (currentSelectable == null) { return; }

        // Trouver le prochain �l�ment interactif en haut
        Selectable previousSelectable = currentSelectable.FindSelectableOnUp();
        while (previousSelectable != null && !previousSelectable.interactable)
        {
            previousSelectable = previousSelectable.FindSelectableOnUp();
        }

        // S�lectionner l'�l�ment trouv�
        if (previousSelectable != null)
        {
            EventSystem.current.SetSelectedGameObject(previousSelectable.gameObject);
        }
    }
}
