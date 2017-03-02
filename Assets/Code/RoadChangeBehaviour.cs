using UnityEngine;
using System.Collections;

public class RoadChangeBehaviour : MonoBehaviour {

	private RoadChange rc;
	private BoxCollider activationCollider;
	private BoxCollider animationCollider;
	private bool activated;
	private bool animated;
	private int laneSelected;
	private Transform pivotSelected;
	private Transform leftPivot;
	private Transform rightPivot;
	private GameObject endRoad;
	private Transform leftEndRoad;
	private Transform rightEndRoad;

	public float speed;
	public Transform centerPivot;
	public GameObject leftRoadObs;
	public GameObject rightRoadObs;

	public void Initialize(RoadChange rc) {
		this.rc = rc;
	}

	private void Start () {
		leftPivot = transform.GetChild (1);
		rightPivot = transform.GetChild (2);

		activated = false;
		animated = false;
	}
	
	private void Update () {
		if (animated) {
			GameObject env = GameObject.Find ("Environment");

			if (laneSelected == 0) {
				BroadcastMessage ("DesactivateDisplacement");
				env.transform.SetParent (transform);
				transform.RotateAround (pivotSelected.position, transform.up, speed * Time.deltaTime);

				//END ROAD CHANGE;
				if (transform.localEulerAngles.y >= 270f) {
					animated = false;
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 270f, transform.eulerAngles.z);
					transform.position = new Vector3 (-26f, 0f, transform.position.z);
					GenerationManager.Instance.ChangeDisplacementSpeed (0f, true);
					GenerationManager.Instance.BuildTerrainMesh (GetEndRoadPos ());
					GenerationManager.Instance.changingRoad = false;
					GenerationManager.Instance.ChangeObsBonParent (transform, transform, true);
					PlayerMovement.Instance.lateralDashSpeed *= 5f;
					PlayerMovement.Instance.bloquedMov = false;
					PlayerMovement.Instance.ChangeState (State.running);
					BroadcastMessage ("ActivateDisplacement");
					env.transform.SetParent (null);
				}
			}
			else if (laneSelected == 2) {
				BroadcastMessage ("DesactivateDisplacement");
				env.transform.SetParent (transform);
				transform.RotateAround (pivotSelected.position, transform.up, -speed * Time.deltaTime);

				//END ROAD CHANGE;
				if (transform.localEulerAngles.y <= 90f) {
					animated = false;
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 90f, transform.eulerAngles.z);
					transform.position = new Vector3 (26f, 0f, transform.position.z);
					GenerationManager.Instance.ChangeDisplacementSpeed (0f, true);
					GenerationManager.Instance.BuildTerrainMesh (GetEndRoadPos ());
					GenerationManager.Instance.changingRoad = false;
					GenerationManager.Instance.ChangeObsBonParent (transform, transform, true);
					PlayerMovement.Instance.lateralDashSpeed *= 5f;
					PlayerMovement.Instance.bloquedMov = false;
					PlayerMovement.Instance.ChangeState (State.running);
					BroadcastMessage ("ActivateDisplacement");
					env.transform.SetParent (null);
				}
			}
			else {
				if (transform.position.z <= -40f) {
					animated = false;
					PlayerMovement.Instance.bloquedMov = false;
					PlayerMovement.Instance.ChangeState (State.running);
				}
			}
		}
	}

	private float GetEndRoadPos() {
		float distance = 0f;

		switch (laneSelected) {
			case 0:
				distance = transform.GetChild (3).position.z + 10f;
				break;
			case 1:
				distance = transform.GetChild (5).position.z + 10f;
				break;
			case 2:
				distance = transform.GetChild (4).position.z + 10f;
				break;
		}

		return distance;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if (!animated) {
				PlayerMovement.Instance.ChangeState (State.changingLane);
				PlayerMovement.Instance.bloquedMov = true;
				laneSelected = PlayerMovement.Instance.GetCurrentLane ();
				SceneManager.Instance.ChooseNextNeighbour (rc.GetNeighbour (laneSelected), rc.GetNewDirection(laneSelected));

				switch (laneSelected) {
				case 0:
					pivotSelected = leftPivot;
					animated = true;
					GenerationManager.Instance.ChangeDisplacementSpeed (5f, false);
					GenerationManager.Instance.DestroyTerrainMesh ();
					PlayerMovement.Instance.lateralDashSpeed = PlayerMovement.Instance.lateralDashSpeed / 5f;
					PlayerMovement.Instance.ChangeLane (true);
					GenerationManager.Instance.tileCount = 4;
					break;
				case 1:
					animated = true;
					GenerationManager.Instance.DestroyTerrainMesh ();
					GenerationManager.Instance.BuildTerrainMesh (GetEndRoadPos ());
					GenerationManager.Instance.tileCount = 1;
					GenerationManager.Instance.changingRoad = false;
					break;
				case 2:
					pivotSelected = rightPivot;
					animated = true;
					GenerationManager.Instance.ChangeDisplacementSpeed (5f, false);
					GenerationManager.Instance.DestroyTerrainMesh ();
					PlayerMovement.Instance.lateralDashSpeed = PlayerMovement.Instance.lateralDashSpeed / 5f;
					PlayerMovement.Instance.ChangeLane (false);
					GenerationManager.Instance.tileCount = 4;
					break;
				}
			}
		}
	}
}
