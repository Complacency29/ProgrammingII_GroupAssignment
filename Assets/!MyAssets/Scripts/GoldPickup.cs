using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class GoldPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinSound;
    [SerializeField, Min(0)] int amount;
    [SerializeField] bool useRandomGoldAmount = false;
    [SerializeField, Range(1, 1000)] int randomAmountMin = 5;
    [SerializeField, Range(1, 1000)] int randomAmountMax = 100;
    [SerializeField, Range(1, 3)] float pickupRange = 1.5f;

    public int Amount { get { return amount; } }

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
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            AudioManager.instance.PlaySFX(coinSound);
            inventory.CurGold += Mathf.Abs(amount);
            Destroy(gameObject);
        }
    }
}
