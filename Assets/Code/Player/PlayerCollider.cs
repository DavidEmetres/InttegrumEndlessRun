using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour {

	private bool invincible;
	private float invincibleTimer;
	private PlayerAnimationManager animManager;
	private GameObject model;
	private MultiAudioPlayer audio;

	public float invincibleDuration;
	public float timeBetweenFlashes;
	[HideInInspector] public CapsuleCollider triggerCollider;

	private void Start() {
		invincible = false;
		animManager = GetComponent<PlayerAnimationManager> ();
		model = transform.GetChild (0).gameObject;
		audio = transform.parent.GetComponent<MultiAudioPlayer> ();
	}

	private void Update() {
		if (invincible) {
			invincibleTimer += Time.deltaTime;

			if (invincibleTimer >= invincibleDuration) {
				invincible = false;
				CancelInvoke ();
				if (!model.activeInHierarchy)
					model.SetActive (true);
			}
		}
	}
	
	private void InvincibleBlink() {
		if (model.activeInHierarchy)
			model.SetActive (false);
		else
			model.SetActive (true);
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Obstacle_4") {
			SceneManager.Instance.life = 0;
			Vector3 pos = PlayerMovement.Instance.transform.position;
			PlayerMovement.Instance.transform.position = new Vector3 (pos.x, pos.y, pos.z - 0.5f);
			animManager.DyingObstacle4Animation ();
			audio.PlayFX (4, 3);
			SceneManager.Instance.GameOver ();
		}
		else if (other.tag == "Obstacle") {
			if (!invincible)
				GetHurt ();
		}
		else if (other.tag == "Coin") {
			GetCoin ();
			other.gameObject.SetActive (false);
		}
	}

	private void GetHurt() {
		SceneManager.Instance.life--;
		HUDManager.Instance.LooseLife ();
		if (SceneManager.Instance.life <= 0) {
			animManager.DyingAnimation ();
			audio.PlayFX (5, 3);
		}
		else {
			animManager.GetHurtAnimation ();
			audio.PlayFX (3, 3);
		}

		invincible = true;
		invincibleTimer = 0f;
		InvokeRepeating ("InvincibleBlink", 0f, timeBetweenFlashes);
	}

	private void GetCoin() {
		SceneManager.Instance.coins++;
		audio.PlayFX (6, 4);
	}

	public void RestoreTriggerCollider() {
		triggerCollider.direction = 1;
		triggerCollider.center = new Vector3 (0, 3.2f, -0.4f);
		triggerCollider.radius = 2;
	}
}
