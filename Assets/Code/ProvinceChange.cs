using UnityEngine;
using System.Collections;

public class ProvinceChange {

	public ProvinceChange(GameObject prefab, float distance) {
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (0f, 180f, 0f);

		Vector3 pos = new Vector3 (0f, 0.01f, distance + 10f);

		prefab.SetActive (true);
		prefab.transform.position = pos;
		prefab.transform.rotation = rot;
	}
}
