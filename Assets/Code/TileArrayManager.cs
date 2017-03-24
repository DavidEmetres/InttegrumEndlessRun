using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

public class TileArrayManager {

	private List<TileArray> oceanicTileArrays = new List<TileArray>();
	private int[] oceanicTypes = new int[] { 2, 1, 1, 1 };
	private List<TileArray> continentalTileArrays = new List<TileArray>();
	private int[] continentalTypes = new int[] { 2, 1, 1, 1 };
	private List<TileArray> mediterraneanTileArrays = new List<TileArray>();
	private int[] mediterraneanTypes = new int[] { 2, 1, 1, 1 };

	private List<Vector4> vectorList = new List<Vector4> ();
	private int count = 0;
	private int[] allObstacles = new int[] { 0, 2 };
	private int[] allBonifications = new int[] { 0, 1 };
	private float[] allHeights = new float[] { 1f, 2.5f, 3.5f, 4.5f, 5f };

	public TileArrayManager() {
		IDictionary<string, TileArray> d = new Dictionary<string, TileArray> ();

		string path = "TileArrays/";
//		string[] allTileArrays;

		TextAsset[] allTileArrays = (Resources.LoadAll (path, typeof(TextAsset))).Cast<TextAsset> ().ToArray ();
//		allTileArrays = Directory.GetFiles (Application.dataPath + "/Resources/" + path, "*.txt", SearchOption.TopDirectoryOnly);

		string[] temp;
		string fileName;
		string line;
		string type;

		for(int i = 0; i < allTileArrays.Length; i++) {

//			temp = allTileArrays [i].Split ('/');
//			fileName = temp [temp.Length - 1].Substring(0, temp[temp.Length - 1].Length - 4);
//			path = "TileArrays/" + fileName;

//			TextAsset txt = (TextAsset)Resources.Load (path);

			TextAsset txt = allTileArrays[i];
			fileName = txt.name;
			Debug.Log (fileName);

			if (txt != null) {
				StreamReader reader = new StreamReader (new MemoryStream (txt.bytes));
				bool force = false;
				string nextTileName = "";

				using (reader) {
					do {
						line = reader.ReadLine();

						if(line != null) {
							string[] vectors = line.Split('|');

							if(vectors.Length > 2) {
								foreach(string v in vectors) {
									string[] tile = v.Split(',');

									float n1, n2, n3, n4;
									bool b = float.TryParse(tile[0], out n1);
									b = float.TryParse(tile[1], out n2);
									b = float.TryParse(tile[2], out n3);
									b = float.TryParse(tile[3], out n4);
									Vector4 vector = new Vector4(n1, n2, n3, n4);
									vectorList.Add(vector);
								}
							}
							else if(vectors.Length > 1) {
								force = (vectors[0] == "TRUE ")? true : false;
								nextTileName = vectors[1];
							}
						}
					}

					while(line != null);

					reader.Close();
				}
				 
				d.Add(fileName, new TileArray(new Vector4[,] {
					{vectorList[0], vectorList[1], vectorList[2]},
					{vectorList[3], vectorList[4], vectorList[5]},
					{vectorList[6], vectorList[7], vectorList[8]},
					{vectorList[9], vectorList[10], vectorList[11]},
					{vectorList[12], vectorList[13], vectorList[14]},
					{vectorList[15], vectorList[16], vectorList[17]},
					{vectorList[18], vectorList[19], vectorList[20]},
					{vectorList[21], vectorList[22], vectorList[23]},
					{vectorList[24], vectorList[25], vectorList[26]},
					{vectorList[27], vectorList[28], vectorList[29]},
				}, fileName, force, nextTileName));
				vectorList.Clear ();
			}
		}

		foreach (KeyValuePair<string, TileArray> value in d) {
			string[] t = value.Key.Split ('_');

			switch (t[0]) {
				case "Oceanic":
					oceanicTileArrays.Add (value.Value);
					break;
				case "Continental":
					continentalTileArrays.Add (value.Value);
					break;
				case "Mediterranean":
					mediterraneanTileArrays.Add (value.Value);
					break;
			}
		}

		foreach (TileArray ta in oceanicTileArrays) {
			if (ta.forceNextTile) {
				for (int i = 0; i < oceanicTileArrays.Count; i++) {
					if (oceanicTileArrays [i].tileArrayName.Trim() == ta.nextTileName.Trim()) {
						ta.SetNextTileIndex (i);
					}
				}
			}
		}
	}

	public TileArray GetRandomTileArray() {
		int i;

		switch (SceneManager.Instance.currentProvince.climate) {
			case Climate.Oceanic:
				i = UnityEngine.Random.Range (0, oceanicTileArrays.Count);

				if (oceanicTileArrays[i].forceNextTile)
					GenerationManager.Instance.ForceNextTile (oceanicTileArrays[i].nextTileArray);
				
				return oceanicTileArrays [i];
				break;
			case Climate.Continental:
				i = UnityEngine.Random.Range (0, continentalTileArrays.Count);
				return continentalTileArrays [i];
				break;
			case Climate.Mediterranean:
				i = UnityEngine.Random.Range (0, mediterraneanTileArrays.Count);
				return mediterraneanTileArrays [i];
				break;
		}

		return null;
	}

