using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour
{
    public Transform pointA; // Point de départ
    public Transform pointB; // Point d'arrivée
    public float spawnInterval = 1f; // Intervalle de génération en secondes
    public float lifetime = 5f; // Durée de vie maximale de l'objet cloné

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            GameObject clone = Instantiate(gameObject, pointA.position, Quaternion.identity);
            Destroy(clone, lifetime);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Update()
    {
        GameObject[] clones = GameObject.FindGameObjectsWithTag(gameObject.tag);
        foreach (GameObject clone in clones)
        {
            if (Vector3.Distance(clone.transform.position, pointB.position) < 0.1f)
            {
                Destroy(clone);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            // Dessiner une sphère rouge au point A
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointA.position, 0.1f);

            // Dessiner une sphère verte au point B
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pointB.position, 0.1f);

            // Dessiner une ligne entre point A et point B
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
