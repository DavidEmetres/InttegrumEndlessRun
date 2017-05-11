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

	public Text kmText;
	public Text coinsText;
	public LineRenderer line;

	public static ResultsScreen Instance;

	private void Awake() {
		Instance = this;
	}

	private void Update() {
		if (showingKm) {
			if (kmShown < totalKm) {
				kmShown+=10;
				kmText.text = kmShown.ToString ("F1") + " km";
			}
			if (kmShown >= totalKm || Input.GetMouseButtonDown(0)) {
				kmText.text = totalKm.ToString ("F1") + " km";
			}
		}

		if (showingCoins) {
			if (coinsShown < totalCoins) {
				coinsShown+=10;
				coinsText.text = coinsShown.ToString ();
			}
			if (coinsShown >= totalCoins || Input.GetMouseButtonDown(0)) {
				coinsText.text = totalCoins.ToString ();
				Invoke ("ShowRun", 1f);
			}
		}
	}

	public void ShowScreen() {
		GetComponent<Animator> ().SetTrigger ("appear");
	}

	public void ShowKm() {
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
			yield return new WaitForSeconds (1f);
		}

		Invoke ("ShowKm", 1f);
	}

	public void ShowCoins() {
		totalCoins = SceneManager.Instance.coins;
		showingCoins = true;
	}

	public void BackToMainMenu() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
	}

	public void RestartGame() {
		SoundManager.Instance.ChangeMusic ("mainMenu");
		UnityEngine.SceneManagement.SceneManager.LoadScene ("LoadingScreen");
	}
}