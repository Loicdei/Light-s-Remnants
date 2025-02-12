using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDifficulty : MonoBehaviour
{
    public enum Difficulty
    {
        Facile,
        Moyen,
        Difficile,
        Veteran
    }

    public Difficulty currentDifficulty;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateDifficulty()
    {
        
    }
}
