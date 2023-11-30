using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour, IDamageable
{
    [SerializeField] int curTargetHealth = 100;


    public void Damage(int _amount)
    {
        Debug.Log(gameObject.name + " was hit for " +  _amount + ".\n" + curTargetHealth + " health remaining.");
        curTargetHealth -= Mathf.Abs(_amount);
        if(curTargetHealth <= 0 )
            Destroy(gameObject);
    }
}
