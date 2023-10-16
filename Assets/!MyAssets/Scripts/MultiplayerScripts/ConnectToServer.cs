using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        if (MasterManager.Instance == null)
        {
            Debug.LogError("Error: Master Manager Not Found!");
            return;
        }

        PhotonNetwork.NickName = "Player" + Random.Range(0, int.MaxValue);
        PhotonNetwork.GameVersion = MasterManager.Instance.gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + ": Has joined the game.");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        LobbyStateManager.Instance?.ChangeLobbyState(LobbyStateManager.LobbyState.MainLobby);
    }
}
