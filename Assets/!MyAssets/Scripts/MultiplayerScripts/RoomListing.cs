using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public RoomInfo MyRoomInfo {  get; private set; }

    public void SetRoomInfo(RoomInfo ri)
    {
        MyRoomInfo = ri;
        _text.text = ri.PlayerCount + "/" + ri.MaxPlayers + ": " + ri.Name;
    }
}
