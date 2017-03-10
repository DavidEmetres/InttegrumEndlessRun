using UnityEngine;
using System.Collections;

public class Tile {

	public GameObject obj;

	public Tile(string path, Vector3 pos, Quaternion rot, Transform parent) {
		GameObject prefab = (GameObject)Resources.Load (path);
		Vector3 finalPos = pos + prefab.transform.position;
		obj = MonoBehaviour.Instantiate (Resources.Load(path), finalPos, prefab.transform.rotation) as GameObject;
		obj.transform.SetParent (parent);
	}
}
