using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
public class MovingTwoPoints : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Rigidbody2D rb;

    private Vector2 currentTarget;
    public float speed = 2f;
    public float stoppingDistance = 0.1f;

    private Vector2 lastPosition;

    private void OnValidate()
    {
        if (pointA != null)
        {
            transform.position = pointA.position;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointA.position;
        lastPosition = rb.position;
    }

    void Update()
    {
        // Déplacer l'objet vers le point ciblé
        Vector2 position = rb.position;
        Vector2 direction = (currentTarget - position).normalized;
        rb.velocity = direction * speed;

        // Vérifie si l'objet est proche du point
        if (Vector2.Distance(position, currentTarget) <= stoppingDistance)
        {
            // Change la target
            currentTarget = (currentTarget == (Vector2)pointA.position) ? pointB.position : pointA.position;
        }
    }

    void FixedUpdate()
    {
        // Calcul du déplacement de la plateforme
        Vector2 deltaPosition = rb.position - lastPosition;

        // Notifie les enfants du déplacement
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                child.position += (Vector3)deltaPosition;
            }
        }

        lastPosition = rb.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
