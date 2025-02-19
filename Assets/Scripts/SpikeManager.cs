using UnityEngine;
using static PlayerDifficulty;

public class SpikeManager : MonoBehaviour
{
    private GameObject[] obstacles; 
    private PlayerDifficulty playerDifficulty;
    public Difficulty currentDifficulty;

    void Start()
    {
        playerDifficulty = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDifficulty>();
        AdjustObstacles();
    }
    void Update()
    {
    }
    private void AdjustObstacles()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        currentDifficulty = playerDifficulty.currentDifficulty;

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
