using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;
    private Vector2 inputVector;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out position))
        {
            // Ajuster la magnitude du mouvement pour qu'il ne dépasse pas la limite
            position.x /= background.sizeDelta.x / 2;
            position.y /= background.sizeDelta.y / 2;
            inputVector = (position.magnitude > 1.0f) ? position.normalized : position;

            // Déplacer le stick
            handle.anchoredPosition = new Vector2(inputVector.x * (background.sizeDelta.x / 3), inputVector.y * (background.sizeDelta.y / 3));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Réinitialiser la position du stick et la direction
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public float Horizontal() => inputVector.x;  // Renvoie la direction horizontale
    public float Vertical() => inputVector.y;    // Renvoie la direction verticale
    public Vector2 Direction() => inputVector;   // Renvoie la direction complète
}
