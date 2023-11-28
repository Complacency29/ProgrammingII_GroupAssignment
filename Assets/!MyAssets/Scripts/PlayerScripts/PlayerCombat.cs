using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCombat : MonoBehaviour, IDamageable
{
    [SerializeField] float attackCooldown = .5f;
    [SerializeField] Transform weaponSpawnPoint;
    Animator animator;
    PlayerInventory inventory;


    [SerializeField] bool holdingWeapon = false;
    Transform currentAttachedWeapon;
    bool canAttack = true;
    InputMaster controls;
    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine == false)
            return;

        controls = new InputMaster();
        controls.PlayerActions.ToggleWeapon.performed += context => ToggleSheathWeapon();
        controls.PlayerActions.Attack.performed += context => Attack();
        controls.PlayerActions.UseHealthPotion.performed += context => UseHealthPotion();

        inventory = GetComponent<PlayerInventory>();
        animator = GetComponent<PlayerMovement>().GetAnimator;
    }

    void ToggleSheathWeapon()
    {
        if (view.IsMine == false)
            return;

        //if we don't have a weapon we don't need to do anything.
        if (inventory.EquippedWeapon == null)
            return;

        //if we aren't holding the weapon
        if (!holdingWeapon)
            UnsheathWeapon();
        else
            SheathWeapon();
    }

    void UnsheathWeapon()
    {
        holdingWeapon = true;
        animator.SetBool("holdingWeapon", true);

        if(currentAttachedWeapon == null)
        {
            //we don't have an attached weapon
            //spawn the equipped weapon at the position and rotation of the WeaponSpawnPoint
            currentAttachedWeapon = Instantiate(inventory.EquippedWeapon.Prefab, weaponSpawnPoint.position, weaponSpawnPoint.rotation).transform;
            currentAttachedWeapon.transform.parent = weaponSpawnPoint;
        }
        else
        {
            //we do have an attached weapon

            if (currentAttachedWeapon == inventory.EquippedWeapon.Prefab)
            {
                //the weapon we currently have matches the one we want to unsheath
                currentAttachedWeapon.gameObject.SetActive(true);
            }
            else
            {
                //the weapon we currently have does NOT match the one we want to unsheath
                //Destroy the old one, instantiate the new one (can replace with object pooling, but probably not required w/ our low player count)
                Destroy(currentAttachedWeapon.gameObject);

                currentAttachedWeapon = Instantiate(inventory.EquippedWeapon.Prefab, weaponSpawnPoint).transform;
                currentAttachedWeapon.transform.localPosition = Vector3.zero;
                currentAttachedWeapon.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void SheathWeapon()
    {
        holdingWeapon = false;
        animator.SetBool("holdingWeapon", false);
        currentAttachedWeapon.gameObject.SetActive(false);
    }

    void Attack()
    {
        if (!canAttack || !holdingWeapon)
            return;

        canAttack = false;
        StartCoroutine(AttackCooldownCO());
        animator.Play("Attack", 2);
    }
    IEnumerator AttackCooldownCO()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    public void Damage(int _amount)
    {
        inventory.CurHealth -= Mathf.Abs(_amount);
        if(inventory.CurHealth <= 0)
        {
            //player is dead.
        }
    }

    private void UseHealthPotion()
    {
        if(inventory.CurHealthPotions > 0)
        {
            inventory.CurHealthPotions--;
            Heal(inventory.HealthPotionHealAmount);
        }
        else
        {
            Debug.Log("Player does not have any health potions");
        }
    }

    private void Heal(int _amount)
    {
        if (view.IsMine == false)
            return;

        view.RPC("HealRPC", RpcTarget.All, _amount);
    }
    private void HealRPC(int _amount)
    {
        //add the absolut value of the given amount to the players health
        inventory.CurHealth += Mathf.Abs(_amount);

        //if that puts us over our max health, set to max health
        if (inventory.CurHealth > inventory.MaxHealth)
        {
            inventory.CurHealth = inventory.MaxHealth;
        }
    }

    private void OnEnable()
    {
        if (view.IsMine == false)
            return;
        controls.Enable();
    }
    private void OnDisable()
    {
        if (view.IsMine == false)
            return;
        controls.PlayerActions.ToggleWeapon.performed -= context => ToggleSheathWeapon();
        controls.PlayerActions.Attack.performed -= context => Attack();
        controls.PlayerActions.UseHealthPotion.performed -= context => UseHealthPotion();
        controls.Disable();
    }
}
