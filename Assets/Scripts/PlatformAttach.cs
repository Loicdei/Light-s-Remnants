using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsPlayerAbove(collision))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!IsPlayerAbove(collision))
            {
                collision.transform.SetParent(null);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    private bool IsPlayerAbove(Collision2D collision)
    {
        // Récupère la position du joueur et de la plateforme
        Vector3 playerPosition = collision.transform.position;
        Vector3 platformPosition = transform.position;

        // Vérifie si le joueur est légèrement au-dessus de la plateforme
        // Ajuste les marges selon la hauteur de ton collider
        float playerBottom = playerPosition.y - collision.collider.bounds.extents.y;
        float platformTop = platformPosition.y + GetComponent<Collider2D>().bounds.extents.y;

        return playerBottom >= platformTop;
    }
}
