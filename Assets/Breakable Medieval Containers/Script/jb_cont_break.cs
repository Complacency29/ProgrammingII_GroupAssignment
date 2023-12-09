using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jb_cont_break : MonoBehaviour {
	public Transform Fragments;
	public float Spread, Force;


	void OnMouseDown () {
        GetComponent<LootBag>().InstantiateLoot(transform.position);

        Destroy(gameObject);
		Instantiate(Fragments, transform.position, transform.rotation);
		Fragments.localScale = transform.localScale;
		Vector3 breakPosition = transform.position;
		Collider[] fragments = Physics.OverlapSphere(breakPosition, Spread);

		foreach (Collider fragment in fragments) {
			if (fragment.attachedRigidbody) {
				fragment.attachedRigidbody.AddExplosionForce(Force, breakPosition, Spread);
			}
		}       
    }
}
