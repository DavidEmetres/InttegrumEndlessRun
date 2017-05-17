using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ProvinceSelectionScreen : MonoBehaviour {

	private string provinceSelected;
	[SerializeField] private Text provinceSelectedText;
	[SerializeField] private GameObject[] provincesShapes;
	[SerializeField] private Button backButton;
	[SerializeField] private Text tickets;
	[SerializeField] private GameObject ticketAdPopUp;
	[SerializeField] private GameObject rewardButton;
	[SerializeField] private Text rewardLabel;
	[SerializeField] private Text rewardTimer;
	private AudioPlayer audio;
	private bool ready;
	private bool popUpVisible;

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

		tickets.text = GlobalData.Instance.tickets.ToString ();

		ProvinceClicked ("MADRID");
	}

	public void ProvinceClicked(string province) {
		if (!ready && !popUpVisible) {
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
		if (!ready && !popUpVisible) {
			if (GlobalData.Instance.tickets == 0) {
				ShowTicketAdPopUp (true);
			}
			else {
				StartCoroutine (PlayButtonCoroutine ());
				backButton.enabled = false;
				ready = true;
			}
		}
	}

	IEnumerator PlayButtonCoroutine() {
		audio.PlayFX (2);

		yield return new WaitForSeconds (audio.GetClipLength (2) + 0.5f);

		UnityEngine.SceneManagement.SceneManager.LoadScene ("LoadingScreen");
	}

	public void ShowTicketAdPopUp(bool visible) {
		popUpVisible = visible;
		backButton.enabled = !visible;
		ticketAdPopUp.SetActive (visible);
	}

	public void ShowRewardButton(bool visible) {
		rewardButton.SetActive (visible);
		if (visible) {
			rewardLabel.text = "¡Recoge tus 5 tickets!";
			rewardTimer.gameObject.SetActive (false);
		}
		else {
			rewardLabel.text = "Recibirás 5 tickets en:";
			UpdateTicketTimer ();
		}
	}

	public void UpdateTicketTimer() {
		if (!rewardTimer.gameObject.activeInHierarchy)
			rewardTimer.gameObject.SetActive (true);
		TimeSpan t = GlobalData.Instance.nextRefill.Subtract (DateTime.Now);
		rewardTimer.text = t.ToString ().Remove (8);
	}
}
