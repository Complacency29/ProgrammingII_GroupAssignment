using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;

public class jb_cont_break : MonoBehaviour, IDamageable
{
	public Transform Fragments;
	public float Spread, Force;

    //PhotonView view;

    /*
    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    */
    public void Damage(int _amount)
    {
        //break the object
        //BreakObject();
        //view.RPC("BreakObjectRPC", RpcTarget.All);
    }

    //[PunRPC]
    void BreakObjectRPC()
    {
        if(GetComponent<LootBag>() != null)
        {
            GetComponent<LootBag>().InstantiateLoot(transform.position);
        }

        Instantiate(Fragments, transform.position, transform.rotation);
        Fragments.localScale = transform.localScale;
        Vector3 breakPosition = transform.position;
        Collider[] fragments = Physics.OverlapSphere(breakPosition, Spread);

        foreach (Collider fragment in fragments)
        {
            if (fragment.attachedRigidbody)
            {
                fragment.attachedRigidbody.AddExplosionForce(Force, breakPosition, Spread);
            }
        }
        Destroy(gameObject);
    }
}
