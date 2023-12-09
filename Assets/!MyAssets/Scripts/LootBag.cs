using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LootBag : MonoBehaviour
{
    public List<Loot> lootList = new List<Loot>();

    private Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();

        foreach (Loot loot in lootList)
        {
            if(randomNumber <= loot.DropChance)
            {
                possibleItems.Add(loot);
            }
        }

        if(possibleItems.Count > 0)
        {
            Loot dropedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return dropedItem;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem();
        if( droppedItem != null )
        {
            GameObject lootGameObject = droppedItem.LootPrefab;
             Instantiate(lootGameObject, spawnPosition, Quaternion.identity);
        }
    }
}
