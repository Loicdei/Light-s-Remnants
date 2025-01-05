using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Transform playerSpawn;
    public bool OneTimeUse;

    private Vector3 spawnPoint;

    private void Awake()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        spawnPoint = transform.GetChild(0).position;
        Debug.Log(spawnPoint);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerSpawn.position = spawnPoint;
            if (OneTimeUse)
            {
                Destroy(gameObject);
            }
        }
    }
}