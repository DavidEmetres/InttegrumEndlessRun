using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	[SerializeField] private Text loadingText;

	private void Start() {
		StartCoroutine ("LoadScene");
	}

	private void Update() {
		loadingText.color = new Color (loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1f));
	}

	private IEnumerator LoadScene() {
		yield return new WaitForSeconds (3);

		AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (1);

		while (!async.isDone)
			yield return null;
	}
}
