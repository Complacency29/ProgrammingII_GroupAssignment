using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyStuff
{
    public class EnemyAttack : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerInventory>().CurHealth -= 40;
            }
        }
    }
}

