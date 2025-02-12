using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDifficulty : MonoBehaviour
{
    public enum Difficulty
    {
        Facile,
        Moyen,
        Difficile,
        Veteran
    }

    [SerializeField]
    private Difficulty spikeDifficulty;

    public Difficulty GetSpikeDifficulty()
    {
        return spikeDifficulty;
    }
}


