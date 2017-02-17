using UnityEngine;
using System.Collections;

public class Displacement : MonoBehaviour {
	
	private void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 
			transform.position.z - (SceneManager.Instance.displacementSpeed * Time.deltaTime));

//		if (transform.position.z <= GenerationManager.Instance.destroyDistance) {
//			Destroy (this.gameObject);
//		}
	}
}