	public TileArray GetSpecificTileArray(int index) {
		switch (SceneManager.Instance.currentProvince.climate) {
			case Climate.Oceanic:
				return oceanicTileArrays [index];
				break;
			case Climate.Continental:
				return continentalTileArrays [index];
				break;
			case Climate.Mediterranean:
				return mediterraneanTileArrays [index];
				break;
		}

		return oceanicTileArrays [index];
	}

	public TileArray CreateRandomTileArray() {
		List<Vector4> vectorList = new List<Vector4> ();

		int j;
		int obs;
		float obsH;
		int bon;
		float bonH;
		int h;

		for (int i = 0; i < 30; i = i + 3) {
			//FIRST OBSTACLE & BONIFICATIONS;
			j = UnityEngine.Random.Range (0, allObstacles.Length);
			obs = allObstacles [j];
			obsH = 1f;

			if (obs != 0 && i != 0) {
				if (i - 3 >= 0) {
					if (vectorList [i - 3].x != 0) {
						obs = 0;
						obsH = 0f;
					}
				}

				if (i - 6 >= 0) {
					if (vectorList [i - 6].x != 0) {
						obs = 0;
						obsH = 0f;
					}
				}

				if (i - 9 >= 0) {
					if (vectorList [i - 9].x != 0) {
						obs = 0;
						obsH = 0f;
					}
				}

				if (i >= 24) {
					obs = 0;
					obsH = 0f;
				}
			}

			j = UnityEngine.Random.Range (0, allBonifications.Length);
			bon = allBonifications [j];
			bonH = 1f;

			Vector4 v1 = new Vector4 (obs, obsH, bon, bonH);

			//SECOND OBSTACLE & BONIFICATIONS;
			j = UnityEngine.Random.Range (0, allObstacles.Length);
			obs = allObstacles [j];
			obsH = 1f;

			if (obs != 0 && i != 0) {
				if (i - 3 >= 0) {
					if (vectorList [i - 3].x != 0) {
						obs = 0;
						obsH = 0f;
					}
				}

				if (i - 6 >= 0) {
					if (vectorList [i - 6].x != 0) {
						obs = 0;
						obsH = 0f;
					}
				}

				if (i - 9 >= 0) {
					if (vectorList [i - 9].x != 0) {
						obs = 0;
						obsH = 0f;
					}
				}

				if (i >= 24) {
					obs = 0;
					obsH = 0f;
				}
			}

			j = UnityEngine.Random.Range (0, allBonifications.Length);
			bon = allBonifications [j];
			bonH = 1f;

			Vector4 v2 = new Vector4 (obs, obsH, bon, bonH);

			//THIRD OBSTACLE & BONIFICATIONS;
			j = UnityEngine.Random.Range (0, allObstacles.Length);
			obs = allObstacles [j];
			obsH = 1f;

			if (obs != 0 && i != 0) {
				if (i - 3 >= 0) {
					if (vectorList [i - 3].x != 0) {
						obs = 0;
						obsH = 0f;
					}
				}

				if (i - 6 >= 0) {
					if (vectorList [i - 6].x != 0) {
						obs = 0;
						obsH = 0f;
					}
				}

				if (i - 9 >= 0) {
					if (vectorList [i - 9].x != 0) {
						obs = 0;
						obsH = 0f;
					}
				}

				if (i >= 24) {
					obs = 0;
					obsH = 0f;
				}
			}

			j = UnityEngine.Random.Range (0, allBonifications.Length);
			bon = allBonifications [j];
			bonH = 1f;

			Vector4 v3 = new Vector4 (obs, obsH, bon, bonH);

			vectorList.Add (v1);
			vectorList.Add (v2);
			vectorList.Add (v3);

			//RANDOM GENERATION PARAMETERS: 
			//1. NOT GENERATE IF THERE IS ANY OBSTACLE IN THREE ROWS AHEAD;
			//2. GENERATE ALL COINS AT HEIGHT 1f;
			//3. NOT GENERATE IN THE LAST MATRIX POSITIONS;
		}

		TileArray t = new TileArray (new Vector4[,] {
			{ vectorList [0], vectorList [1], vectorList [2] },
			{ vectorList [3], vectorList [4], vectorList [5] },
			{ vectorList [6], vectorList [7], vectorList [8] },
			{ vectorList [9], vectorList [10], vectorList [11] },
			{ vectorList [12], vectorList [13], vectorList [14] },
			{ vectorList [15], vectorList [16], vectorList [17] },
			{ vectorList [18], vectorList [19], vectorList [20] },
			{ vectorList [21], vectorList [22], vectorList [23] },
			{ vectorList [24], vectorList [25], vectorList [26] },
			{ vectorList [27], vectorList [28], vectorList [29] },
		}, "RandomTileArray", false, "");

		return t;
	}
}
