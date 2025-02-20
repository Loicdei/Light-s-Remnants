using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject firstButton;
    private bool isPaused = false;
    private Button selectedButton;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            SetPause();
        }

        selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
    }

    public void SetPause()
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

    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Sélectionne automatiquement le premier bouton
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Désélectionne les boutons
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuStatic");
    }
}
