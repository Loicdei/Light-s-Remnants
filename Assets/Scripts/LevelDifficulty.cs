using UnityEngine;

public class LevelDifficulty : MonoBehaviour
{
    [SerializeField]
    private int deathThreshold = 5;
    [SerializeField]
    private float targetTime = 60f;

    private int deathCount = 0;
    private float startTime;

    void Start()
    {
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

        Difficulty newDifficulty;

        if (deathPercentage <= 25 && timePercentage <= 25)
        {
            newDifficulty = Difficulty.Veteran;
        }
        else if (deathPercentage <= 50 && timePercentage <= 50)
        {
            newDifficulty = Difficulty.Difficile;
        }
        else if (deathPercentage <= 75 && timePercentage <= 75)
        {
            newDifficulty = Difficulty.Moyen;
        }
        else
        {
            newDifficulty = Difficulty.Facile;
        }

        DifficultyManager.SetDifficulty(newDifficulty);
        ResetCounters();
    }

    private void ResetCounters()
    {
        deathCount = 0;
        startTime = Time.time;
    }
}
