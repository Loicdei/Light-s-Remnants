using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerDifficulty;

public class LevelDifficulty : MonoBehaviour
{
    [SerializeField]
    private int deathThreshold = 5;
    [SerializeField]
    private float targetTime = 60f;

    private int deathCount = 0;
    private float startTime;        // Temps de départ pour calculer le temps passé

    private PlayerDifficulty playerDifficulty;
    private Difficulty currentDifficulty;

    void Start()
    {
        playerDifficulty = GameObject.FindGameObjectWithTag("PlayerDifficultySave").GetComponent<PlayerDifficulty>();
        startTime = Time.time;
    }

    public void IncrementDeathCount()
    {
        deathCount++;
        Debug.Log("Mort du joueur : " + deathCount);
    }

    public void CalculateDifficulty()
    {
        float timeSpent = Time.time - startTime;

        float deathPercentage = ((float)deathCount / deathThreshold) * 100;
        float timePercentage = (timeSpent / targetTime) * 100;

        currentDifficulty = playerDifficulty.GetPlayerDifficulty();

        if (deathPercentage <= 25 && timePercentage <= 25)
        {
            currentDifficulty = Difficulty.Veteran;
        }
        else if (deathPercentage <= 50 && timePercentage <= 50)
        {
            currentDifficulty = Difficulty.Difficile;
        }
        else if (deathPercentage <= 75 && timePercentage <= 75)
        {
            currentDifficulty = Difficulty.Moyen;
        }
        else
        {
            currentDifficulty = Difficulty.Facile;
        }

        playerDifficulty.SetPlayerDifficulty(currentDifficulty);

        ResetDifficulty();
    }
    private void ResetDifficulty()
    {
        deathCount = 0;
        startTime = Time.time;
    }
}
