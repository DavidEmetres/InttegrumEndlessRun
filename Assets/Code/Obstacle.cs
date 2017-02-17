using UnityEngine;
using System.Collections;

public class Obstacle {

	public GameObject model;

	public Obstacle(string prefabPath, float generationDistance, Transform parent, float xPos, float yPos) {
		model = UnityEngine.Object.Instantiate(Resources.Load(prefabPath), new Vector3(xPos, yPos, generationDistance), Quaternion.identity) as GameObject; 
		model.transform.SetParent (parent);
		model.AddComponent<Displacement> ();
	}
}