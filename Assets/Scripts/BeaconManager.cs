using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconManager : MonoBehaviour
{
    public static BeaconManager instance;  // Singleton pour un accès global
    private Beacon[] beacons;  // Tableau contenant toutes les balises dans la scène

    void Awake()
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
        beacons = FindObjectsOfType<Beacon>();  // Trouver toutes les balises dans la scène
    }

    // Fonction pour vérifier si toutes les balises sont allumées
    public void CheckAllBeaconsLit()
    {
        foreach (Beacon beacon in beacons)
        {
            if (!beacon.IsLit())
            {
                return;  // Si une balise n'est pas allumée, on quitte
            }
        }
        Debug.Log("Toutes les balises sont allumées !");
    }
}
