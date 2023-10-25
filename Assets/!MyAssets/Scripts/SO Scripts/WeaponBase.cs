using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ScriptableObject
{
    [SerializeField] int damage;
    [SerializeField] int baseValue;
    [SerializeField] GameObject prefab;

    public int GetDamage { get { return damage; } }
    public int GetBaseValue { get { return baseValue; } }
    public GameObject Prefab { get { return prefab; } }
}
