using UnityEngine;
using System.Collections;

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
	}
}
