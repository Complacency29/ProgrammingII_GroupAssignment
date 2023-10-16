using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStateManager : MonoBehaviorSingleton<LobbyStateManager>
{
    [SerializeField] private Transform _loadingCanvas;
    [SerializeField] private Transform _mainLobbyCanvas;
    
    protected override void Awake()
    {
        base.Awake();

        ChangeLobbyState(LobbyState.Init);
    }
    
    public void ChangeLobbyState(LobbyState newState)
    {
        switch (newState)
        {
            case LobbyState.Init:
                _loadingCanvas?.gameObject.SetActive(false);
                _mainLobbyCanvas?.gameObject.SetActive(false);

                ChangeLobbyState(LobbyState.Loading);
                break;
            case LobbyState.Loading:
                _loadingCanvas?.gameObject.SetActive(true);
                _mainLobbyCanvas?.gameObject.SetActive(false);
                break;
            case LobbyState.MainLobby:
                _loadingCanvas?.gameObject.SetActive(false);
                _mainLobbyCanvas?.gameObject.SetActive(true);
                break;
        }
    }

    public enum LobbyState
    {
        Init,
        Loading,
        MainLobby
    }
}
