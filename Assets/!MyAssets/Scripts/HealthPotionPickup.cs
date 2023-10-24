using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            if (inventory.CurHealthPotions >= inventory.MaxHealthPotions)
            {
                //player already has max health potions
                return;
            }

            //we don't have max potions, so add one and remove the game object
            inventory.CurHealthPotions++;
            Destroy(gameObject);
        }
    }
}
