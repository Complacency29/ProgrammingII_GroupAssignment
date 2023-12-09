using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionPickup : MonoBehaviour
{
    [SerializeField] public AudioClip pickup;
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
            AudioManager.instance.PlaySFX(pickup);
            inventory.CurHealthPotions++;
            Destroy(gameObject);
        }
    }
}
