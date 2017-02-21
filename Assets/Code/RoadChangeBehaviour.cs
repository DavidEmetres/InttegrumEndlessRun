using UnityEngine;
using System.Collections;

public class RoadChangeBehaviour : MonoBehaviour {

	private BoxCollider activationCollider;
	private BoxCollider animationCollider;
	private bool activated;
	private bool animated;
	private int laneSelected;
	private Transform pivotSelected;
	private float anglesRotated;
	private Transform leftPivot;
	private Transform rightPivot;
	private GameObject endRoad;
	private Transform leftEndRoad;
	private Transform rightEndRoad;

	public float speed;

	private void Start () {
		leftPivot = transform.GetChild (1);
		rightPivot = transform.GetChild (2);
		endRoad = transform.GetChild (3).gameObject;
		leftEndRoad = transform.GetChild (4);
		rightEndRoad = transform.GetChild (5);

		activated = false;
		animated = false;
	}
	
	private void Update () {
		if (animated) {
			if (laneSelected == 0) {
				transform.RotateAround (pivotSelected.position, transform.up, speed * Time.deltaTime);

				if (transform.localEulerAngles.y >= 210f) {
					endRoad.transform.localPosition = leftEndRoad.localPosition;
					endRoad.transform.localRotation = leftEndRoad.localRotation;
				}

				if (transform.localEulerAngles.y >= 270f) {
					animated = false;
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 270f, transform.eulerAngles.z);
					transform.position = new Vector3 (transform.position.x + 0.5f, 0f, transform.position.z);
					GenerationManager.Instance.ChangeDisplacementSpeed (0f, true);
					GenerationManager.Instance.BuildTerrainMesh (120f);
					GenerationManager.Instance.changingRoad = false;
				}
			}

			if (laneSelected == 2) {
				transform.RotateAround (pivotSelected.position, transform.up, 4.5f * GenerationManager.Instance.displacementSpeed * Time.deltaTime);
				anglesRotated += 4.5f * GenerationManager.Instance.displacementSpeed * Time.deltaTime;

				if(anglesRotated >= 90f)
					animated = false;
			}
		}

//		transform.RotateAround (pivotSelected.position, transform.up, speed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if (!activated) {
				activated = true;

				//BLOCK PLAYER MOVEMENT;
				laneSelected = PlayerMovement.Instance.GetCurrentLane ();
				if (laneSelected == 0) {
					pivotSelected = leftPivot;
				}
				else if (laneSelected == 2) {
					pivotSelected = rightPivot;
				}

				animated = true;
				anglesRotated = 0f;
				GenerationManager.Instance.ChangeDisplacementSpeed (5f, false);
				GenerationManager.Instance.DestroyTerrainMesh ();
			}
		}
	}
}
