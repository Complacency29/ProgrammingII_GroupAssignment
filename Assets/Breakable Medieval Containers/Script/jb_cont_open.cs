using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jb_cont_open : MonoBehaviour {
	void OnMouseDown () {
		var barrelAnimator = gameObject.GetComponent<Animator>();
		bool barrelState = barrelAnimator.GetBool("isOpen");
		if (!barrelState) {
			barrelAnimator.SetBool("isOpen", true);
		}
	}
}
