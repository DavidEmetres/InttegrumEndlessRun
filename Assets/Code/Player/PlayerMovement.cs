using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Vector3[] lanes;
	private Vector3[] cameraLanes;
	private CharacterController controller;
	private Rigidbody rigidbody;
	private int lane;
	private Vector2 touchInitialPosition;
	private bool swipping;
	private State currentState;
	private float jumpTime;
	private bool rayToFloorEnabled;
	private bool isInGround;
	private CapsuleCollider triggerCollider;
	private float rollTimer;
	private float groundPos;
	private float startJumpHeight;
	private PlayerAnimationManager animManager;

	public float minSwipeDistanceX;
	public float minSwipeDistanceY;
	public float lateralDashSpeed;
	public LayerMask layerMaskRayToFloor;
	public AnimationCurve jumpCurve;
	public float maxJumpDistance;
	public float jumpSpeed;
	public float rollDuration;
	public bool bloquedMov;

	public static PlayerMovement Instance;

	private void Awake() {
		Instance = this;

		controller = GetComponent<CharacterController> ();
		rigidbody = GetComponent<Rigidbody> ();
		animManager = transform.GetChild (0).GetComponent<PlayerAnimationManager> ();

		CapsuleCollider[] col = transform.GetChild(0).GetComponents<CapsuleCollider> ();
		foreach (CapsuleCollider c in col) {
			if (c.isTrigger) {
				triggerCollider = c;
				break;
			}
		}
		transform.GetChild (0).GetComponent<PlayerCollider> ().triggerCollider = triggerCollider;
	}

	private void Start () {
		swipping = false;
		lane = 1;
		isInGround = true;
		lanes = SceneManager.Instance.lanes;
		cameraLanes = SceneManager.Instance.cameraLanes;
		groundPos = 0f;
		rayToFloorEnabled = true;
		bloquedMov = false;
	}
	
	private void Update () {
		if(!bloquedMov)
			CheckMouseInput ();
		
		Ray ray = new Ray (new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Vector3.down);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 10f, layerMaskRayToFloor)) {
			groundPos = hit.point.y;
		}

		if (rayToFloorEnabled) {
			if (transform.position.y <= (groundPos + 1.1f)) {
				animManager.EndFalling ();
				if (!isInGround)
					currentState = State.running;
				isInGround = true;
				transform.position = new Vector3 (transform.position.x, (groundPos + 1f), transform.position.z);
			}
			else {
				animManager.FallingAnimation ();
				isInGround = false;
			}
		}
			
		if (currentState == State.running) {
			float temp = Mathf.Lerp (transform.position.x, lanes [lane].x, lateralDashSpeed * Time.deltaTime);
			transform.position = new Vector3 (temp, transform.position.y, transform.position.z);
			float ctemp = Mathf.Lerp (Camera.main.transform.position.x, cameraLanes [lane].x, lateralDashSpeed * Time.deltaTime);
			Camera.main.transform.position = new Vector3 (ctemp, Camera.main.transform.position.y, Camera.main.transform.position.z);
		}
		else if (currentState == State.jumping) {
			if (jumpTime <= 1) {
				float t = jumpCurve.Evaluate (jumpTime);
				transform.position = new Vector3 (transform.position.x, startJumpHeight + 1f + (t * maxJumpDistance), transform.position.z);
			}
			else {
				rigidbody.useGravity = true;
				rayToFloorEnabled = true;
				animManager.EndJumpAnimation ();
			}

			jumpTime += jumpSpeed;
		}
		else if (currentState == State.rolling) {
			rollTimer += Time.deltaTime;

			if (rollTimer >= rollDuration) {
				currentState = State.running;
				animManager.EndRollAnimation();
			}
		}
		else if (currentState == State.changingLane) {
			if (isInGround) {
				float temp = Mathf.Lerp (transform.position.x, lanes [lane].x, lateralDashSpeed * Time.deltaTime);
				transform.position = new Vector3 (temp, transform.position.y, transform.position.z);
				float ctemp = Mathf.Lerp (Camera.main.transform.position.x, cameraLanes [lane].x, lateralDashSpeed * Time.deltaTime);
				Camera.main.transform.position = new Vector3 (ctemp, Camera.main.transform.position.y, Camera.main.transform.position.z);

				rollTimer += Time.deltaTime;

				if (rollTimer >= rollDuration) {
					currentState = State.running;
					animManager.EndRollAnimation();
				}
			}
			else {
				if (jumpTime <= 1) {
					float t = jumpCurve.Evaluate (jumpTime);
					transform.position = new Vector3 (transform.position.x, startJumpHeight + 1f + (t * maxJumpDistance), transform.position.z);
				}
				else {
					rigidbody.useGravity = true;
					rayToFloorEnabled = true;
					animManager.EndJumpAnimation ();
				}

				jumpTime += jumpSpeed;
			}
		}

		float tempCam = Mathf.Lerp(Camera.main.transform.position.y, (groundPos + 5.4f), 2f * Time.deltaTime);
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, tempCam, Camera.main.transform.position.z);
	}

	private void CheckTactilInput() {
		if (Input.touches.Length > 0) {
			Touch touch = Input.touches [0];

			if (touch.phase == TouchPhase.Began) {
				touchInitialPosition = touch.position;
			}
			else if (touch.phase == TouchPhase.Ended) {
				float swipeDistHorizontally = (new Vector3 (touch.position.x, 0, 0) -
				                              new Vector3 (touchInitialPosition.x, 0, 0)).magnitude;

				float swipeDistVertically = (new Vector3 (0, touch.position.y, 0) -
					new Vector3 (0, touchInitialPosition.y, 0)).magnitude;

				//CHECK SLIDE VERTICALLY;
				if (swipeDistVertically > minSwipeDistanceY) {
					if (touchInitialPosition.y < touch.position.y)
						Jump ();
					else
						Roll ();
				}

				//CHECK SLIDE HORIZONTALLY;
				else if (swipeDistHorizontally > minSwipeDistanceX) {
					bool swipeRight = touchInitialPosition.x < touch.position.x;
					ChangeLane (swipeRight);
				}
			}
		}
	}

	private void CheckMouseInput() {
		if (Input.GetMouseButtonDown (0))
			touchInitialPosition = Input.mousePosition;

		if (Input.GetMouseButtonUp (0)) {
			float swipeDistHorizontally = (new Vector3 (Input.mousePosition.x, 0, 0) -
				new Vector3 (touchInitialPosition.x, 0, 0)).magnitude;

			float swipeDistVertically = (new Vector3 (0, Input.mousePosition.y, 0) -
				new Vector3 (0, touchInitialPosition.y, 0)).magnitude;

			if (swipeDistHorizontally > minSwipeDistanceX) {
				bool swipeRight = touchInitialPosition.x < Input.mousePosition.x;
				ChangeLane (swipeRight);
			}

			else if (swipeDistVertically > minSwipeDistanceY) {
				if (touchInitialPosition.y < Input.mousePosition.y)
					Jump ();
				else
					Roll ();
			}
		}
	}

	public void ChangeState(State state) {
		currentState = state;
	}

	public State GetCurrentState() {
		return currentState;
	}

	public void ChangeLane(bool right) {
		if (!SceneManager.Instance.gameOver) {
			if (isInGround && (currentState != State.rolling) || currentState == State.changingLane) {
				lane = (right) ? lane + 1 : lane - 1;
				if (lane > 2)
					lane = 2;
				else if (lane < 0)
					lane = 0;
				else
					animManager.ChangeLaneAnimation (right);
			}
		}
	}

	private void Jump() {
		if (!SceneManager.Instance.gameOver) {
			if (isInGround && currentState != State.rolling) {
				jumpTime = 0f;
				startJumpHeight = transform.position.y - 1f;
				currentState = State.jumping;
				rigidbody.useGravity = false;
				rayToFloorEnabled = false;
				isInGround = false;
				animManager.JumpAnimation ();
			}
		}
	}

	private void Roll() {
		if (!SceneManager.Instance.gameOver) {
			if (isInGround && currentState != State.rolling) {
				rollTimer = 0f;
				currentState = State.rolling;
				triggerCollider.direction = 2;
				triggerCollider.center = new Vector3 (0, 1.2f, 0.7f);
				triggerCollider.radius = 1;
				animManager.RollAnimation ();
			}
		}
	}

	public int GetCurrentLane() {
		return lane;
	}
}

public enum State {
	running,
	jumping,
	rolling,
	changingLane,
	dying
}
