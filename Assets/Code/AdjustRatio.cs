using UnityEngine;
using System.Collections;

public class AdjustRatio : MonoBehaviour {

	private void Awake() {
		Camera.main.aspect = 480f / 800f;
	}
}
