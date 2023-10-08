using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int curHealth;
    [SerializeField] int maxHealth = 100;

    int curPotions = 0;
    [SerializeField] int maxPotions = 3;

    float curLampOil = 100f;
    [SerializeField] float maxLampOil = 100f;

    [SerializeField] int curGold = 0;
    
    public int CurHealth { get { return curHealth; } set { curHealth = value; } }
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int CurPotions { get { return curPotions; } set { curPotions = value; } }
    public int MaxPotions { get { return maxPotions; } set { maxPotions = value; } }
    public float CurLampOil { get { return curLampOil; } set { curLampOil = value; } }
    public float MaxLampOil { get { return maxLampOil; } set { maxLampOil = value; } }
    public int CurGold { get { return curGold; } set { curGold = value; } }


    private void Awake()
    {
        curHealth = MaxHealth;
    }
}
