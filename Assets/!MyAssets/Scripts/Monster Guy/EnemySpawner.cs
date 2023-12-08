using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace EnemyStuff
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] float spawnCooldown = 5f;
        GameObject enemy;
        bool spawning;

        // Start is called before the first frame update
        void Start()
        {
            enemy = Resources.Load("Enemy") as GameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if(!spawning && FindObjectsOfType<Enemy>().Length == 0)
            {
                if(Photon.Pun.PhotonNetwork.IsMasterClient)
                {
                    StartCoroutine(Spawn());
                }
            }
        }

        private IEnumerator Spawn()
        {
            spawning = true;
            yield return new WaitForSecondsRealtime(spawnCooldown);
            Photon.Pun.PhotonView.Instantiate(enemy, transform);
            spawning = false;
            yield break;
        }
    }
}

