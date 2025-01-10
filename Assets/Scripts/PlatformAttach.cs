using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    private Vector3 lastPlatformPosition;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            lastPlatformPosition = transform.position;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 deltaPosition = transform.position - lastPlatformPosition;
            collision.transform.position += new Vector3(deltaPosition.x, 0, 0);  // Applique seulement le mouvement sur X
            lastPlatformPosition = transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            lastPlatformPosition = Vector3.zero;
        }
    }
}
