using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModuleSnapping
{
    public class PlayerHandler : MonoBehaviorSingleton<PlayerHandler>
    {
        [SerializeField] private GameObject playerPrefab;

        public void Start()
        {
            foreach (Photon.Realtime.Player player in Photon.Pun.PhotonNetwork.PlayerList)
            {
                Debug.Log(player.NickName);
            }

            Photon.Pun.PhotonNetwork.Instantiate(playerPrefab.name, transform.position, Quaternion.identity);
        }

        public void OnPhotonPlayerConnected(Photon.Realtime.Player player)
        {
            Debug.Log("bruh1");
        }
    }
}

