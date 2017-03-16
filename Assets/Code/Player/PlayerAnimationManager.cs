using UnityEngine;
using System.Collections;

public class PlayerAnimationManager : MonoBehaviour {

	private Animator anim;

	private void Start() {
		anim = GetComponent<Animator> ();
	}

	public void ChangeLaneAnimation(bool right) {
		anim.SetTrigger ("changing");
		anim.SetBool ("changeRight", right);
	}

	public void JumpAnimation() {
		anim.SetInteger ("jumpAnim", Random.Range (1, 4));
		anim.SetTrigger ("jumping");
	}
}
