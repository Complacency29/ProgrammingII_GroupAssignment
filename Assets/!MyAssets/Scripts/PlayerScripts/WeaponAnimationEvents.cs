using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will store the animation events to be used by animator.
/// This will include disabling and enabling the colliders on an equipped weapon.
/// To do this, the colliders must be designated here and toggled manually.
/// This script will NOT handle detecting the hit with the enemy. That will be done with a script on the weapon itself.
/// </summary>
public class WeaponAnimationEvents : MonoBehaviour
{
    Collider currentWeaponCollider;
    public Collider CurrentWeaponCollider { get { return currentWeaponCollider; } set { currentWeaponCollider = value; } }


    //this will be called via an animation event when the weapon is swung
    public void EnableWeaponColliders()
    {
        currentWeaponCollider.enabled = true;
    }
    //this will be called via an animation event near the end of the weapon swing
    public void DisableWeaponColliders()
    {
        currentWeaponCollider.enabled = false;
    }
}
