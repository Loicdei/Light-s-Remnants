using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BresonController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Rigidbody2D rb;

    private Vector2 currentTarget;
    public float speed = 2f;
    public float stoppingDistance = 0.1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointA.position;
    }

    void Update()
    {
        // Déplacer l'ennemi vers le point ciblé
        Vector2 position = rb.position;
        Vector2 direction = (currentTarget - position).normalized;
        rb.velocity = direction * speed;

        // Vérifie si l'ennemi est proche du point
        if (Vector2.Distance(position, currentTarget) <= stoppingDistance)
        {
            // Change la target
            currentTarget = (currentTarget == (Vector2)pointA.position) ? pointB.position : pointA.position;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}