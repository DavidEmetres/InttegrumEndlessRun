using UnityEngine;
using System.Collections;

public class DestroyObjectsColliding : MonoBehaviour {

	public bool destroyObstacles;
	public bool destroyEnviro;

	private void OnTriggerStay(Collider other)  {
		if (destroyObstacles) {
			if (other.tag == "Obstacle" || other.tag == "Obstacle_4") {
				Destroy (other.gameObject);
			}
		}

		if (destroyEnviro) {
			if (other.tag == "Enviro") {
				Destroy (other.gameObject);
			}
		}
	}
}
