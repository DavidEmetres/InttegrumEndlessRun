using UnityEngine;
using System.Collections;

public class ProvinceChange {

	public ProvinceChange(float distance) {
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (0f, 180f, 0f);

		Vector3 pos = new Vector3 (0f, 0.01f, distance);

		GameObject prefab = MyResources.Instance.GetTunnel ();
		MonoBehaviour.Instantiate (prefab, pos, rot);
	}
}
