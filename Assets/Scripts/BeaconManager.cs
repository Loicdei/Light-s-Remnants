using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconManager : MonoBehaviour
{
    public static BeaconManager instance;  // Singleton pour un acc�s global
    private Beacon[] beacons;  // Tableau contenant toutes les balises dans la sc�ne

    void Awake()
        // Cr�e l'instant pour pouvoir l'appeler
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        beacons = FindObjectsOfType<Beacon>();  // Trouver toutes les balises dans la sc�ne
    }

    // Fonction pour v�rifier si toutes les balises sont allum�es
    public void CheckAllBeaconsLit()
    {
        foreach (Beacon beacon in beacons)
        {
            if (!beacon.IsLit())
            {
                return;
            }
        }
        Debug.Log("Toutes les balises sont allum�es !");
    }
}
