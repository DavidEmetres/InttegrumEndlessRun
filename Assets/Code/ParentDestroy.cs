using UnityEngine;
using System.Collections;

public class ParentDestroy : MonoBehaviour {
	
	void Update () {
		if (transform.childCount == 0)
			Destroy (this.gameObject);
	}
}
