using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    private GameObject[] obstacles;
    private Difficulty currentDifficulty;

    void Start()
    {
        if (!PlayerPrefs.HasKey("SavedDifficulty"))
        {
            DifficultyManager.SetDifficulty(Difficulty.Moyen); // D�finit la difficult� par d�faut
            Debug.Log("Difficult� initialis�e � Moyen.");
        }
        AdjustObstacles();
    }

    private void AdjustObstacles()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        currentDifficulty = DifficultyManager.GetDifficulty();

        foreach (GameObject obstacle in obstacles)
        {
            ObstacleDifficulty currentObstacle = obstacle.GetComponent<ObstacleDifficulty>();

            if (currentObstacle != null)
            {
                Difficulty spikeDifficulty = currentObstacle.GetSpikeDifficulty();

                if (spikeDifficulty > currentDifficulty)
                {
                    obstacle.SetActive(false);
                }
                else
                {
                    obstacle.SetActive(true);
                }
            }
        }
    }
}
