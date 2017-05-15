using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

	public Transform pivot;
	public float rotSpeed;
	public Vector3 offset;

	private void LateUpdate() {
		offset = Quaternion.AngleAxis (rotSpeed * Time.deltaTime, Vector3.up) * offset;
		transform.position = pivot.position + offset;
		transform.LookAt (pivot.position);
	}
}
