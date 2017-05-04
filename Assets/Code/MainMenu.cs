using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	private List<Dictionary<string, object>> provincesData;

	public GameObject button1;
	public GameObject button2;
	public GameObject button3;
	public GameObject exitDialog;

	public static MainMenu Instance;

	private void Awake() {
		Instance = this;

		provincesData = CSVReader.Read ("provinces_data");
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
		switch (button) {
			case 1:
				button2.GetComponent<Button> ().enabled = false;
				button3.GetComponent<Button> ().enabled = false;
				button1.GetComponent<Animator> ().SetTrigger ("buttonPressed");
				break;
			case 2:
				button1.GetComponent<Button> ().enabled = false;
				button3.GetComponent<Button> ().enabled = false;
				button2.GetComponent<Animator> ().SetTrigger ("buttonPressed");
				break;
			case 3:
				button1.GetComponent<Button> ().enabled = false;
				button2.GetComponent<Button> ().enabled = false;
				button3.GetComponent<Animator> ().SetTrigger ("buttonPressed");
				break;
		}
	}
}
