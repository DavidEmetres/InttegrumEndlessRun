using UnityEngine;
using System.Collections;

public class Displacement : MonoBehaviour {

	private Obstacle obs;

	public void Initialize(Obstacle obs) {
		this.obs = obs;
	}
	
	void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 
			transform.position.z - (SceneManager.Instance.obstacleSpeed * Time.deltaTime));

		if (transform.position.z <= SceneManager.Instance.destroyDistance) {
			SceneManager.Instance.DestroyObstacle (obs);
		}
	}
}
