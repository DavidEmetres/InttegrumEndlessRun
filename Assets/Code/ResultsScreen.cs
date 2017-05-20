using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour {

	private float kmShown;
	private float totalKm;
	private bool showingKm;
	private int coinsShown;
	private int totalCoins;
	private bool showingCoins;
	private List<Province> provincesRunned = new List<Province>();
	private GameObject startLine;
	private GameObject endLine;
	private bool popUpVisible;
	private bool rewardObtained;
	private AudioPlayer audio;

	public Text kmText;
	public Text coinsText;
	public LineRenderer line;
	public GameObject coinAdPopUp;
	public GameObject adButton;

	public static ResultsScreen Instance;

	private void Awake() {
		Instance = this;

		audio = GetComponent<AudioPlayer> ();
	}

	private void Update() {
		if (showingKm) {
			if (kmShown < totalKm) {
				kmShown+=10;
				kmText.text = kmShown.ToString ("F1") + " km";
			}
			if (kmShown >= totalKm || Input.GetMouseButtonDown(0)) {
				kmText.text = totalKm.ToString ("F1") + " km";
				showingKm = false;
			}
		}

		if (showingCoins) {
			if (coinsShown < totalCoins) {
				coinsShown+=10;
				coinsText.text = coinsShown.ToString ();
			}
			if (coinsShown >= totalCoins || Input.GetMouseButtonDown(0)) {
				coinsText.text = totalCoins.ToString ();
				showingCoins = false;
				Invoke ("ShowRun", 0.5f);
			}
		}
	}

	public void ShowScreen() {
		SoundManager.Instance.music.volume /= 2f;
		GetComponent<Animator> ().SetTrigger ("appear");
	}

	public void ShowKm() {
		audio.PlayFX (1);
		totalKm = SceneManager.Instance.totalKm + SceneManager.Instance.provinceKm;
		showingKm = true;
	}

	public void ShowRun() {
		provincesRunned = SceneManager.Instance.provincesRunned;
		StartCoroutine (ShowProvinces ());
	}

	IEnumerator ShowProvinces() {
		for (int i = 0; i < provincesRunned.Count; i++) {
			GameObject p = GameObject.Find (provincesRunned [i].name.ToLower ());
			p.GetComponent<Image> ().enabled = true;
			audio.PlayFX (2);
			yield return new WaitForSeconds (0.5f);
		}

		Invoke ("ShowKm", 0);
	}

	public void ShowCoins() {
		audio.PlayFX (0);
		totalCoins = SceneManager.Instance.coins;
		showingCoins = true;
	}

	public void ShowCoinAd(bool visible) {
		audio.PlayFX (3);
		popUpVisible = visible;
		coinAdPopUp.SetActive (visible);
	}

	public void RunAd() {
		UnityAds.Instance.ShowCoinAd ();
	}

	public void RewardObtained() {
		rewardObtained = true;
		adButton.GetComponent<Button> ().enabled = false;
		adButton.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.2f);
		adButton.transform.GetChild(0).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.2f);
		adButton.transform.GetChild(1).GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.2f);
		coinsText.text = "" + (SceneManager.Instance.coins + SceneManager.Instance.coins);
	}

	public void BackToMainMenu() {
		if (!popUpVisible) {
			SoundManager.Instance.music.volume *= 2f;
			audio.PlayFX (3);
			UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
		}
	}

	public void RestartGame() {
		if (!popUpVisible) {
			SoundManager.Instance.music.volume *= 2f;
			audio.PlayFX (3);
			SoundManager.Instance.ChangeMusic ("mainMenu");
			UnityEngine.SceneManagement.SceneManager.LoadScene ("LoadingScreen");
		}
	}
}