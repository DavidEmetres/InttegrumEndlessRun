using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	[SerializeField] private Text loadingText;
	[SerializeField] private GameObject logo;
	[SerializeField] private Text provinceDescription;

	private void Start() {
		StartCoroutine ("LoadScene");
		Dictionary<string,object> pData = GameObject.Find ("ProvincesData").GetComponent<ProvincesData> ().GetData ();
		provinceDescription.text = pData ["descripcion"].ToString ();
	}

	private void Update() {
		loadingText.color = new Color (loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1f));
		logo.transform.RotateAround (logo.transform.position, logo.transform.forward, -200f * Time.deltaTime);
	}

	private IEnumerator LoadScene() {
		yield return new WaitForSeconds (10);

		AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (1);

		while (!async.isDone)
			yield return null;
	}
}
