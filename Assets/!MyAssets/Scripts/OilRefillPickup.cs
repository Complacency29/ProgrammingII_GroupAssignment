using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilRefillPickup : MonoBehaviour
{
    [SerializeField] private AudioClip pickup;
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if(inventory != null)
        {
            if (inventory.CurOilRefill >= inventory.MaxOilRefill)
            {
                //player already has max health potions
                return;
            }

            //we don't have max potions, so add one and remove the game object
            AudioManager.instance.PlaySFX(pickup);
            inventory.CurOilRefill++;
            Destroy(gameObject);
        }
    }
}
