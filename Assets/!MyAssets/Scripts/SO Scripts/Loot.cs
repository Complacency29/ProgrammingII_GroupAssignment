using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot")]
public class Loot : ScriptableObject
{
    public GameObject LootPrefab;
    public int DropChance;

    public Loot(int dropChance)
    {
        DropChance = dropChance;
    }
}
