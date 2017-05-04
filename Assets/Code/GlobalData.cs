using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class GlobalData : MonoBehaviour {

	public int coins;
	public int tickets;
	public float kmRunned;
	public List<int[]> stampsUnlocked;
	public int[] provincesUnlocked;
	public bool firstTime;

	public static GlobalData Instance;

	private void Awake() {
		if (Instance != null && Instance != this)
			Destroy (gameObject);

		Instance = this;
		DontDestroyOnLoad (gameObject);

		bool b = false;

		if (!b) {
			stampsUnlocked = new List<int[]> ();
			provincesUnlocked = new int[47];

			for (int i = 0; i < 47; i++) {
				stampsUnlocked.Add (new int[5] { 0, 0, 0, 0, 0 });
				provincesUnlocked [i] = 0;
			}

			provincesUnlocked [27] = 1;

			firstTime = true;

			SaveGame ();
		}
	}

	public void SaveGame() {
		if (File.Exists (Application.persistentDataPath + "/playerData.binary"))
			DeleteSaveGame ();

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.binary");

		GameData data = new GameData ();
		data.coins = coins;
		data.tickets = tickets;
		data.kmRunned = kmRunned;
		data.stampsUnlocked = stampsUnlocked;
		data.provincesUnlocked = provincesUnlocked;
		data.firstTime = firstTime;

		bf.Serialize (file, data);
		file.Close ();
	}

	public bool LoadGame() {
		if (File.Exists (Application.persistentDataPath + "/playerData.binary")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerData.binary", FileMode.Open);
			GameData data = (GameData)bf.Deserialize (file);
			file.Close ();

			coins = data.coins;
			tickets = data.tickets;
			kmRunned = data.kmRunned;
			stampsUnlocked = data.stampsUnlocked;
			provincesUnlocked = data.provincesUnlocked;
			firstTime = data.firstTime;

			return true;
		}

		return false;
	}

	public void DeleteSaveGame() {
		if (File.Exists (Application.persistentDataPath + "/playerData.binary")) {
			File.Delete (Application.persistentDataPath + "/playerData.binary");
		}
	}

	private void OnApplicationQuit() {
		SaveGame ();
	}
}

[Serializable]
public class GameData {

	public int coins;
	public int tickets;
	public float kmRunned;
	public List<int[]> stampsUnlocked;
	public int[] provincesUnlocked;
	public bool firstTime;
}
