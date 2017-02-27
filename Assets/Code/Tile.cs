using UnityEngine;
using System.Collections;

public class Tile {

	public Tile(string path, Vector3 pos, Quaternion rot, Transform parent) {
		GameObject obj = MonoBehaviour.Instantiate (Resources.Load(path), pos, rot) as GameObject;
		obj.transform.SetParent (parent);
	}
}
