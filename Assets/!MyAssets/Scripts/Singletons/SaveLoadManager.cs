using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This manager will exist in scene on an empty game object
/// It can be called to save or load player data.
/// </summary>
public class SaveLoadManager : MonoBehaviorSingleton<SaveLoadManager>
{
    [SerializeField] PlayerInventory inventoryToSave;

    public void SavePlayerData(string _playerName = "player")
    {
        SaveLoad.SavePlayerData(inventoryToSave, _playerName + "Data");
    }
    public void LoadPlayerData(string _playerName)
    {
        PlayerSaveData loadedData = SaveLoad.LoadPlayerData(_playerName + "Data");
        inventoryToSave.CurHealth = loadedData.CurHealth;
        inventoryToSave.MaxHealth = loadedData.MaxHealth;
        inventoryToSave.CurGold = loadedData.CurGold;
    }
}
