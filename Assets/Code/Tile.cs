using UnityEngine;
using System.Collections;

public class Tile {

	public Tile(string path, Vector3 pos, Quaternion rot) {
		MonoBehaviour.Instantiate (Resources.Load(path), pos, rot);
	}
}
