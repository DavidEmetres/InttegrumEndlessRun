using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour {

	[SerializeField] private GameObject[] screens;

	public void ChangeScreen(string fromToScreen) {
		string[] t = fromToScreen.Split ('_');
		int fromScreen = int.Parse (t [0]);
		int toScreen = int.Parse (t [1]);

		if (fromScreen >= 0) {
			screens [fromScreen].GetComponent<Animator> ().SetTrigger ("disappear");
		}

		if (toScreen >= 0) {
			screens [toScreen].GetComponent<Animator> ().SetTrigger ("appear");
		}
		else {
			MainMenu.Instance.button1.GetComponent<Button> ().enabled = true;
			MainMenu.Instance.button2.GetComponent<Button> ().enabled = true;
			MainMenu.Instance.button3.GetComponent<Button> ().enabled = true;
		}
	}

	public void ChangeScene(string scene) {
		UnityEngine.SceneManagement.SceneManager.LoadScene (scene);
	}
}
