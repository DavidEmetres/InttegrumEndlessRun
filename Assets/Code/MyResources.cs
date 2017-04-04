using UnityEngine;
using System.Collections;

public class MyResources : MonoBehaviour {

	[SerializeField] private GameObject coin;
	[SerializeField] private GameObject tunnel;

	[Header("Oceanic Assets")]
	[SerializeField] private GameObject[] oceanicPrefabsType1;
	[SerializeField] private GameObject[] oceanicPrefabsType2;
	[SerializeField] private GameObject[] oceanicPrefabsType3;
	[SerializeField] private GameObject[] oceanicPrefabsType4;
	[SerializeField] private GameObject[] oceanicPrefabsType5;
	[SerializeField] private GameObject[] oceanicPrefabsType6;
	[SerializeField] private GameObject[] oceanicEnvironment;
	[SerializeField] private Material oceanicTerrainMat;
	[SerializeField] private Material oceanicEnviroMat;
	[SerializeField] private GameObject oceanicRoadChange;

	[Header("Continental Assets")]
	[SerializeField] private GameObject[] continentalPrefabsType1;
	[SerializeField] private GameObject[] continentalPrefabsType2;
	[SerializeField] private GameObject[] continentalPrefabsType3;
	[SerializeField] private GameObject[] continentalPrefabsType4;
	[SerializeField] private GameObject[] continentalPrefabsType5;
	[SerializeField] private GameObject[] continentalPrefabsType6;
	[SerializeField] private GameObject[] continentalEnvironment;
	[SerializeField] private Material continentalTerrainMat;
	[SerializeField] private Material continentalEnviroMat;
	[SerializeField] private GameObject continentalRoadChange;

	[Header("Mediterranean Assets")]
	[SerializeField] private GameObject[] mediterraneanPrefabsType1;
	[SerializeField] private GameObject[] mediterraneanPrefabsType2;
	[SerializeField] private GameObject[] mediterraneanPrefabsType3;
	[SerializeField] private GameObject[] mediterraneanPrefabsType4;
	[SerializeField] private GameObject[] mediterraneanPrefabsType5;
	[SerializeField] private GameObject[] mediterraneanPrefabsType6;
	[SerializeField] private GameObject[] mediterraneanEnvironment;
	[SerializeField] private Material mediterraneanTerrainMat;
	[SerializeField] private Material mediterraneanEnviroMat;
	[SerializeField] private GameObject mediterraneanRoadChange;

	public static MyResources Instance;

	private void Awake() {
		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	public Material GetMaterial(Climate climate, bool terrain) {
		Material selectedMat = null;

		if (climate == Climate.Oceanic) {
			if (terrain)
				selectedMat = oceanicTerrainMat;
			else
				selectedMat = oceanicEnviroMat;
		}

		else if (climate == Climate.Continental) {
			if (terrain)
				selectedMat = continentalTerrainMat;
			else
				selectedMat = continentalEnviroMat;
		}

		else if (climate == Climate.Mediterranean) {
			if (terrain)
				selectedMat = mediterraneanTerrainMat;
			else
				selectedMat = mediterraneanEnviroMat;
		}

		return selectedMat;
	}

	public GameObject GetCoin() {
		return coin;
	}

	public GameObject GetRoadChange(Climate climate) {
		GameObject rc = null;

		if (climate == Climate.Oceanic) {
			rc = oceanicRoadChange;
		}
		else if (climate == Climate.Continental) {
			rc = continentalRoadChange;
		}
		else if (climate == Climate.Mediterranean) {
			rc = mediterraneanRoadChange;
		}

		return rc;
	}

	public GameObject GetTunnel() {
		return tunnel;
	}

	public GameObject GetObstacle(Climate climate, int type, int num) {
		GameObject[] selectedArray = null;

		if (climate == Climate.Oceanic) {
			switch (type) {
				case 1:
					selectedArray = oceanicPrefabsType1;
					break;
				case 2:
					selectedArray = oceanicPrefabsType2;
					break;
				case 3:
					selectedArray = oceanicPrefabsType3;
					break;
				case 4:
					selectedArray = oceanicPrefabsType4;
					break;
				case 5:
					selectedArray = oceanicPrefabsType5;
					break;
				case 6:
					selectedArray = oceanicPrefabsType6;
					break;
			}
		}

		else if (climate == Climate.Continental) {
			switch (type) {
				case 1:
					selectedArray = continentalPrefabsType1;
					break;
				case 2:
					selectedArray = continentalPrefabsType2;
					break;
				case 3:
					selectedArray = continentalPrefabsType3;
					break;
				case 4:
					selectedArray = continentalPrefabsType4;
					break;
				case 5:
					selectedArray = continentalPrefabsType5;
					break;
				case 6:
					selectedArray = continentalPrefabsType6;
					break;
			}
		}

		else if (climate == Climate.Mediterranean) {
			switch (type) {
				case 1:
					selectedArray = mediterraneanPrefabsType1;
					break;
				case 2:
					selectedArray = mediterraneanPrefabsType2;
					break;
				case 3:
					selectedArray = mediterraneanPrefabsType3;
					break;
				case 4:
					selectedArray = mediterraneanPrefabsType4;
					break;
				case 5:
					selectedArray = mediterraneanPrefabsType5;
					break;
				case 6:
					selectedArray = mediterraneanPrefabsType6;
					break;
			}
		}

		return selectedArray [num-1];
	}

	public GameObject GetEnviro(Climate climate, int num) {
		GameObject[] selectedArray = null;

		if(climate == Climate.Oceanic) {
			selectedArray = oceanicEnvironment;
		}

		else if(climate == Climate.Continental) {
			selectedArray = continentalEnvironment;
		}

		else if(climate == Climate.Mediterranean) {
			selectedArray = mediterraneanEnvironment;
		}

		return selectedArray[num];
	}
}
