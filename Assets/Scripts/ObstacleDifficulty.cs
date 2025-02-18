using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDifficulty : MonoBehaviour
{
    [SerializeField]
    private Difficulty spikeDifficulty;

    public Difficulty GetSpikeDifficulty()
    {
        return spikeDifficulty;
    }

}


