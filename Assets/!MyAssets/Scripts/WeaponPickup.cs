using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

[RequireComponent(typeof(SphereCollider))]
public class WeaponPickup : MonoBehaviour
{
    [SerializeField] WeaponBase weapon;

    InputMaster controls;
    bool playerInRange = false;
    PhotonView currentPhotonView;
    bool weaponPickedUp = false;
    PlayerInventory playerInventory;

    private void Awake()
    {
        controls = new InputMaster();
    }

    private void Update()
    {
        // Check for input if a player is in range AND ensure that this photon view belongs to this client
        if (playerInRange && currentPhotonView != null && currentPhotonView.IsMine)
        {
            // Check for pickup input
            if (controls.PlayerActions.Interact.ReadValue<float>() > 0.1f)
            {
                // Call a method to handle the weapon pickup
                if (weaponPickedUp)
                    return;
                weaponPickedUp = true;
                PickUpWeapon();
            }
        }
    }

    private void PickUpWeapon()
    {
        if(playerInventory != null)
        {
            //check if the player has a weapon already
            if(playerInventory.EquippedWeapon != null)
            {
                Debug.Log("There is already a weapon equipped. Drop the weapon first");
                return;
            }
            playerInventory.EquippedWeapon = weapon;
            Debug.Log("Weapon picked up: " + weapon.WeaponName);
            Destroy(gameObject);
        }
        else
        {
            //playerInventory is null
            Debug.Log("No player inventory!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if this is a player
        if(other.tag == "Player")
        {
            //this is a player
            playerInRange = true;
            currentPhotonView = other.GetComponent<PhotonView>();
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
            currentPhotonView = null;
            playerInventory = null;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
