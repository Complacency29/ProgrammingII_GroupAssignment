using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ScriptableObject
{
    [SerializeField] string weaponName = "Enter weapon name.";
    [SerializeField, Range(1,100)] int damage;
    [SerializeField, Range(0,1000000)] int baseGoldValue;
    [SerializeField, Tooltip("This should be the mesh that will be placed in the characters hand. " +
        "Place the mesh in the hands of the player character and adjust its rotation, then save it as a prefab. " +
        "This will ensure it is in the correct position when it is enabled on the player.")] GameObject equippableObjectPrefab;
    public string WeaponName { get { return weaponName; } }
    public int GetDamage { get { return damage; } }
    public int GetBaseValue { get { return baseGoldValue; } }
    public GameObject Prefab { get { return equippableObjectPrefab; } }
}
