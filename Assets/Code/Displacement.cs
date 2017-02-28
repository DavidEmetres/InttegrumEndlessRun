using UnityEngine;
using System.Collections;

public class Displacement : MonoBehaviour {

	public float destroyDistance;

	private void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 
			transform.position.z - (GenerationManager.Instance.displacementSpeed * Time.deltaTime));

		if (transform.position.z <= destroyDistance)
			Destroy (this.gameObject);


	}
}
