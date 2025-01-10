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
        currentTarget = pointB.position;  // Initialement se dirige vers pointB
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float distanceToTarget = Vector2.Distance(transform.position, currentTarget);

        if (distanceToTarget < stoppingDistance)
        {
            currentTarget = currentTarget == (Vector2)pointA.position ? pointB.position : pointA.position;
        }

        Vector2 direction = (currentTarget - (Vector2)transform.position).normalized;
        Vector2 newPosition = (Vector2)transform.position + direction * speed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);  // Utilisation de MovePosition pour déplacer la plateforme
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
