using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad
{    
    //call this method to save all the data in the player inventory
    public static void SavePlayerData(PlayerInventory _playerDataToSave, string _fileName)
    {
        //Create a new binary formatter, set the file path, and create a new file stream
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + _fileName + ".dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        //create a token for the player save data that we are going to save
        PlayerSaveData psd = new PlayerSaveData(_playerDataToSave);

        //Serialize the file using the binary formatter, and close the stream
        bf.Serialize(stream, psd);
        stream.Close();
    }

    /*
    //This method is for saving the players settings
    public static void SaveSettings(string _fileName, PlayerSettingsManager _psm)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + _fileName + ".phs";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerSettingsData setDat = new PlayerSettingsData(_psm);

        bf.Serialize(stream, setDat);
        stream.Close();
    }
    */

    //call this method to load all the information in the player inventory. This returns the save data
    public static PlayerSaveData LoadPlayerData(string _fileName)
    {
        //Debug.Log("Attempt load.");
        string path = Application.persistentDataPath + "/" + _fileName + ".dat";

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSaveData psd = bf.Deserialize(stream) as PlayerSaveData;

            stream.Close();

            return psd;
        }
        return null;
    }
    public static void DeleteSaveFile(string _fileName, int _saveSlot)
    {
        string path = Application.persistentDataPath + "/" + _fileName + "_" + _saveSlot + ".pha";

        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /*
    public static PlayerSettingsData LoadSettings(string _fileName)
    {
        //Debug.Log("Attempt settings load.");
        string path = Application.persistentDataPath + "/" + _fileName + ".phs";
        
        if(File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSettingsData setDat = bf.Deserialize(stream) as PlayerSettingsData;

            stream.Close();

            return setDat;
        }
        return null;
    }
    */
}

/*
[System.Serializable]
public class PlayerSettingsData
{
    private float _masterVolume;
    private float _musicVolume;
    private float _sfxVolume;

    public float MasterVolume { get { return _masterVolume; } }
    public float MusicVolume { get { return _musicVolume; } }
    public float SFXVolume { get { return _sfxVolume; } }
    public PlayerSettingsData(PlayerSettingsManager _psm)
    {
        _masterVolume = _psm.MasterVolume;
        _musicVolume = _psm.MusicVolume;
        _sfxVolume = _psm.SfxVolume;
    }
}
*/


/// <summary>
/// This class will contain all data we want to save on the player
/// </summary>
[System.Serializable]
public class PlayerSaveData
{
    private int curHealth;
    private int maxHealth;
    private int curHealthPots;
    private int maxHealthPots;
    private int healthPotHealAmount;
    private float curLampOil;
    private float maxLampOil;
    private int curOilRefills;
    private int maxOilRefills;
    private int oilRefillAmount;
    private int curGold;
    private int upgradesBits;

    public int CurHealth { get { return curHealth; } }
    public int MaxHealth { get { return maxHealth; } }
    public int CurHealthPots { get { return curHealthPots; } }
    public int MaxHealthPots { get { return maxHealthPots; } }
    public int HealthPotHealAmount { get { return healthPotHealAmount; } }
    public float CurLampOil { get { return curLampOil; } }
    public float MaxLampOil { get { return maxLampOil; } }
    public int CurOilRefills { get { return curOilRefills; } }
    public int MaxOilRefills { get { return maxOilRefills; } }
    public int OilRefillAmount { get { return oilRefillAmount; } }
    public int CurGold { get { return curGold; } }
    public int UpgradesBits { get { return upgradesBits; } }

    public PlayerSaveData(PlayerInventory _inventoryToSave)
    {

        curHealth = _inventoryToSave.CurHealth;
        maxHealth = _inventoryToSave.MaxHealth;
        curHealthPots = _inventoryToSave.CurHealthPotions;
        maxHealthPots = _inventoryToSave.MaxHealthPotions;
        healthPotHealAmount = _inventoryToSave.HealthPotionHealAmount;
        curLampOil = _inventoryToSave.CurLampOil;
        maxLampOil = _inventoryToSave.MaxLampOil;
        curOilRefills = _inventoryToSave.CurOilRefill;
        maxOilRefills = _inventoryToSave.MaxOilRefill;
        oilRefillAmount = _inventoryToSave.OilRefillAmount;
        curGold = _inventoryToSave.CurGold;
        upgradesBits = _inventoryToSave.UpgradesBits;
    }
}