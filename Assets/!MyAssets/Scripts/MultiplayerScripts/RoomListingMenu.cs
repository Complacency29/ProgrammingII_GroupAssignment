using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomListing _roomListingPrefab;

    private List<RoomListing> _roomListings = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo item in roomList)
        {
            if (item.RemovedFromList)
            {
                int index = _roomListings.FindIndex(x => x.MyRoomInfo.Name == item.Name);

                if (index != -1)
                {
                    Destroy(_roomListings[index].gameObject);
                    _roomListings.RemoveAt(index);
                }
            }
            else
            {
                RoomListing rl = Instantiate(_roomListingPrefab, _content);
                rl.SetRoomInfo(item);
                _roomListings.Add(rl);
            }
        }
    }
}
