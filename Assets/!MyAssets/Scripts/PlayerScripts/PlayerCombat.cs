using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float attackCooldown = .5f;
    Animator animator;

    bool holdingWeapon = false;
    bool canAttack = true;
    InputMaster controls;

    private void Awake()
    {
        controls = new InputMaster();
        controls.PlayerActions.Attack.performed += context => Attack();
    }

    private void Start()
    {
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

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.PlayerActions.Attack.performed -= context => Attack();
        controls.Disable();
    }
}
