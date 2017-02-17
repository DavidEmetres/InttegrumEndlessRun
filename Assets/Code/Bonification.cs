using UnityEngine;
using System.Collections;

public class Bonification {

	public GameObject model;

	public Bonification(float generationDistance, Transform parent, float xPos, float yPos) {
		model = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Bonification"), new Vector3(xPos, yPos, generationDistance), Quaternion.identity) as GameObject; 
		model.transform.SetParent (parent);
		model.AddComponent<Displacement> ();
	}
}
