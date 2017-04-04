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
		JumpAnimation (-1);
	}

	public void JumpAnimation(int jAnim) {
		if (jAnim <= 0)
			jAnim = Random.Range (1, 4);

		anim.SetInteger ("jumpAnim", jAnim);
		anim.SetTrigger ("jumping");
		anim.SetBool ("isJumping", true);
	}

	public void EndJumpAnimation() {
		anim.SetBool ("isFalling", true);
		anim.SetBool ("isJumping", false);
	}

	public void EndFalling() {
		anim.SetBool ("isFalling", false);
	}

	public void RollAnimation() {
		anim.SetInteger ("rollAnim", Random.Range (1, 3));
		anim.SetTrigger ("rolling");
		anim.SetBool ("isRolling", true);
	}

	public void EndRollAnimation() {
		anim.SetBool ("isRolling", false);
	}

	public void DyingAnimation() {
		anim.SetTrigger ("dying");
	}

	public void DyingObstacle4Animation() {
		anim.SetTrigger ("dyingObstacle4");
	}

	public void GetHurtAnimation() {
		anim.SetTrigger ("getHurt");
	}

	public void FallingAnimation() {
		anim.SetBool ("isFalling", true);
	}

	public void SendGameOver() {
		SceneManager.Instance.GameOver ();
	}
}
