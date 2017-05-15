using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProvinceSelectionScreen : MonoBehaviour {

	private string provinceSelected;
	[SerializeField] private Text provinceSelectedText;
	[SerializeField] private GameObject[] provincesShapes;
	[SerializeField] private Button backButton;
	private AudioPlayer audio;
	private bool ready;

	public void Start() {
		audio = GetComponent<AudioPlayer> ();

		int[] pu = GlobalData.Instance.provincesUnlocked;

		while (pu.Length == 0) {
			pu = GlobalData.Instance.provincesUnlocked;
		}

		for (int i = 0; i < pu.Length; i++) {
			if (pu[i] == 1) {
				provincesShapes [i].SetActive (true);
			}
			else {
				provincesShapes [i].SetActive (false);
			}
		}

		ProvinceClicked ("MADRID");
	}

	public void ProvinceClicked(string province) {
		if (!ready) {
			audio.PlayFX (1);
			ProvincesData.Instance.selectedProvince = province.ToLower ();
			provinceSelectedText.text = province;
			foreach (GameObject p in provincesShapes) {
				if (p.name == province.ToLower ()) {
					p.GetComponent<Image> ().color = new Color (0.8f, 0.8f, 0.8f, 1f);
				}
				else {
					p.GetComponent<Image> ().color = Color.white;
				}
			}
		}
	}

	public void PlayButtonClicked() {
		if (!ready) {
			StartCoroutine (PlayButtonCoroutine ());
			backButton.enabled = false;
			ready = true;
		}
	}

	IEnumerator PlayButtonCoroutine() {
		audio.PlayFX (2);

		yield return new WaitForSeconds (audio.GetClipLength (2) + 0.5f);

		UnityEngine.SceneManagement.SceneManager.LoadScene ("LoadingScreen");
	}
}
