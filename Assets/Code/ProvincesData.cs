using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProvincesData : MonoBehaviour {

	private List<Dictionary<string, object>> provincesData;

	public string selectedProvince;
	public Sprite[] provincesImages;

	public static ProvincesData Instance;

	private void Awake() {
		Instance = this;
		DontDestroyOnLoad (this.gameObject);
		provincesData = CSVReader.Read ("provinces_data");
	}

	public Dictionary<string, object> GetData() {
		for (int i = 0; i < provincesData.Count; i++) {
			if (provincesData [i] ["provincia"].ToString().ToLower() == selectedProvince) {
				return provincesData [i];
			}
		}

		return null;
	}
}
