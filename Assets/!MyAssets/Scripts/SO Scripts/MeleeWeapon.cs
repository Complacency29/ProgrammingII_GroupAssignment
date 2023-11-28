using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Melee Weapon")]
public class MeleeWeapon : WeaponBase
{
    [SerializeField, Range(.1f, 4f)] float swingSpeed;
}
