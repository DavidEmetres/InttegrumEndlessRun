using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public AsyncOperation async;

	private void Start() {
		async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("MainMenu");
		async.allowSceneActivation = false;
	}

	public void EndAnim() {
//		UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
		async.allowSceneActivation = true;
	}
}
