using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private Transform[] lanes;
	private CharacterController controller;
	private Rigidbody rigidbody;
	private int lane;
	private Vector2 touchInitialPosition;
	private bool swipping;
	private State currentState;
	private float jumpTime;
	private bool rayToFloorEnabled;
	private bool isInGround;

	public float minSwipeDistanceX;
	public float minSwipeDistanceY;
	public float lateralDashSpeed;
	public LayerMask layerMaskRayToFloor;
	public AnimationCurve jumpCurve;
	public float maxJumpDistance;
	public float jumpSpeed;
	public float fallingForce;

	private void Awake() {
		controller = GetComponent<CharacterController> ();
		rigidbody = GetComponent<Rigidbody> ();
	}

	private void Start () {
		swipping = false;
		lane = 1;
		rayToFloorEnabled = true;
	}
	
	private void Update () {
		CheckTactilInput ();
		CheckMouseInput ();

		Ray ray = new Ray (transform.position + new Vector3(0f, 1f, 0f), Vector3.down);
		RaycastHit hit;

		if (rayToFloorEnabled) {
			float groundPos = 0f;
			if (Physics.Raycast (ray, out hit, layerMaskRayToFloor)) {
				if (hit.collider.gameObject.name == "Floor") {
					groundPos = hit.point.y;
				}
				currentState = State.running;
				isInGround = true;
			}

			if (transform.position.y <= groundPos) {
				currentState = State.running;
				isInGround = true;
				transform.position = new Vector3 (transform.position.x, groundPos, transform.position.z);
			}
			else
				isInGround = false;
		}

		if (currentState == State.running) {
			rigidbody.useGravity = true;
			float temp = Mathf.Lerp (transform.position.x, lanes [lane].position.x, lateralDashSpeed * Time.deltaTime);
			transform.position = new Vector3 (temp, transform.position.y, transform.position.z);
		}
		else if (currentState == State.jumping) {
			if (jumpTime <= 1) {
				float t = jumpCurve.Evaluate (jumpTime);
				transform.position = new Vector3 (transform.position.x, t * maxJumpDistance, transform.position.z);
			}
			else {
				rayToFloorEnabled = true;
				rigidbody.useGravity = true;
				rigidbody.AddForce (Vector3.down * fallingForce);
			}
			jumpTime += jumpSpeed * Time.deltaTime;
		}
		Debug.Log (currentState);
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
		if (isInGround) {
			lane = (right) ? lane + 1 : lane - 1;
			if (lane > 2)
				lane = 2;
			else if (lane < 0)
				lane = 0;
		}
	}

	private void Jump() {
		if (isInGround) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);
			jumpTime = 0f;
			currentState = State.jumping;
			rayToFloorEnabled = false;
			rigidbody.useGravity = false;
		}
	}

	private void Roll() {

	}

	enum State {
		running,
		jumping,
		rolling
	}
}
