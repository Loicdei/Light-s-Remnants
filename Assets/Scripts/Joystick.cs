using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;
    private Vector2 inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position - (Vector2)background.position;
        inputVector = Vector2.ClampMagnitude(pos / (background.sizeDelta / 2), 1);
        handle.anchoredPosition = inputVector * (background.sizeDelta / 2);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public float Horizontal() => inputVector.x;
}
