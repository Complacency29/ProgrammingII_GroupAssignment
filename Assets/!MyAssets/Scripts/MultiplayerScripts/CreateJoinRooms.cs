using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _createInput;
    [SerializeField] private TMP_InputField _joinInput;

    public void CreateRoomButton()
    {
        if(_createInput.text == string.Empty)
        {
            return;
        }

        PhotonNetwork.CreateRoom(_createInput.text);
    }

    public void JoinRoomButton()
    {
        if (_joinInput.text == string.Empty)
        {
            return;
        }

        PhotonNetwork.JoinRoom(_joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GeneratorScene");        
    }
}
