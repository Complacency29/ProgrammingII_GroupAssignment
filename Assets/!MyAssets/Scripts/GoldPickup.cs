using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

[RequireComponent(typeof(SphereCollider))]
public class GoldPickup : MonoBehaviour
{
    [SerializeField, Min(0)] int amount;
    [SerializeField] bool useRandomGoldAmount = false;
    [SerializeField, Range(1, 1000)] int randomAmountMin = 5;
    [SerializeField, Range(1, 1000)] int randomAmountMax = 100;
    [SerializeField, Range(1, 3)] float pickupRange = 1.5f;

    [SerializeField] int goldThreshold = 500;
    public GameObject[] portalObjects;

    public int Amount { get { return amount; } }
    public int getGoldThreshold {  get { return goldThreshold; } }

    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        GetComponent<SphereCollider>().radius = pickupRange;
    }
    private void Start()
    {
        if(useRandomGoldAmount)
        {
            int rng = Random.Range(randomAmountMin, randomAmountMax + 1);
            amount = rng;
        }

        //Finds tags of "Portal" and turns them off to begin
        portalObjects = GameObject.FindGameObjectsWithTag("Portal");
        foreach (GameObject portalObject in portalObjects)
        {
            portalObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.CurGold += Mathf.Abs(amount);
            Destroy(gameObject);

            //Finds tags of "Portal" and turns them on if we have enough gold
            foreach (GameObject portalObject in portalObjects)
                {
                    //Checks if the player has enough gold
                    if (inventory.CurGold >= goldThreshold)
                    {
                        //Displays message
                        inventory.GetComponent<PlayerHints>().AddHint("The Portal is Open!");
                        
                        portalObject.SetActive(true);
                    }
                }
        }
    }
}
