using UnityEngine;
using System.Collections;

public class ChangeAnimation : MonoBehaviour {

	private bool changing = false;
	private Transform currentPivot;
	private float rotDirection;

	public Transform[] pivots;

	void Start () {
		
	}
	
	void Update () {
		if (changing) {
			transform.RotateAround (currentPivot.position, transform.up, rotDirection * GenerationManager.Instance.displacementSpeed * Time.deltaTime);

			if (rotDirection == 1) {
				if (transform.eulerAngles.y >= 90f) {
					changing = false;
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 90f, transform.eulerAngles.z);
				}
			}
			else {
				if (transform.eulerAngles.y <= -90f) {
					changing = false;
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, -90f, transform.eulerAngles.z);
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha1))
			SelectRoad (0);

		if (Input.GetKeyDown (KeyCode.Alpha3))
			SelectRoad (2);
	}

	public void SelectRoad(int lane) {
		currentPivot = pivots [lane];
		changing = true;

		rotDirection = (lane > 1) ? -1f : 1f;
	}
}
