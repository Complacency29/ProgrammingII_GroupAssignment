using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using ModuleSnapping;

public class PlayerInventory : MonoBehaviour
{
    [Header("Health")]
    //Current and Maximum health
    private int curHealth;
    [SerializeField] int maxHealth = 100;

    //Current and maximum health potions
    int curHealthPotions = 0;
    [SerializeField] int maxHealthPotions = 3;

    //Amount each health potion will heal
    [SerializeField] int healthPotionHealAmount = 20;

    [Header("Lamp")]
    //Current and Maximum lamp oil
    float curLampOil = 100f;
    [SerializeField] float maxLampOil = 100f;

    //Current and Maximum oil refills
    int curOilRefill = 0;
    [SerializeField] int maxOilRefill = 3;

    [SerializeField] int oilRefillAmount = 20;

    [Header("Other")]
    //Current gold amount
    [SerializeField] int currentGold = 0;

    //This will keep track of our currently equipped weapon
    [SerializeField] WeaponBase equippedWeapon;

    //This is the int that keeps track of our upgrades
    int upgradesBits = 0;
    
    public int CurHealth { get { return curHealth; } set { curHealth = value; } }
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int CurHealthPotions { get { return curHealthPotions; } set { curHealthPotions = value; } }
    public int MaxHealthPotions { get { return maxHealthPotions; } set { maxHealthPotions = value; } }
    public int HealthPotionHealAmount { get { return healthPotionHealAmount; } set { healthPotionHealAmount = value; } }
    public float CurLampOil { get { return curLampOil; } set { curLampOil = value; } }
    public float MaxLampOil { get { return maxLampOil; } set { maxLampOil = value; } }
    public int CurOilRefill { get { return curOilRefill; } set { curOilRefill = value; } }
    public int MaxOilRefill { get { return maxOilRefill; } set { maxOilRefill = value; } }
    public int OilRefillAmount { get { return oilRefillAmount; } set { oilRefillAmount = value; } }
    public int CurGold { get { return currentGold; } set { currentGold = value; } }
    public WeaponBase EquippedWeapon { get { return equippedWeapon; } set { equippedWeapon = value; } }
    public int UpgradesBits { get { return upgradesBits; } set { upgradesBits = value; } }


    private void Awake()
    {
        curHealth = MaxHealth;
        curLampOil = MaxLampOil;
    }

    public void UnlockUpgrade(Upgrades _upgradeToUnlock)
    {
        upgradesBits |= (int)_upgradeToUnlock; //this adds the correct bit to the bitwise operator.
    }
    public bool CheckForUpgrade(Upgrades _upgradeToCheck)
    {
        //0010                  //10
        if ((upgradesBits & (int)_upgradeToCheck) != 0)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        if(GetComponent<PhotonView>().IsMine == false)
        {
            return;
        }

        if(curHealth <= 0)
        {
            curHealth = maxHealth;
            curLampOil = MaxLampOil;

            transform.position = PlayerHandler.Instance.transform.position;

            //ExitRoom();
        }
    }

    /*public void ExitRoom()
    {
        StartCoroutine(ExitRoomAndLoad());
    }

    IEnumerator ExitRoomAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        
        while(PhotonNetwork.InRoom)
        {
            yield return null;
        }

        SceneManager.LoadScene("LobbyScene");
    }*/

    /// <summary>
    /// This enum is for use with a bitwise operator
    /// Upgrades can then be saved as an int
    /// </summary>
    public enum Upgrades
    {
        MaxHealthUpgrade1 = 1,
        MaxHealthUpgrade2 = 2,
        MaxHealthPotsUpgrade = 4,
        MaxOilRefillUpgrade = 8
    }
}
