using UnityEngine;
using static PlayerDifficulty;

public class SpikeManager : MonoBehaviour
{
    private GameObject[] obstacles; 
    public PlayerDifficulty playerDifficulty;
    public Difficulty currentDifficulty;

    void Start()
    {
        playerDifficulty = GetComponent<PlayerDifficulty>();
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        AdjustObstacles();
    }
    void Update()
    {
        currentDifficulty = PlayerDifficulty.currentDifficulty;
    }
    private void AdjustObstacles()
    {
        foreach (GameObject obstacle in obstacles)
        {
            ObstacleDifficulty currentObstacle = obstacle.GetComponent<ObstacleDifficulty>();

            if (currentObstacle != null)
            {
                ObstacleDifficulty.Difficulty spikeDifficulty = currentObstacle.GetSpikeDifficulty();

                if (ObstacleHarderThanPlayer(spikeDifficulty))
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

    private bool ObstacleHarderThanPlayer(ObstacleDifficulty.Difficulty spikeDifficulty)
    {
        return spikeDifficulty > currentDifficulty;
    }
}
