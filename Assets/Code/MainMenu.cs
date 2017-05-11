using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	private List<Dictionary<string, object>> provincesData;
	private AudioPlayer audio;

	public GameObject button1;
	public GameObject button2;
	public GameObject button3;
	public GameObject exitDialog;

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
	}

	public void ShowExitDialog(bool visible) {
		exitDialog.SetActive (visible);
		button1.GetComponent<Button> ().enabled = !visible;
		button2.GetComponent<Button> ().enabled = !visible;
		button3.GetComponent<Button> ().enabled = !visible;
	}

	public void ExitApplication() {
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
