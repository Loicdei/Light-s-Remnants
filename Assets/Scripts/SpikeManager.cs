using UnityEngine;
using static PlayerDifficulty;

public class SpikeManager : MonoBehaviour
{
    private GameObject[] obstacles; 
    public PlayerDifficulty playerDifficulty;
    public Difficulty currentDifficulty;

    void Start()
    {
        playerDifficulty = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDifficulty>();
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        AdjustObstacles();
    }
    void Update()
    {
        currentDifficulty = playerDifficulty.currentDifficulty;
    }
    private void AdjustObstacles()
    {
        foreach (GameObject obstacle in obstacles)
        {
            ObstacleDifficulty currentObstacle = obstacle.GetComponent<ObstacleDifficulty>();

            if (currentObstacle != null)
            {
                Difficulty spikeDifficulty = currentObstacle.GetSpikeDifficulty();

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

    private bool ObstacleHarderThanPlayer(Difficulty spikeDifficulty)
    {
        return spikeDifficulty > currentDifficulty;
    }
}
