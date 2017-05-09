using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	private void Update() {
		transform.LookAt (Camera.main.transform.position, -Vector3.up);
		transform.eulerAngles = new Vector3 (0f, transform.eulerAngles.y, 0f);
	}
}
