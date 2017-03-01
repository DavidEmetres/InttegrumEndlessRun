using UnityEngine;
using System.Collections;

public class Displacement : MonoBehaviour {

	public float destroyDistance;
	public bool parentWithDisplacement;

	private void Update () {
		if (transform.parent != null) {
			if (transform.parent.parent != null) {
				if (transform.parent.parent.parent != null) {
					if (transform.parent.parent.parent.name == "RoadChange(Clone)")
						parentWithDisplacement = true;
					else
						parentWithDisplacement = false;
				}
				else
					parentWithDisplacement = false;
			}
			else
				parentWithDisplacement = false;
		}
		else
			parentWithDisplacement = false;

		if (!parentWithDisplacement) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 
				transform.position.z - (GenerationManager.Instance.displacementSpeed * Time.deltaTime));
		}

		if (transform.position.z <= destroyDistance)
			Destroy (this.gameObject);
	}
}
