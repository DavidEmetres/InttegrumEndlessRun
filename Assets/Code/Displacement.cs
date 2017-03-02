using UnityEngine;
using System.Collections;

public class Displacement : MonoBehaviour {

	private bool activated = true;

	public float destroyDistance;

	private void Update () {
		if (activated) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 
				transform.position.z - (GenerationManager.Instance.displacementSpeed * Time.deltaTime));
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
