using Photon.Pun.Demo.SlotRacer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //need to find other inputs and do an if statement around them
    public GameObject pauseMenu;
    private static bool _isPaused;
    private InputMaster _controls;

    private void Awake()
    {
        _controls = new InputMaster();
    }

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (_controls.MenuNavigation.Pause.ReadValue<float>() > 0.1)
        {
            if (_isPaused)
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
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ExitRoom()
    {

    }

    public void Settings()
    {

    }
}
