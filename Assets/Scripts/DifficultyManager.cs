using UnityEngine;

public static class DifficultyManager
{
    private const string DifficultyKey = "SavedDifficulty";

    public static Difficulty GetDifficulty()
    {
        if (PlayerPrefs.HasKey(DifficultyKey))
        {
            Debug.Log((Difficulty)PlayerPrefs.GetInt(DifficultyKey));
            return (Difficulty)PlayerPrefs.GetInt(DifficultyKey);
        }
        return Difficulty.Moyen; 
    }

    public static void SetDifficulty(Difficulty difficulty)
    {
        Debug.Log(difficulty.ToString());
        PlayerPrefs.SetInt(DifficultyKey, (int)difficulty);
        PlayerPrefs.Save();
    }

    public static void ResetDifficulty()
    {
        PlayerPrefs.DeleteKey(DifficultyKey);
    }
}
