using UnityEngine;
using System.Collections;

public class ProvinceChange {

	public ProvinceChange(GameObject prefab) {
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (0f, 180f, 0f);

		GameObject enviro = GenerationManager.Instance.lastEnviro;

		Vector3 pos = new Vector3 (0f, 0.01f, enviro.transform.position.z + 50f);

		if (GenerationManager.Instance.lastTile.transform.position.z + 50f > pos.z) {
			GenerationManager.Instance.lastTile.SetActive (false);
		}

		prefab.SetActive (true);
		prefab.transform.position = pos;
		prefab.transform.rotation = rot;

		for (int i = 0; i < prefab.transform.GetChild (3).childCount; i++) {
			if (!prefab.transform.GetChild (3).GetChild (i).gameObject.activeInHierarchy)
				prefab.transform.GetChild (3).GetChild (i).gameObject.SetActive (true);
		}
	}
}
