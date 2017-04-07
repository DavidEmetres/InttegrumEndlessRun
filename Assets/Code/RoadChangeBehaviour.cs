using UnityEngine;
using System.Collections;

public class RoadChangeBehaviour : MonoBehaviour {

	private GameObject env;
	private RoadChange rc;
	private BoxCollider activationCollider;
	private BoxCollider animationCollider;
	private bool animated;
	private int laneSelected;
	private Transform pivotSelected;
	private GameObject endRoad;
	private Transform leftEndRoad;
	private Transform rightEndRoad;
	private bool justSelectedRoad;
	private float skyboxRot;
	private float currentYAngle;
	private bool completed;

	public float speed;
	public Transform centerPivot;
	public Transform leftPivot;
	public Transform rightPivot;

	public void Initialize(RoadChange rc) {
		this.rc = rc;
		env = GameObject.Find ("Environment");
		speed = -80f;
		leftPivot = transform.GetChild (0).GetChild (1).transform;
		rightPivot = transform.GetChild (0).GetChild (2).transform;
		centerPivot = transform.GetChild (0).GetChild (3).transform;
	}

	private void OnEnable () {
		animated = false;
	}

	private void Update () {
		if (animated && !completed) {
			if (laneSelected == 0) {
				if (justSelectedRoad) {
					env.transform.SetParent (transform);
					skyboxRot = RenderSettings.skybox.GetFloat ("_Rotation");
					justSelectedRoad = false;
				}

				skyboxRot += (speed * Time.deltaTime);
				RenderSettings.skybox.SetFloat ("_Rotation", skyboxRot);
				transform.RotateAround (pivotSelected.position, transform.up, speed * Time.deltaTime);

				//END ROAD CHANGE;
				if (transform.localEulerAngles.y >= 270f) {
					completed = true;
					animated = false;
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 270f, transform.eulerAngles.z);
					transform.position = new Vector3 (-26f, 0.01f, transform.position.z);
					GenerationManager.Instance.ChangeDisplacementSpeed (0f, true);
					GenerationManager.Instance.BuildTerrainMesh (GenerationManager.Instance.changingRoadStartPos.transform.position.z + 100f - 190f);
					GenerationManager.Instance.BuildEnviroMesh (GenerationManager.Instance.changingRoadStartPos.transform.position.z + 100f - 190f, false);
					GenerationManager.Instance.BuildEnviroMesh (GenerationManager.Instance.changingRoadStartPos.transform.position.z + 100f - 190f, true);
					GenerationManager.Instance.changingRoad = false;
					PlayerMovement.Instance.lateralDashSpeed *= 5f;
					PlayerMovement.Instance.bloquedMov = false;
					PlayerMovement.Instance.ChangeState (State.running);
					SceneManager.Instance.RoadChangeFinished ();
					env.transform.SetParent (null);
					Destroy (this);
				}
			}
			else if (laneSelected == 2) {
				if (justSelectedRoad) {
					env.transform.SetParent (transform);
					skyboxRot = RenderSettings.skybox.GetFloat ("_Rotation");
					justSelectedRoad = false;
				}

				skyboxRot += (-speed * Time.deltaTime);
				RenderSettings.skybox.SetFloat ("_Rotation", skyboxRot);
				transform.RotateAround (pivotSelected.position, transform.up, -speed * Time.deltaTime);

				//END ROAD CHANGE;
				if (transform.localEulerAngles.y <= 90f) {
					completed = true;
					animated = false;
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 90f, transform.eulerAngles.z);
					transform.position = new Vector3 (26f, 0.01f, transform.position.z);
					GenerationManager.Instance.ChangeDisplacementSpeed (0f, true);
					GenerationManager.Instance.BuildTerrainMesh (GenerationManager.Instance.changingRoadStartPos.transform.position.z + 100f - 190f);
					GenerationManager.Instance.BuildEnviroMesh (GenerationManager.Instance.changingRoadStartPos.transform.position.z + 100 - 190f, false);
					GenerationManager.Instance.BuildEnviroMesh (GenerationManager.Instance.changingRoadStartPos.transform.position.z + 100f - 190f, true);
					GenerationManager.Instance.changingRoad = false;
					PlayerMovement.Instance.lateralDashSpeed *= 5f;
					PlayerMovement.Instance.bloquedMov = false;
					PlayerMovement.Instance.ChangeState (State.running);
					SceneManager.Instance.RoadChangeFinished ();
					env.transform.SetParent (null);
					Destroy (this);
				}
			}
			else {
				if (transform.position.z <= -40f) {
					animated = false;
					PlayerMovement.Instance.bloquedMov = false;
					PlayerMovement.Instance.ChangeState (State.running);
					Destroy (this);
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
			if (!animated && !completed) {
				PlayerMovement.Instance.ChangeState (State.changingLane);
				PlayerMovement.Instance.bloquedMov = true;
				laneSelected = PlayerMovement.Instance.GetCurrentLane ();
				GenerationManager.Instance.laneSelected = laneSelected;
				Neighbours n = rc.GetNeighbour (laneSelected);
				SceneManager.Instance.ChooseNextNeighbour (n, rc.GetNewDirection(laneSelected));
				justSelectedRoad = true;
				HUDManager.Instance.ShowNextProvince (n.neighbourProvince.name);

				switch (laneSelected) {
					case 0:
						pivotSelected = leftPivot;
						animated = true;
						SceneManager.Instance.RoadChangeStarted ();
						GenerationManager.Instance.ChangeDisplacementSpeed (5f, false);
						GenerationManager.Instance.DestroyTerrainMesh ();
						PlayerMovement.Instance.lateralDashSpeed = PlayerMovement.Instance.lateralDashSpeed / 5f;
						PlayerMovement.Instance.ChangeLane (true);
						break;
					case 1:
						animated = true;
						GenerationManager.Instance.DestroyTerrainMesh ();
						GenerationManager.Instance.BuildTerrainMesh (GenerationManager.Instance.changingRoadStartPos.transform.position.z + 150f - 200f);
						GenerationManager.Instance.BuildEnviroMesh (GenerationManager.Instance.changingRoadStartPos.transform.position.z + 150f - 200f, false);
						GenerationManager.Instance.BuildEnviroMesh (GenerationManager.Instance.changingRoadStartPos.transform.position.z + 150f - 200f, true);
						GenerationManager.Instance.tileCount = -15;
						GenerationManager.Instance.changingRoad = false;
						GenerationManager.Instance.tileCount = 5;
						break;
					case 2:
						pivotSelected = rightPivot;
						animated = true;
						SceneManager.Instance.RoadChangeStarted ();
						GenerationManager.Instance.ChangeDisplacementSpeed (5f, false);
						GenerationManager.Instance.DestroyTerrainMesh ();
						PlayerMovement.Instance.lateralDashSpeed = PlayerMovement.Instance.lateralDashSpeed / 5f;
						PlayerMovement.Instance.ChangeLane (false);
						break;
				}
			}
		}
	}
}
