using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot")]
public class Loot : ScriptableObject
{
    public GameObject LootPrefab;
    public string LootName;
    public int DropChance;

    public Loot(string lootName, int dropChance)
    {
        LootName = lootName;
        DropChance = dropChance;
    }
}
