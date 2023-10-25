using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCombat : MonoBehaviour, IDamageable
{
    [SerializeField] float attackCooldown = .5f;
    Animator animator;
    PlayerInventory inventory;


    [SerializeField] bool holdingWeapon = false;
    bool canAttack = true;
    InputMaster controls;
    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine == false)
            return;

        controls = new InputMaster();
        controls.PlayerActions.Attack.performed += context => Attack();
        controls.PlayerActions.UseHealthPotion.performed += context => UseHealthPotion();

        inventory = GetComponent<PlayerInventory>();
        animator = GetComponent<PlayerMovement>().GetAnimator;
    }

    void UnsheathWeapon()
    {
        holdingWeapon = true;
        animator.SetBool("holdingWeapon", true);
    }

    void SheathWeapon()
    {
        holdingWeapon = false;
        animator.SetBool("holdingWeapon", false);
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
        controls.PlayerActions.Attack.performed -= context => Attack();
        controls.Disable();
    }
}
