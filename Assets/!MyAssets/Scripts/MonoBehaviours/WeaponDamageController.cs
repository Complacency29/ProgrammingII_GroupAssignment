using UnityEngine;
/// <summary>
/// This script will handle dealing damage to the target of the weapon
/// it will require a collider
/// when a collision is detected, it will look for an IDamageable interface
/// If the interface is found, we will damage the target.
/// </summary>
[RequireComponent(typeof(Collider))]
public class WeaponDamageController : MonoBehaviour
{
    int damageAmount;
    public int DamageAmount { get { return damageAmount; } set {  damageAmount = value; } }

    private void Awake()
    {
        //make sure the collider is a trigger
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null )

        {
            damageable.Damage(DamageAmount);
        }
    }
}
