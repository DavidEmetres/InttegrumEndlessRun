using UnityEngine;
using System.Collections;

public class Tile {

	public GameObject obj;

	public Tile(int type, int num, Vector3 pos, Quaternion rot, Transform parent, bool displaceActive) {
		GameObject prefab = null;
		Vector3 finalPos = pos;

		if (type == -1)
			prefab = MyResources.Instance.GetCoin ();
		else
			prefab = MyResources.Instance.GetObstacle (SceneManager.Instance.currentProvince.climate, type, num);

		if(prefab != null)
			finalPos += prefab.transform.position;
		
		obj = MonoBehaviour.Instantiate (prefab, finalPos, prefab.transform.rotation) as GameObject;
		obj.GetComponent<Displacement> ().enabled = displaceActive;
		obj.transform.SetParent (parent);
	}
}