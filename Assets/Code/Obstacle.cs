using UnityEngine;
using System.Collections;

public class Obstacle {

	public GameObject model;

	public Obstacle(float generationDistance, Transform parent, float xPos, float yPos) {
		model = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/BasicObstacle"), new Vector3(xPos, yPos, generationDistance), Quaternion.identity) as GameObject; 
		model.transform.SetParent (parent);
		model.AddComponent<Displacement> ();
		model.GetComponent<Displacement> ().Initialize (this);
	}
}