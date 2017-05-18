using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public void EndAnim() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
	}
}
