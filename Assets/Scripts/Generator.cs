using System.Collections;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public Transform pointB;  // Point d'arrivée (Transform)
    public GameObject objectToClone;  // L'objet à cloner
    public float cloneInterval = 2f;  // Intervalle entre chaque clonage
    public float moveSpeed = 5f;  // Vitesse de déplacement de l'objet
    public float lifetime = 10f;  // Temps avant destruction de l'objet (si non atteint le point B)

    private void Start()
    {
        StartCoroutine(GenerateObjects());
    }

    private IEnumerator GenerateObjects()
    {
        while (true)
        {
            // Clone l'objet à la position du générateur (transform)
            GameObject newObject = Instantiate(objectToClone, transform.position, Quaternion.identity);

            // Déplace l'objet vers le point B
            StartCoroutine(MoveObject(newObject));

            // Attend l'intervalle avant de générer le prochain objet
            yield return new WaitForSeconds(cloneInterval);
        }
    }

    private IEnumerator MoveObject(GameObject obj)
    {
        float startTime = Time.time;
        Vector3 startPosition = obj.transform.position;
        Vector3 targetPosition = pointB.position;

        // Déplacement vers le point B
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Détruire l'objet après avoir atteint le point B
        Destroy(obj);
    }

    private IEnumerator DestroyObjectAfterTime(GameObject obj)
    {
        // Attente de la durée avant la destruction
        yield return new WaitForSeconds(lifetime);
        Destroy(obj);
    }

    // Méthode pour dessiner les Gizmos dans l'éditeur
    private void OnDrawGizmos()
    {
        // Vérifie que pointB est défini avant de dessiner
        if (pointB != null)
        {
            // Définir la couleur des Gizmos pour les points
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.2f);  // Point de départ

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pointB.position, 0.2f);  // Point B

            // Dessiner une ligne entre les deux points
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, pointB.position);
        }
    }
}
