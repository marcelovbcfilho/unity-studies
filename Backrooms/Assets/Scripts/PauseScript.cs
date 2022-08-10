using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static bool GameIsPaused;

    [Tooltip("Pause menu canvas")] public GameObject pauseMenuUI;

    [Tooltip("Pause menu canvas")] public List<GameObject> playerUIs = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Play();
            else
                Pause();
        }
    }

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        ToggleOtherPlayerUIs();
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        ToggleOtherPlayerUIs();
        Time.timeScale = 0f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void ToggleOtherPlayerUIs()
    {
        foreach (var playerUI in playerUIs) playerUI.SetActive(!GameIsPaused);
    }
}