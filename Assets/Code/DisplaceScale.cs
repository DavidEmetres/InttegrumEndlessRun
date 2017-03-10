using UnityEngine;
using System.Collections;

public class DisplaceScale : MonoBehaviour {

	private bool activated = true;

	public float destroyDistance;

	private void Update () {
		if (activated) {
			transform.localScale = new Vector3 (transform.localScale.x + 0.05f * Time.deltaTime, transform.localScale.y + 0.05f * Time.deltaTime, 1f);
			transform.position = new Vector3 (transform.position.x, (transform.GetComponent<BoxCollider>().size.y/2f * transform.localScale.y), 
				transform.position.z - (GenerationManager.Instance.displacementSpeed/10f * Time.deltaTime));
			transform.LookAt (PlayerMovement.Instance.gameObject.transform);
		}

		if (transform.position.z <= destroyDistance) {
			Destroy (this.gameObject);
		}
	}

	public void DesactivateDisplacement() {
		activated = false;
	}

	public void ActivateDisplacement() {
		activated = true;
	}
}
