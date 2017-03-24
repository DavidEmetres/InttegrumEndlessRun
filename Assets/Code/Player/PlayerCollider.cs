using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour {

	private bool invincible;
	private float invincibleTimer;
	private PlayerAnimationManager animManager;

	public SkinnedMeshRenderer[] renderer;
	public float invincibleDuration;
	public float timeBetweenFlashes;
	public CapsuleCollider triggerCollider;

	private void Awake() {
		
	}

	private void Start() {
		invincible = false;
		animManager = GetComponent<PlayerAnimationManager> ();
	}

	private void Update() {
		if (invincible) {
			invincibleTimer += Time.deltaTime;

			if (invincibleTimer >= invincibleDuration) {
				invincible = false;
				CancelInvoke ();
				foreach(SkinnedMeshRenderer mr in renderer)
					mr.material.color = new Color (mr.material.color.r, mr.material.color.g, mr.material.color.b, 1f);
			}
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Obstacle_4") {
			SceneManager.Instance.life = 0;
			Vector3 pos = PlayerMovement.Instance.transform.position;
			PlayerMovement.Instance.transform.position = new Vector3 (pos.x, pos.y, pos.z - 0.5f);
			animManager.DyingObstacle4Animation ();
			SceneManager.Instance.GameOver ();
		}
		else if (other.tag == "Obstacle") {
			if (!invincible)
				GetHurt ();
		}
		else if (other.tag == "Coin") {
			GetCoin ();
			Destroy (other.gameObject);
		}
	}

	private void ChangeAlpha() {
		foreach (SkinnedMeshRenderer mr in renderer) {
			if (mr.material.color.a >= 1f)
				mr.material.color = new Color (mr.material.color.r, mr.material.color.g, mr.material.color.b, 0.5f);
			else
				mr.material.color = new Color (mr.material.color.r, mr.material.color.g, mr.material.color.b, 1f);
		}
	}

	private void GetHurt() {
		SceneManager.Instance.life--;
		if (SceneManager.Instance.life <= 0) {
			animManager.DyingAnimation ();
		}
		else
			animManager.GetHurtAnimation ();

		invincible = true;
		invincibleTimer = 0f;
		InvokeRepeating ("ChangeAlpha", 0f, timeBetweenFlashes);
	}

	private void GetCoin() {
		SceneManager.Instance.coins++;
	}

	public void RestoreTriggerCollider() {
		triggerCollider.direction = 1;
		triggerCollider.center = new Vector3 (0, 3.2f, -0.4f);
		triggerCollider.radius = 2;
	}
}
