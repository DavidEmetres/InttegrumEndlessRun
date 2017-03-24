#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class ObstacleEditor : MonoBehaviour {

	private Vector4[,] tileArray = new Vector4[10, 3];
	private Vector2 tileSelected = new Vector2 (-1, -1);
	private bool anyTileSelected = false;
	private GameObject prefabSelected;
	private float objNumber;
	private GameObject buttonSelected;
	private GameObject coinSelected;
	private GameObject objSelected;
	private bool rotatingCam;
	private float camRotDirection;
	private List<ScrollStruct> allFiles = new List<ScrollStruct>();
	private string fileLoaded = "";
	private Climate currentClimate;
	private bool forcingNextTile;
	private bool loadingStructure;

	public LayerMask editorRayCast;
	public Color tileOcuppiedColor;
	public GameObject coinScrollbar;
	public GameObject obstacleScrollbar;
	public GameObject deleteCoinButton;
	public GameObject deleteObstacleButton;
	public Transform cameraRotPivot;
	public GameObject scrollview;
	public Sprite buttonSprite;
	public Font buttonFont;
	public GameObject loadButton;
	public GameObject newButton;
	public GameObject editButton;
	public GameObject deleteButton;
	public GameObject textSaved;
	public GameObject currentFileText;
	public GameObject climateButton;
	public GameObject obstacleScrollview;
	public GameObject forceNextTileToggle;

	public static ObstacleEditor Instance;

	private void Awake() {
		Instance = this;
		currentClimate = Climate.Oceanic;
		climateButton.transform.GetChild(0).GetComponent<Text> ().text = currentClimate.ToString();
	}

	private void Update() {
		if (tileSelected != new Vector2 (-1, -1))
			anyTileSelected = true;
		else
			anyTileSelected = false;

		if (coinSelected != null) {
			coinScrollbar.SetActive (true);
			deleteCoinButton.SetActive (true);
			float y = (float)System.Math.Round (coinScrollbar.GetComponent<Scrollbar> ().value, 2);
			y = ScrollbarToTransform (y);
			if (y == 0f)
				y = 1f;
			coinSelected.transform.position = new Vector3 (coinSelected.transform.position.x, y, coinSelected.transform.position.z);
			tileArray [(int)tileSelected.x, (int)tileSelected.y].w = y;
		}
		else {
			coinScrollbar.SetActive (false);
			deleteCoinButton.SetActive (false);
		}

		if (objSelected != null) {
			obstacleScrollbar.SetActive (true);
			deleteObstacleButton.SetActive (true);
			float y = (float)System.Math.Round (obstacleScrollbar.GetComponent<Scrollbar> ().value, 2);
			y = ScrollbarToTransform (y);
			objSelected.transform.position = new Vector3 (objSelected.transform.position.x, y, objSelected.transform.position.z);
			tileArray [(int)tileSelected.x, (int)tileSelected.y].y = y;
		}
		else {
			obstacleScrollbar.SetActive (false);
			deleteObstacleButton.SetActive (false);
		}

		//ROTATE CAMERA;
		if (rotatingCam)
			Camera.main.transform.RotateAround (cameraRotPivot.position, Vector3.up, 2f * camRotDirection);
		
		//LOAD SCREEN;
		if (scrollview.activeInHierarchy) {
			loadButton.SetActive (false);
		}
		else
			loadButton.SetActive (true);

		//EDIT/DELETE STRUCTURE;
		if (fileLoaded != "") {
			editButton.SetActive (true);
			deleteButton.SetActive (true);
			currentFileText.GetComponent<Text> ().text = fileLoaded;

		}
		else {
			editButton.SetActive (false);
			deleteButton.SetActive (false);
			currentFileText.GetComponent<Text> ().text = "No file loaded";
		}
	}

	private IEnumerator ChangeTextAlpha(Text txt) {
		while (txt.color.a > 0.1f) {
			txt.color = new Color (txt.color.r, txt.color.g, txt.color.b, txt.color.a - 0.02f);
			yield return null;
		}

		txt.gameObject.SetActive (false);
		txt.color = new Color (txt.color.r, txt.color.g, txt.color.b, 1f);
	}

	public void SaveStructure() {
		string path = Application.dataPath + "/Resources/TileArrays/";
		string[] lines = new string[11];

		for (int i = 9; i >= 0; i--) {
			lines [9-i] = tileArray [i, 0].x + "," + tileArray [i, 0].y + "," + tileArray [i, 0].z + "," + tileArray [i, 0].w + "|" +
			tileArray [i, 1].x + "," + tileArray [i, 1].y + "," + tileArray [i, 1].z + "," + tileArray [i, 1].w + "|" +
			tileArray [i, 2].x + "," + tileArray [i, 2].y + "," + tileArray [i, 2].z + "," + tileArray [i, 2].w;
		}

		if (forceNextTileToggle.GetComponent<Toggle> ().isOn) {
			string[] temp = forceNextTileToggle.transform.GetChild (1).GetComponent<Text> ().text.Split ('|');
			if (temp.Length > 1) {
				lines [10] = "TRUE |" + temp [1];
			}
			else
				lines [10] = "FALSE | null";
		}
		else
			lines [10] = "FALSE | null";

		string g = Guid.NewGuid().ToString("N");
		fileLoaded = currentClimate.ToString() + "_TileArray" + g;
		path += fileLoaded;
		System.IO.File.WriteAllLines (path + ".txt", lines);
		textSaved.GetComponent<Text> ().text = "SAVED";
		textSaved.SetActive (true);
		StartCoroutine ("ChangeTextAlpha", textSaved.GetComponent<Text> ());
		AssetDatabase.Refresh ();
	}

	public void EditStructure() {
		string path = Application.dataPath + "/Resources/TileArrays/";
		string[] lines = new string[11];

		for (int i = 9; i >= 0; i--) {
			lines [9-i] = tileArray [i, 0].x + "," + tileArray [i, 0].y + "," + tileArray [i, 0].z + "," + tileArray [i, 0].w + "|" +
				tileArray [i, 1].x + "," + tileArray [i, 1].y + "," + tileArray [i, 1].z + "," + tileArray [i, 1].w + "|" +
				tileArray [i, 2].x + "," + tileArray [i, 2].y + "," + tileArray [i, 2].z + "," + tileArray [i, 2].w;
		}

		if (forceNextTileToggle.GetComponent<Toggle> ().isOn) {
			string[] temp = forceNextTileToggle.transform.GetChild (1).GetComponent<Text> ().text.Split ('|');
			if (temp.Length > 1) {
				lines [10] = "TRUE |" + temp [1];
			}
			else
				lines [10] = "FALSE | null";
		}
		else
			lines [10] = "FALSE | null";

		path += fileLoaded;
		string metaPath = path + ".meta";
		System.IO.File.Delete (path);
		System.IO.File.Delete (metaPath);
		System.IO.File.WriteAllLines (path + ".txt", lines);
		textSaved.GetComponent<Text> ().text = "EDITED";
		textSaved.SetActive (true);
		StartCoroutine ("ChangeTextAlpha", textSaved.GetComponent<Text> ());
		AssetDatabase.Refresh ();
	}

	public void DeleteStructure() {
		string path = Application.dataPath + "/Resources/TileArrays/";
		path += fileLoaded + ".txt";
		string metaPath = path + ".meta";
		System.IO.File.Delete (path);
		System.IO.File.Delete (metaPath);
		ClearTiles ();
		textSaved.GetComponent<Text> ().text = "DELETED";
		textSaved.SetActive (true);
		StartCoroutine ("ChangeTextAlpha", textSaved.GetComponent<Text> ());

		for (int i = 0; i < scrollview.transform.GetChild(0).GetChild(0).childCount; i++) {
			Destroy (scrollview.transform.GetChild(0).GetChild(0).GetChild(i).gameObject);
		}

		AssetDatabase.Refresh ();
	}

	public void ClearTiles() {
		tileSelected = new Vector2 (-1, -1);
		prefabSelected = null;
		coinSelected = null;
		objSelected = null;
		fileLoaded = "";

		if (buttonSelected != null) {
			buttonSelected.GetComponent<Image> ().color = Color.white;
			buttonSelected = null;
		}

		BroadcastMessage ("ClearTile");

		for (int i = 0; i < tileArray.GetLength(0); i++) {
			for (int j = 0; j < tileArray.GetLength (1); j++) {
				tileArray [i, j] = Vector4.zero;
			}
		}
	}

	public void LoadStructure(string fileName) {
		ClearTiles ();
		fileLoaded = fileName;
		string[] clim = fileName.Split ('_');
		currentClimate = (Climate)Enum.Parse (typeof(Climate), clim [0], true); 
		climateButton.transform.GetChild (0).GetComponent<Text> ().text = currentClimate.ToString ();

		List<Vector4> vectorList = new List<Vector4> ();
		string line;
		string path = "TileArrays/" + fileName;

		TextAsset txt = (TextAsset)Resources.Load (path);

		if (txt != null) {
			StreamReader reader = new StreamReader (new MemoryStream (txt.bytes));

			using (reader) {
				do {
					line = reader.ReadLine ();

					if (line != null) {
						string[] vectors = line.Split ('|');

						if (vectors.Length > 2) {
							foreach (string v in vectors) {
								string[] tile = v.Split (',');

								float n1, n2, n3, n4;
								bool b = float.TryParse (tile [0], out n1);
								b = float.TryParse (tile [1], out n2);
								b = float.TryParse (tile [2], out n3);
								b = float.TryParse (tile [3], out n4);
								Vector4 vector = new Vector4 (n1, n2, n3, n4);
								vectorList.Add (vector);
							}
						}
						else if(vectors.Length > 1) {
							if(vectors[0] == "TRUE ") {
								forceNextTileToggle.GetComponent<Toggle>().isOn = true;
								forceNextTileToggle.transform.GetChild(1).GetComponent<Text>().text = "Force Next Tile |" + vectors[1];
							}
							else {
								forceNextTileToggle.GetComponent<Toggle>().isOn = false;
								forceNextTileToggle.transform.GetChild(1).GetComponent<Text>().text = "Force Next Tile";
							}
						}
					}
				} while(line != null);

				reader.Close ();
			}

			int count = vectorList.Count-1;
			for (int i = 0; i < tileArray.GetLength (0); i++) {
				for (int j = 2; j >= 0; j--) {
					tileArray [i, j] = vectorList [count];
					count--;

					Vector3 pos;

					if (tileArray [i, j].x != 0) {
						pos = transform.GetChild (i).GetChild (j).position;
						pos = new Vector3 (pos.x, tileArray [i, j].y, pos.z);

						string t = tileArray [i, j].x.ToString ();
						string[] temp = t.Split ('.');
						path = "Prefabs/" + currentClimate.ToString () + "/Obstacle_" + temp[0] + "_" + temp[1];

						GameObject prefab = (GameObject)Resources.Load(path);
						GameObject obj = Instantiate (prefab, pos, prefab.transform.rotation) as GameObject;

						if (obj.GetComponent<Displacement> () != null)
							obj.GetComponent<Displacement> ().enabled = false;

						obj.transform.SetParent (transform.GetChild (i).GetChild (j));

						transform.GetChild (i).GetChild (j).gameObject.GetComponent<SpriteRenderer> ().color = tileOcuppiedColor;
					}

					if (tileArray [i, j].z != 0) {
						pos = transform.GetChild (i).GetChild (j).position;
						pos = new Vector3 (pos.x, tileArray [i, j].w, pos.z);

						GameObject prefab = null;

						switch ((int)tileArray [i, j].z) {
							case -1:
								path = "Prefabs/" + currentClimate.ToString() + "/Coin";
								prefab = (GameObject)Resources.Load (path);
								break;
						}

						GameObject obj = Instantiate (prefab, pos, prefab.transform.rotation) as GameObject;

						if (obj.GetComponent<Displacement> () != null)
							obj.GetComponent<Displacement> ().enabled = false;
						if (obj.GetComponent<Animator> () != null)
							obj.GetComponent<Animator> ().enabled = false;

						obj.transform.SetParent (transform.GetChild (i).GetChild (j));

						transform.GetChild (i).GetChild (j).gameObject.GetComponent<SpriteRenderer> ().color = tileOcuppiedColor;
					}
				}
			}

			vectorList.Clear ();
		}
	}

	public void OpenLoadScrollview() {
		loadingStructure = true;

		if (tileSelected != new Vector2 (-1, -1)) {
			if (coinSelected != null || objSelected != null) {
				coinSelected = null;
				objSelected = null;

				Color c = transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color;
				transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color = new Color(c.r*2, c.g*2, c.b*2, 1f);
			}
			else
				transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color = Color.white;

			tileSelected = new Vector2 (-1, -1);
		}

		if (prefabSelected != null) {
			buttonSelected.GetComponent<Image>().color = Color.white;
			prefabSelected = null;
			buttonSelected = null;
		}
		
		scrollview.SetActive (true);
		string path = Application.dataPath + "/Resources/TileArrays/";

		string[] files = Directory.GetFiles (path, "*.txt", SearchOption.TopDirectoryOnly);

		for(int j = 0; j < files.Length; j++) {
			string[] temp = files [j].Split ('/');
			string fileName = temp [temp.Length - 1].Substring (0, temp [temp.Length - 1].Length - 4);
			DateTime creationTime = Directory.GetCreationTimeUtc (path + fileName + ".txt");

			ScrollStruct sc = new ScrollStruct (j, fileName, creationTime);
			allFiles.Add (sc);
		}

		ScrollComprarer comparer = new ScrollComprarer ();
		allFiles.Sort (comparer);

		for (int i = 0; i < allFiles.Count; i++) {
			GameObject go = new GameObject (allFiles[i].fileName);
			go.layer = LayerMask.NameToLayer ("UI");
			RectTransform rt = go.AddComponent<RectTransform> ();
			Button b = go.AddComponent<Button> ();
			Image img = go.AddComponent<Image> ();
			b.transform.parent = scrollview.transform.GetChild (0).GetChild (0).transform;
			b.targetGraphic = img;
			img.sprite = buttonSprite;
			img.type = Image.Type.Sliced;

			rt.anchorMax = new Vector2 (1, 1);
			rt.anchorMin = new Vector2 (0, 1);
			rt.pivot = new Vector2 (0.5f, 1f);
			rt.localScale = new Vector2 (1, 1);
			rt.offsetMax = new Vector2 (0, 0 - (i * 60));
			rt.offsetMin = new Vector2 (0, -60 - (i * 60));

			GameObject tgo = new GameObject ("Text");
			RectTransform t = tgo.AddComponent<RectTransform> ();
			tgo.transform.parent = go.transform;
			Text txt = tgo.AddComponent<Text> ();
			string[] clim = allFiles [i].fileName.Split ('_');
			txt.text = clim[0] + " " + allFiles[i].creationTime;
			txt.color = Color.black;
			txt.font = buttonFont;
			txt.fontSize = 40;

			t.anchorMax = new Vector2 (1, 1);
			t.anchorMin = new Vector2 (0, 0);
			t.pivot = new Vector2 (0.5f, 0.5f);
			t.localScale = new Vector2 (1, 1);
			t.offsetMax = new Vector2 (0, 0);
			t.offsetMin = new Vector2 (0, 0);

			if (forcingNextTile) {
				b.onClick.AddListener (delegate {
					ForceNextTile (b.name);
				});
			}
			else {
				b.onClick.AddListener (delegate {
					LoadStructure (b.name);
				});
			}
		}
	}

	public void CloseLoadScrollview() {
		loadingStructure = false;
		scrollview.SetActive (false);
		allFiles.Clear ();
	}

	public void ForceNextToggled() {
		if (forceNextTileToggle.GetComponent<Toggle> ().isOn) {
			if (!loadingStructure) {
				forcingNextTile = true;
				OpenLoadScrollview ();
			}
		}
		else {
			if (!loadingStructure) {
				forceNextTileToggle.transform.GetChild (1).GetComponent<Text> ().text = "Force Next Tile";
				forcingNextTile = false;
				if (scrollview.activeInHierarchy)
					CloseLoadScrollview ();
			}
		}
	}

	public void ForceNextTile(string nextTileName) {
		forcingNextTile = false;
		forceNextTileToggle.transform.GetChild(1).GetComponent<Text>().text = "Force Next Tile | " + nextTileName;
		CloseLoadScrollview ();
	}

	public void RotateCamera(bool right) {
		camRotDirection = (right) ? 1f : -1f;
		rotatingCam = true;
	}

	public void StopRotatingCamera() {
		rotatingCam = false;
	}

	public void Delete(bool coin) {
		if (coin) {
			Destroy (coinSelected.gameObject);
			coinSelected = null;
			tileArray [(int)tileSelected.x, (int)tileSelected.y].z = 0;
			tileArray [(int)tileSelected.x, (int)tileSelected.y].w = 0;
			if (objSelected == null) {
				transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
				tileSelected = new Vector2 (-1, -1);
			}
		}
		else {
			Destroy (objSelected.gameObject);
			objSelected = null;
			tileArray [(int)tileSelected.x, (int)tileSelected.y].x = 0;
			tileArray [(int)tileSelected.x, (int)tileSelected.y].y = 0;
			if (coinSelected == null) {
				transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
				tileSelected = new Vector2 (-1, -1);
			}
		}
	}

	public void SelectTile(Vector2 tile) {
		//PLACE GAMEOBJECT;
		if (prefabSelected != null) {
			if ((objNumber == -1 && tileArray [(int)tile.x, (int)tile.y].z == 0) || (objNumber != -1 && tileArray [(int)tile.x, (int)tile.y].x == 0)) {
				Vector3 pos = transform.GetChild ((int)tile.x).GetChild ((int)tile.y).position;
				Quaternion rot = prefabSelected.transform.rotation;
				pos = new Vector3 (pos.x + prefabSelected.transform.position.x, prefabSelected.transform.position.y, pos.z + prefabSelected.transform.position.z);
				GameObject obj = Instantiate (prefabSelected, pos, rot) as GameObject;

				if (obj.GetComponent<Displacement> () != null)
					obj.GetComponent<Displacement> ().enabled = false;
				if (obj.GetComponent<Animator> () != null)
					obj.GetComponent<Animator> ().enabled = false;
		
				obj.transform.SetParent (transform.GetChild ((int)tile.x).GetChild ((int)tile.y));

				transform.GetChild ((int)tile.x).GetChild ((int)tile.y).gameObject.GetComponent<SpriteRenderer> ().color = tileOcuppiedColor;

				if (objNumber == -1) {
					if (tileArray [(int)tile.x, (int)tile.y].x != 0 && tileArray [(int)tile.x, (int)tile.y].y < 2.25f) {
						pos = new Vector3 (pos.x, 2.25f, pos.z);
						obj.transform.position = pos;
					}

					tileArray [(int)tile.x, (int)tile.y].z = objNumber;
					if (pos.y <= 0) {
						pos = new Vector3 (pos.x, 1f, pos.z);
						obj.transform.position = pos;
					}
					tileArray [(int)tile.x, (int)tile.y].w = pos.y;
				}
				else {
					if (tileArray [(int)tile.x, (int)tile.y].z != 0 && tileArray [(int)tile.x, (int)tile.y].w < 2.25f) {
						tileArray [(int)tile.x, (int)tile.y].w = 2.25f;

						int childCount = transform.GetChild ((int)tile.x).GetChild ((int)tile.y).childCount;

						for (int i = 0; i < childCount; i++) {
							if (transform.GetChild ((int)tile.x).GetChild ((int)tile.y).GetChild (i).tag == "Coin") {
								Vector3 temp = transform.GetChild ((int)tile.x).GetChild ((int)tile.y).GetChild (i).transform.position;
								transform.GetChild ((int)tile.x).GetChild ((int)tile.y).GetChild (i).transform.position = new Vector3 (temp.x, 2.5f, temp.z);
								break;
							}
						}
					}

					tileArray [(int)tile.x, (int)tile.y].x = objNumber;
					tileArray [(int)tile.x, (int)tile.y].y = pos.y;
				}
			}
		}

		//SELECT TILE AND OR GAMEOBJECT;
		else {
			if (anyTileSelected) {
				Color c1 = transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color;
				transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color = new Color (c1.r * 2f, c1.g * 2f, c1.b * 2f, 1f);
				if (tileSelected == tile) {
					tileSelected = new Vector2 (-1, -1);
				}
				else {
					tileSelected = tile;
					Color c2 = transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color;
					transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color = new Color (c2.r/2f, c2.g/2f, c2.b/2f, 1f);
				}
			}
			else {
				tileSelected = tile;
				Color c3 = transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color;
				transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color = new Color (c3.r/2f, c3.g/2f, c3.b/2f, 1f);
			}

			if (tileSelected != new Vector2 (-1, -1)) {
				if (tileArray [(int)tileSelected.x, (int)tileSelected.y].z != 0) {
					int childCount = transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).childCount;

					for (int i = 0; i < childCount; i++) {
						if (transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).GetChild (i).tag == "Coin") {
							coinSelected = transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).GetChild (i).gameObject;
							break;
						}
					}

					float posy = TransformToScrollbar (coinSelected.transform.position.y);
					coinScrollbar.GetComponent<Scrollbar> ().value = posy;
				}
				else {
					coinSelected = null;
				}

				if (tileArray [(int)tileSelected.x, (int)tileSelected.y].x != 0) {
					int childCount = transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).childCount;

					for (int i = 0; i < childCount; i++) {
						if (transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).GetChild (i).tag != "Coin") {
							objSelected = transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).GetChild (i).gameObject;
							break;
						}
					}

					float posy = TransformToScrollbar (objSelected.transform.position.y);
					obstacleScrollbar.GetComponent<Scrollbar> ().value = posy;
				}
				else {
					objSelected = null;
				}
			}
			else {
				coinSelected = null;
				objSelected = null;
			}
		}
	}

	public void SelectPrefab(string obj) {
		if (obj.Contains ("Obstacle")) {
			string[] temp = obj.Split ('_');
			int i;
			Int32.TryParse (temp [1], out i);
			int j;
			Int32.TryParse (temp [2], out j);
			objNumber = i + (j / 10f);
		}
		else
			objNumber = -1;

		string path = "Prefabs/" + currentClimate.ToString() + "/" + obj;
		GameObject newPrefab = (GameObject)Resources.Load (path);

		if (prefabSelected != null) {
			if (prefabSelected == newPrefab) {
				prefabSelected = null;
			}
			else {
				prefabSelected = newPrefab;
			}
		}
		else {
			prefabSelected = newPrefab;
		}
	}

	public void ChangeClimate() {
		currentClimate++;
		if (currentClimate.ToString() == "3")
			currentClimate = 0;
		
		climateButton.transform.GetChild(0).GetComponent<Text> ().text = currentClimate.ToString();

		ClearTiles ();

		string clim = currentClimate.ToString ();
		int childs = obstacleScrollview.transform.GetChild (0).GetChild (0).childCount;
		for (int i = 0; i < childs; i++) {
			if (obstacleScrollview.transform.GetChild (0).GetChild (0).GetChild (i).name.Contains (clim)) {
				obstacleScrollview.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (true);
			}
			else
				obstacleScrollview.transform.GetChild (0).GetChild (0).GetChild (i).gameObject.SetActive (false);
		}

		obstacleScrollview.transform.GetChild (0).GetChild (0).GetChild (0).gameObject.SetActive (true);
	}

	public void ButtonSelected(GameObject button) {
		if (buttonSelected != null) {
			if (buttonSelected == button) {
				button.GetComponent<Image> ().color = Color.white;
				buttonSelected = null;
			}
			else {
				buttonSelected.GetComponent<Image> ().color = Color.white;
				buttonSelected = button;
				button.GetComponent<Image> ().color = new Color (.5f, .5f, .5f, 1f);
			}
		}
		else {
			buttonSelected = button;
			button.GetComponent<Image> ().color = new Color (.5f, .5f, .5f, 1f);
		}

		if (anyTileSelected) {
			Color c1 = transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color;
			transform.GetChild ((int)tileSelected.x).GetChild ((int)tileSelected.y).gameObject.GetComponent<SpriteRenderer> ().color = new Color (c1.r * 2f, c1.g * 2f, c1.b * 2f, 1f);
			tileSelected = new Vector2 (-1, -1);
			coinSelected = null;
			objSelected = null;
		}
	}

	private float TransformToScrollbar(float value) {
		int i = (int)(value * 100f);

		switch (i) {
			case 100:
				value = 0f;
				break;
			case 250:
				value = 0.14f;
				break;
			case 350:
				value = 0.28f;
				break;
			case 450:
				value = 0.43f;
				break;
			case 550:
				value = 0.56f;
				break;
			case 650:
				value = 0.7f;
				break;
			case 750:
				value = 0.86f;
				break;
			case 850:
				value = 1f;
				break;
		}

		return value;
	}

	private float ScrollbarToTransform(float value) {
		int i = (int)(value * 100f);

		switch (i) {
			case 0:
				value = 0f;
				break;
			case 14:
				value = 2.5f;
				break;
			case 28:
				value = 3.5f;
				break;
			case 43:
				value = 4.5f;
				break;
			case 56:
				value = 5.5f;
				break;
			case 70:
				value = 6.5f;
				break;
			case 86:
				value = 7.5f;
				break;
			case 100:
				value = 8.5f;
				break;
		}

		return value;
	}
}

public class ScrollStruct {
	public int id;
	public string fileName;
	public DateTime creationTime;

	public ScrollStruct(int id, string fileName, DateTime creationTime) {
		this.id = id;
		this.fileName = fileName;
		this.creationTime = creationTime;
	}
}

public class ScrollComprarer : IComparer<ScrollStruct> {

	public int Compare(ScrollStruct x, ScrollStruct y) {
		return DateTime.Compare (x.creationTime, y.creationTime);
	}
}

#endif