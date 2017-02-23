using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour {

	private MeshRenderer renderer;
	private bool invincible;
	private float invincibleTimer;

	public float invincibleDuration;
	public float timeBetweenFlashes;

	private void Awake() {
		renderer = GetComponent<MeshRenderer> ();
	}

	private void Start() {
		invincible = false;
	}

	private void Update() {
		if (invincible) {
			invincibleTimer += Time.deltaTime;

			if (invincibleTimer >= invincibleDuration) {
				invincible = false;
				CancelInvoke ();
				renderer.material.color = new Color (renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f);
			}
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Obstacle") {
			if (!invincible)
				GetHurt ();
		}
		else if (other.tag == "Coin") {
			GetCoin ();
			Destroy (other.gameObject);
		}
	}

	private void ChangeAlpha() {
		if (renderer.material.color.a >= 1f)
			renderer.material.color = new Color (renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.5f);
		else
			renderer.material.color = new Color (renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f);
	}

	private void GetHurt() {
		SceneManager.Instance.life--;
		if (SceneManager.Instance.life <= 0) {
			SceneManager.Instance.GameOver ();
		}

		invincible = true;
		invincibleTimer = 0f;
		InvokeRepeating ("ChangeAlpha", 0f, timeBetweenFlashes);
	}

	private void GetCoin() {
		SceneManager.Instance.coins++;
	}
}
