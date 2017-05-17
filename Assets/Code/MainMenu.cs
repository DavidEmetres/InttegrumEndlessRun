using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class MainMenu : MonoBehaviour {

	private List<Dictionary<string, object>> provincesData;
	private AudioPlayer audio;

	public GameObject button1;
	public GameObject button2;
	public GameObject button3;
	public GameObject exitDialog;
	public GameObject rewardButton;
	public GameObject rewardDialog;

	public static MainMenu Instance;

	private void Awake() {
		Instance = this;

		provincesData = CSVReader.Read ("provinces_data");

		audio = GetComponent<AudioPlayer> ();

		if (Time.timeScale < 1)
			Time.timeScale = 1;
	}

	private void Start() {
		SoundManager.Instance.ChangeMusic ("mainMenu");

		button1.GetComponent<Button> ().enabled = true;
		button2.GetComponent<Button> ().enabled = true;
		button3.GetComponent<Button> ().enabled = true;
	}

	private void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {	//BACK BUTTON;
			ShowExitDialog(true);
		}

		CheckRefill ();
	}

	public void CheckRefill() {
		if (DateTime.Compare(DateTime.Now, GlobalData.Instance.nextRefill) > -1) {
			rewardButton.SetActive (true);
			GameObject.Find ("ProvinceSelectionScreen").GetComponent<ProvinceSelectionScreen> ().ShowRewardButton (true);
			if (GlobalData.Instance.justEnteredGame) {
				rewardButton.GetComponent<Button> ().onClick.Invoke ();
				GlobalData.Instance.justEnteredGame = false;
			}
		}
		else {
			GameObject.Find ("ProvinceSelectionScreen").GetComponent<ProvinceSelectionScreen> ().UpdateTicketTimer ();
			if (GlobalData.Instance.justEnteredGame)
				GlobalData.Instance.justEnteredGame = false;
		}
	}

	public void ShowRewardDialog(bool visible) {
		rewardDialog.SetActive (visible);
		button1.GetComponent<Button> ().enabled = !visible;
		button2.GetComponent<Button> ().enabled = !visible;
		button3.GetComponent<Button> ().enabled = !visible;
		rewardButton.GetComponent<Button> ().enabled = !visible;
	}

	public void GetReward() {
		GlobalData.Instance.tickets += 5;
		ShowRewardDialog (false);
		rewardButton.SetActive (false);
		GameObject.Find ("ProvinceSelectionScreen").GetComponent<ProvinceSelectionScreen> ().ShowRewardButton (false);
		GameObject.Find ("TicketCount").transform.GetChild (0).GetComponent<Text> ().text = GlobalData.Instance.tickets.ToString ();
		GlobalData.Instance.nextRefill = DateTime.Now.AddHours (24);
	}

	public void ShowExitDialog(bool visible) {
		audio.PlayFX (0);
		exitDialog.SetActive (visible);
		button1.GetComponent<Button> ().enabled = !visible;
		button2.GetComponent<Button> ().enabled = !visible;
		button3.GetComponent<Button> ().enabled = !visible;
	}

	public void ExitApplication() {
		audio.PlayFX (0);
		GlobalData.Instance.SaveGame ();
		Application.Quit ();
	}

	public void AddCoins() {
		GlobalData.Instance.coins += 10000;
		GlobalData.Instance.SaveGame ();
	}

	public void Delete() {
		GlobalData.Instance.DeleteSaveGame ();
		GlobalData.Instance.SaveGame ();
		GlobalData.Instance.LoadGame ();
	}

	public void ButtonPressed(int button) {
		audio.PlayFX (0);

		button1.GetComponent<Button> ().enabled = false;
		button2.GetComponent<Button> ().enabled = false;
		button3.GetComponent<Button> ().enabled = false;

		switch (button) {
			case 1:
				button1.GetComponent<Animator> ().SetTrigger ("buttonPressed");
				break;
			case 2:
				button2.GetComponent<Animator> ().SetTrigger ("buttonPressed");
				break;
			case 3:
				button3.GetComponent<Animator> ().SetTrigger ("buttonPressed");
				break;
		}
	}
}
