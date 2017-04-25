using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProvinceSelectionScreen : MonoBehaviour {

	private string provinceSelected;
	[SerializeField] private Text provinceSelectedText;
	[SerializeField] private GameObject[] provincesShapes;

	public void ProvinceClicked(string province) {
		ProvincesData.Instance.selectedProvince = province.ToLower();
		provinceSelectedText.text = province;
		foreach(GameObject p in provincesShapes) {
			if (p.name == province.ToLower()) {
				p.GetComponent<Image> ().color = new Color (0.8f, 0.8f, 0.8f, 1f);
			}
			else {
				p.GetComponent<Image> ().color = Color.white;
			}
		}
	}

	public void PlayButtonClicked() {
		GameObject obj = new GameObject (provinceSelected);
		DontDestroyOnLoad (obj);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("LoadingScreen");
	}
}
