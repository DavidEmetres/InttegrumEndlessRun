using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private Transform[] lanes;
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

	public float minSwipeDistanceX;
	public float minSwipeDistanceY;
	public float lateralDashSpeed;
	public LayerMask layerMaskRayToFloor;
	public AnimationCurve jumpCurve;
	public float maxJumpDistance;
	public float jumpSpeed;
	public float rollDuration;

	private void Awake() {
		controller = GetComponent<CharacterController> ();
		rigidbody = GetComponent<Rigidbody> ();

		CapsuleCollider[] col = transform.GetChild(0).GetComponents<CapsuleCollider> ();
		foreach (CapsuleCollider c in col) {
			if (c.isTrigger) {
				triggerCollider = c;
				break;
			}
		}
	}

	private void Start () {
		swipping = false;
		lane = 1;
		isInGround = true;
		lanes = SceneManager.Instance.lanes;
		groundPos = 0f;
		rayToFloorEnabled = true;
	}
	
	private void Update () {
//		CheckTactilInput ();
		CheckMouseInput ();

		Ray ray = new Ray (new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Vector3.down);
		RaycastHit hit;
		Debug.DrawRay (ray.origin, ray.direction * 100f, Color.red);
		if (Physics.Raycast (ray, out hit, 10f, layerMaskRayToFloor)) {
			groundPos = hit.point.y;
		}
		Debug.Log (groundPos);
		if (rayToFloorEnabled) {
			if (transform.position.y <= (groundPos + 1.1f)) {
				if(!isInGround)
					currentState = State.running;
				isInGround = true;
				transform.position = new Vector3 (transform.position.x, (groundPos + 1f), transform.position.z);
			}
			else
				isInGround = false;
		}

		if (currentState == State.running) {
			float temp = Mathf.Lerp (transform.position.x, lanes [lane].position.x, lateralDashSpeed * Time.deltaTime);
			transform.position = new Vector3 (temp, transform.position.y, transform.position.z);
		}
		else if (currentState == State.jumping) {
//			if (jumpTime <= 2) {
//				float t = jumpCurve.Evaluate (jumpTime);
//				transform.position = new Vector3 (transform.position.x, startJumpHeight + 1f + (t * maxJumpDistance), transform.position.z);
//
//				if (jumpTime >= 1) {
//					rigidbody.useGravity = true;
//					rayToFloorEnabled = true;
//				}
//			}
//
//			jumpTime += jumpSpeed;

			if (jumpTime <= 1) {
				float t = jumpCurve.Evaluate (jumpTime);
				transform.position = new Vector3 (transform.position.x, startJumpHeight + 1f + (t * maxJumpDistance), transform.position.z);
			}
			else {
				rigidbody.useGravity = true;
				rayToFloorEnabled = true;
			}

			jumpTime += jumpSpeed;
		}
		else if (currentState == State.rolling) {
			rollTimer += Time.deltaTime;

			if (rollTimer >= rollDuration) {
				triggerCollider.center = Vector3.zero;
				currentState = State.running;

				//PROVISIONAL VISUAL UPDATE;
				transform.localScale = new Vector3 (1f, 1f, 1f);
			}
		}

//		Debug.Log (isInGround);
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

				//CHECK SLIDE HORIZONTALLY;
				if (swipeDistHorizontally > minSwipeDistanceX) {
					bool swipeRight = touchInitialPosition.x < touch.position.x;
					ChangeLane (swipeRight);
				}

				//CHECK SLIDE VERTICALLY;
				else if (swipeDistVertically > minSwipeDistanceY) {
					if (touchInitialPosition.y < touch.position.y)
						Jump ();
					else
						Roll ();
				}
			}

//			else if (touch.phase == TouchPhase.Moved && !swipping) {
//				swipping = true;
//				float swipeDistHorizontally = (new Vector3 (touch.position.x, 0, 0) -
//				                              new Vector3 (touchInitialPosition.x, 0, 0)).magnitude;
//
//				float swipeDistVertically = (new Vector3 (0, touch.position.y, 0) -
//					new Vector3 (0, touchInitialPosition.y, 0)).magnitude;
//
//				//CHECK SLIDE HORIZONTALLY;
//				if (swipeDistHorizontally > minSwipeDistanceX) {
//					bool swipeRight = touchInitialPosition.x < touch.position.x;
//					ChangeLane (swipeRight);
//				}
//
//				//CHECK SLIDE VERTICALLY;
//				else if (swipeDistVertically > minSwipeDistanceY) {
//					if (touchInitialPosition.y < touch.position.y)
//						Jump ();
//					else
//						Roll ();
//				}
//			}
//			else if (touch.phase == TouchPhase.Ended) {
//				swipping = false;
//			}
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

	private void ChangeLane(bool right) {
		if (isInGround && (currentState != State.rolling)) {
			lane = (right) ? lane + 1 : lane - 1;
			if (lane > 2)
				lane = 2;
			else if (lane < 0)
				lane = 0;
		}
	}

	private void Jump() {
		if (isInGround) {
			jumpTime = 0f;
			startJumpHeight = transform.position.y - 1f;
			currentState = State.jumping;
			rigidbody.useGravity = false;
			rayToFloorEnabled = false;
			isInGround = false;
		}
	}

	private void Roll() {
		if (isInGround) {
			triggerCollider.center = new Vector3 (0, -1, 0);
			rollTimer = 0f;
			currentState = State.rolling;

			//PROVISIONAL VISUAL UPDATE;
			transform.localScale = new Vector3 (1f, 0.5f, 1f);
		}
	}

	enum State {
		running,
		jumping,
		rolling,
		dying
	}
}
