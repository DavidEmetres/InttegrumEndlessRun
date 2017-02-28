using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ObstacleEditor : MonoBehaviour {

	private Vector4[,] tileArray = new Vector4[10, 3];
	private Vector2 tileSelected = new Vector2 (-1, -1);
	private bool anyTileSelected = false;
	private GameObject prefabSelected;
	private int objNumber;
	private GameObject buttonSelected;
	private GameObject coinSelected;
	private GameObject objSelected;
	private bool rotatingCam;
	private float camRotDirection;

	public LayerMask editorRayCast;
	public Color tileOcuppiedColor;
	public GameObject coinScrollbar;
	public GameObject obstacleScrollbar;
	public GameObject deleteCoinButton;
	public GameObject deleteObstacleButton;
	public Transform cameraRotPivot;

	public static ObstacleEditor Instance;

	private void Awake() {
		Instance = this;
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
			coinSelected.transform.position = new Vector3 (coinSelected.transform.position.x, y, coinSelected.transform.position.z);
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
		}
		else {
			obstacleScrollbar.SetActive (false);
			deleteObstacleButton.SetActive (false);
		}

		//ROTATE CAMERA;
		if(rotatingCam)
			Camera.main.transform.RotateAround (cameraRotPivot.position, Vector3.up, 2f * camRotDirection);
	}

	public void SaveStructure() {
		string path = Application.dataPath + "/Resources/TileArrays/";
		string[] lines = new string[10];

		for (int i = 0; i < tileArray.GetLength (0); i++) {
			lines [i] = tileArray [i, 0].x + "," + tileArray [i, 0].y + "," + tileArray [i, 0].z + "," + tileArray [i, 0].w + "|" +
			tileArray [i, 1].x + "," + tileArray [i, 1].y + "," + tileArray [i, 1].z + "," + tileArray [i, 1].w + "|" +
			tileArray [i, 2].x + "," + tileArray [i, 2].y + "," + tileArray [i, 2].z + "," + tileArray [i, 2].w;
		}

		path += "TileArray" + Guid.NewGuid ().ToString ("N");
		Debug.Log (path);
		System.IO.File.WriteAllLines (path + ".txt", lines);
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
			if ((objNumber == 1 && tileArray [(int)tile.x, (int)tile.y].z == 0) || (objNumber != 1 && tileArray [(int)tile.x, (int)tile.y].x == 0)) {
				Vector3 pos = transform.GetChild ((int)tile.x).GetChild ((int)tile.y).position;
				pos = new Vector3 (pos.x, 1f, pos.z);
				GameObject obj = Instantiate (prefabSelected, pos, prefabSelected.transform.rotation) as GameObject;

				if (obj.GetComponent<Displacement> () != null)
					obj.GetComponent<Displacement> ().enabled = false;
		
				obj.transform.SetParent (transform.GetChild ((int)tile.x).GetChild ((int)tile.y));

				transform.GetChild ((int)tile.x).GetChild ((int)tile.y).gameObject.GetComponent<SpriteRenderer> ().color = tileOcuppiedColor;

				if (objNumber == 1) {
					if (tileArray [(int)tile.x, (int)tile.y].x != 0 && tileArray [(int)tile.x, (int)tile.y].y == 1f) {
						pos = new Vector3 (pos.x, 2.5f, pos.z);
						obj.transform.position = pos;
					}

					tileArray [(int)tile.x, (int)tile.y].z = objNumber;
					tileArray [(int)tile.x, (int)tile.y].w = pos.y;
				}
				else {
					if (tileArray [(int)tile.x, (int)tile.y].z != 0 && tileArray [(int)tile.x, (int)tile.y].w == 1f) {
						tileArray [(int)tile.x, (int)tile.y].w = 2.5f;

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

	public void SelectPrefab(GameObject obj) {
		if (prefabSelected != null) {
			if (prefabSelected == obj) {
				prefabSelected = null;
			}
			else {
				prefabSelected = obj;
			}
		}
		else {
			prefabSelected = obj;
		}

		switch (obj.tag) {
			case "Coin":
				objNumber = 1;
				break;
			case "Obstacle":
				objNumber = 2;
				break;
		}
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
				value = 0.25f;
				break;
			case 350:
				value = 0.5f;
				break;
			case 450:
				value = 0.75f;
				break;
			case 500:
				value = 1f;
				break;
		}

		return value;
	}

	private float ScrollbarToTransform(float value) {
		int i = (int)(value * 100f);

		switch (i) {
			case 0:
				value = 1f;
				break;
			case 25:
				value = 2.5f;
				break;
			case 50:
				value = 3.5f;
				break;
			case 75:
				value = 4.5f;
				break;
			case 100:
				value = 5f;
				break;
		}

		return value;
	}

	private void UpdateVisuals() {

	}

	private void OnGUI() {
		GUI.Label(new Rect(10, 10, 500, 20), "Tile Selected: " + tileSelected);
		GUI.Label(new Rect(10, 70, 500, 20), "Prefabs Selected: " + prefabSelected);
		GUI.Label(new Rect(10, 140, 500, 20), "Coin Selected: " + coinSelected);
		GUI.Label(new Rect(10, 210, 500, 20), "Object Selected: " + objSelected);
	}
}
