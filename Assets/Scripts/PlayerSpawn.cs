using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += 1.0f;
            player.transform.position = spawnPosition;
        }
    }
}
