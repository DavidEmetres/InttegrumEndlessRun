using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private Transform[] lanes;
	private CharacterController controller;
	private int lane;
	private int touchIndex;
	private Vector2 touchInitialPosition;
	private bool changingLane;
	private Vector3 speed;

	public float minTouchDistanceX;
	public float minTouchDistanceY;
	public float lateralDashSpeed;

	private void Awake() {
		controller = GetComponent<CharacterController> ();
	}

	private void Start () {
		touchIndex = -1;
		changingLane = false;
	}
	
	private void Update () {
		CheckTactilInput ();

		if (changingLane) {
			controller.SimpleMove (speed);
		}
	}

	private void CheckTactilInput() {
		foreach (Touch touch in Input.touches) {
			if (touchIndex >= 0) {
				if (touch.phase == TouchPhase.Began) {
					touchIndex = touch.fingerId;
					touchInitialPosition = touch.position;
				}
			} 

			else {
				if (touch.fingerId != touchIndex)
					continue;

				if (touch.phase == TouchPhase.Moved) {
					//CHECK SLIDE HORIZONTALLY;
					if (Mathf.Abs(touchInitialPosition.x - touch.position.x) >= minTouchDistanceX) {
						bool slideRight = (touchInitialPosition.x < touch.position.x) ? true : false;
						ChangeLane (slideRight);

						//DISABLED THE ACTUAL TOUCH;
						touchIndex = -1;
					}

					//CHECK SLIDE VERTICALLY;
					if (Mathf.Abs(touchInitialPosition.y - touch.position.y) >= minTouchDistanceY) {

					}
				}

				if (touch.phase == TouchPhase.Ended) {
					touchIndex = -1;
				}
			}

			Debug.Log (touch.position);
		}
	}

	private void ChangeLane(bool right) {
		int direction = (right) ? 1 : -1;
		speed = new Vector3 (lateralDashSpeed * Time.deltaTime * direction, 0f, 0f);
		changingLane = true;
	}
}
