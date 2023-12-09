using Photon.Pun;
using Photon.Pun.Demo.SlotRacer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false;
    InputMaster _controls;
    [SerializeField] Slider _volumeSlider;

    private void Awake()
    {
        _controls = new InputMaster();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (_controls.MenuNavigation.Pause.ReadValue<float>() > 0.1)
        {
            if (isPaused == true)
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
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ExitRoom()
    {
        StartCoroutine(ExitRoomAndLoad());
    }

    public void ChangeVolume()
    {
        AudioListener.volume = _volumeSlider.value;
    }

    IEnumerator ExitRoomAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }
        SceneManager.LoadScene("LobbyScene");
    }
}
