using UnityEngine;
using System.Collections;

public class ProvinceChangeBehaviour : MonoBehaviour {

	private bool activated = false;
	private Transform endTunnel;

	private void Start() {
		endTunnel = transform.GetChild (1);
	}

	private void Update() {

	}

	private void OnTriggerEnter(Collider other) {
		if (!activated) {
			if (other.tag == "Player") {
				activated = true;
				SceneManager.Instance.ProvinceChange ();
				GenerationManager.Instance.DestroyTerrainMesh ();
				GenerationManager.Instance.ChangeTerrainMat ();
				GenerationManager.Instance.BuildTerrainMesh (endTunnel.position.z - 190f);
				GenerationManager.Instance.BuildEnviroMesh (endTunnel.position.z - 190f, false);
				GenerationManager.Instance.BuildEnviroMesh (endTunnel.position.z - 190f, true);
				GenerationManager.Instance.changingProvince = false;
				GenerationManager.Instance.tileCount = 5;
			}
		}
	}
}
