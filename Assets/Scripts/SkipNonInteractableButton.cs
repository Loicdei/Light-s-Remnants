using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//ChatGPT à été utiliser en majorité

public class SkipNonInteractableButton : MonoBehaviour
{
    private float navigationCooldown = 0.5f; // Temps minimum entre deux navigations (ralenti)
    private float lastNavigationTime = 0f;   // Moment de la dernière navigation
    private float verticalInputThreshold = 0.8f; // Seuil pour détecter une direction verticale

    void Update()
    {
        // Vérifier les entrées clavier, manette et D-pad
        float verticalInput = Input.GetAxis("Vertical");

        // Détecter les mouvements vers le bas ou le haut
        bool isDown = Input.GetKeyDown(KeyCode.DownArrow) || verticalInput < -verticalInputThreshold;
        bool isUp = Input.GetKeyDown(KeyCode.UpArrow) || verticalInput > verticalInputThreshold;

        // Navigation bas
        if (isDown && CanNavigate())
        {
            lastNavigationTime = Time.time;
            Navigate(Direction.Down);
        }

        // Navigation haut
        if (isUp && CanNavigate())
        {
            lastNavigationTime = Time.time;
            Navigate(Direction.Up);
        }
    }

    private bool CanNavigate()
    {
        // Vérifie si suffisamment de temps s'est écoulé depuis la dernière navigation
        return Time.time - lastNavigationTime > navigationCooldown;
    }

    private enum Direction
    {
        Up,
        Down
    }

    private void Navigate(Direction direction)
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) return;

        Selectable currentSelectable = current.GetComponent<Selectable>();
        if (currentSelectable == null) return;

        Transform parentPanel = current.transform.parent;
        Selectable nextSelectable = null;
        HashSet<Selectable> visited = new HashSet<Selectable>();

        // Navigation verticale
        if (direction == Direction.Down)
            nextSelectable = currentSelectable.FindSelectableOnDown();
        else if (direction == Direction.Up)
            nextSelectable = currentSelectable.FindSelectableOnUp();

        while (nextSelectable != null && (!nextSelectable.interactable || nextSelectable.transform.parent != parentPanel))
        {
            if (visited.Contains(nextSelectable)) break; // Empêche les boucles infinies
            visited.Add(nextSelectable);

            nextSelectable = direction == Direction.Down
                ? nextSelectable.FindSelectableOnDown()
                : nextSelectable.FindSelectableOnUp();
        }

        if (nextSelectable != null && nextSelectable.transform.parent == parentPanel)
        {
            StartCoroutine(SelectButtonWithDelay(nextSelectable));
        }
    }

    private IEnumerator SelectButtonWithDelay(Selectable button)
    {
        yield return new WaitForSeconds(0.1f); // Attendez un court délai avant de changer la sélection
        EventSystem.current.SetSelectedGameObject(null); // Réinitialiser la sélection
        EventSystem.current.SetSelectedGameObject(button.gameObject); // Sélectionner le bouton trouvé
    }
}
