using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProvincesData : MonoBehaviourSingleton<ProvincesData> {

	private List<Dictionary<string, object>> provincesData;

	public string selectedProvince;
	public Sprite[] provincesImages;

	private void Awake() {
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
