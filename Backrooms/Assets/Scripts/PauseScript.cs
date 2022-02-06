using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Tooltip("Pause menu canvas")]
    public GameObject pauseMenuUI;

    [Tooltip("Pause menu canvas")]
    public List<GameObject> playerUIs = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                this.Play();
            }
            else
            {
                this.Pause();
            }
        }
    }

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameIsPaused = false;
        this.pauseMenuUI.SetActive(false);
        this.ToggleOtherPlayerUIs();
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        GameIsPaused = true;
        this.pauseMenuUI.SetActive(true);
        this.ToggleOtherPlayerUIs();
        Time.timeScale = 0f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    void ToggleOtherPlayerUIs()
    {
        foreach (GameObject playerUI in this.playerUIs)
        {
            playerUI.SetActive(!GameIsPaused);
        }
    }
}
