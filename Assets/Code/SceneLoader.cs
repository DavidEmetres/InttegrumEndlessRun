using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	[SerializeField] private Text loadingText;
	[SerializeField] private GameObject logo;
	[SerializeField] private Text provinceDescription;
	[SerializeField] private Image provinceImage;

	private void Start() {
		StartCoroutine ("LoadScene");
		ProvincesData pd = GameObject.Find ("ProvincesData").GetComponent<ProvincesData> ();
		Dictionary<string,object> pData = pd.GetData ();
		provinceDescription.text = pData ["descripcion"].ToString ();

		List<Sprite> imgs = new List<Sprite> ();
		foreach (Sprite img in pd.provincesImages) {
			string[] temp = img.name.Split ('_');
			if (temp [0] == pd.selectedProvince) {
				imgs.Add (img);
			}
		}

		int i = Random.Range (0, imgs.Count);
		provinceImage.sprite = imgs [i];
	}

	private void Update() {
		loadingText.color = new Color (loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1f));
		logo.transform.RotateAround (logo.transform.position, logo.transform.forward, -200f * Time.deltaTime);
	}

	private IEnumerator LoadScene() {
		yield return new WaitForSeconds (5f);

		AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (2);

		while (!async.isDone)
			yield return null;
	}
}
