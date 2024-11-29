using UnityEngine;
using static System.TimeZoneInfo;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    private bool isFollowing = true; // Vérifie si elle doit suivre le joueur ou non

    [SerializeField] private Transform target;

    // Bordures personnalisées
    [SerializeField] private float minX = -5f; // Bordure gauche
    [SerializeField] private float maxX = 100f;  // Bordure droite
    [SerializeField] private float minY = 0f;    // Bordure basse
    [SerializeField] private float maxY = 100f;   // Bordure haute

    private void Update()
    {
        if (isFollowing)
        {
            Vector3 targetPosition = target.position + offset;

            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
    public void SetFollowing(bool following)
    {
        isFollowing = following;
    }

    public void setSmoothTime(float newTime)
    {
        smoothTime = newTime;
    }
}
