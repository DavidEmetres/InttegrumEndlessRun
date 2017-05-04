using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProvinceChangeBehaviour : MonoBehaviour {

	private bool activated = false;
	private Transform endTunnel;
	private GameObject sign;

	private void Start() {
		endTunnel = transform.GetChild (1);
		sign = transform.GetChild (2).gameObject;
	}

	private void OnTriggerEnter(Collider other) {
		if (!activated) {
			if (other.tag == "Player") {
				activated = true;
				GenerationManager.Instance.signAppeared = false;
				SceneManager.Instance.ProvinceChange ();
				GenerationManager.Instance.DestroyTerrainMesh ();
				GenerationManager.Instance.BuildTerrainMesh (endTunnel.position.z - 190f);
				GenerationManager.Instance.BuildEnviroMesh (endTunnel.position.z - 190f, false);
				GenerationManager.Instance.BuildEnviroMesh (endTunnel.position.z - 190f, true);
				GenerationManager.Instance.changingProvince = false;
				GenerationManager.Instance.tileCount = 5;
				GenerationManager.Instance.enviroCount = 0;
				HUDManager.Instance.ChangeProvince ();
				sign.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = SceneManager.Instance.currentProvince.name.ToUpper();
				Invoke ("ReActivate", 3f);
			}
		}
	}

	private void ReActivate() {
		activated = false;
	}
}
