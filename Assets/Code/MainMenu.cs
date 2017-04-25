using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	private List<Dictionary<string, object>> provincesData;

	public GameObject button1;
	public GameObject button2;
	public GameObject button3;

	public static MainMenu Instance;

	private void Awake() {
		Instance = this;

		provincesData = CSVReader.Read ("provinces_data");
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
