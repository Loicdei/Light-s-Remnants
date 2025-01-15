using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkipNonInteractableButton : MonoBehaviour
{
    private float verticalInputThreshold = 0.1f; // Seuil pour détecter une direction verticale

    private enum Direction
    {
        Up,
        Down
    }

    void Update()
    {
        // Vérifier les entrées clavier, manette et D-pad
        float verticalInput = Input.GetAxis("Vertical");

        // Navigation bas
        if (Input.GetKeyDown(KeyCode.DownArrow) || verticalInput < -verticalInputThreshold)
        {
            Navigate(Direction.Down);
        }

        // Navigation haut
        if (Input.GetKeyDown(KeyCode.UpArrow) || verticalInput > verticalInputThreshold)
        {
            Navigate(Direction.Up);
        }
    }

    private void Navigate(Direction direction)
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) return;

        Selectable currentSelectable = current.GetComponent<Selectable>();
        if (currentSelectable == null) return;

        Selectable nextSelectable = null;
        HashSet<Selectable> visited = new HashSet<Selectable>();

        while (true)
        {
            nextSelectable = direction == Direction.Down
                ? currentSelectable.FindSelectableOnDown()
                : currentSelectable.FindSelectableOnUp();

            // Si aucun autre sélectable ou si un cycle est détecté
            if (nextSelectable == null || visited.Contains(nextSelectable))
                break;

            visited.Add(nextSelectable);

            // Si le prochain sélectable est interactable, on le sélectionne
            if (nextSelectable.interactable)
            {
                EventSystem.current.SetSelectedGameObject(nextSelectable.gameObject);
                break;
            }

            currentSelectable = nextSelectable;
        }
    }
}
