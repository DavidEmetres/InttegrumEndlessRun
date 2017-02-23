using UnityEngine;
using System.Collections;

public class ProvinceChange {

	public ProvinceChange(float distance) {
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (0f, 180f, 0f);

		Vector3 pos = new Vector3 (0f, 0.01f, distance);

		MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Tunnel"), pos, rot);
	}
}
