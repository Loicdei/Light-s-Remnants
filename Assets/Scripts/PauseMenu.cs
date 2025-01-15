using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f; // Arr�te le temps de jeu
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f; // Reprend le temps de jeu
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Assurez-vous que le temps est repris avant de changer de sc�ne
        SceneManager.LoadScene("MenuStatic"); // Remplacez "MainMenu" par le nom de votre sc�ne de menu principal
    }
}
