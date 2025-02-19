using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDifficulty : MonoBehaviour
{
    [SerializeField]
    private Difficulty playerDifficulty;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public Difficulty GetPlayerDifficulty()
    {
        return playerDifficulty;
    }

    public void SetPlayerDifficulty(Difficulty difficulty)
    {
        playerDifficulty = difficulty;
    }


}


