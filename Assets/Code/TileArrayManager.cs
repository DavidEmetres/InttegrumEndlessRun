using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class TileArrayManager {

	private List<TileArray> tileArrays = new List<TileArray>();
	private List<Vector4> vectorList = new List<Vector4> ();
	private int count = 0;
	private int[] allObstacles = new int[] { 0, 2 };
	private int[] allBonifications = new int[] { 0, 1 };
	private float[] allHeights = new float[] { 1f, 2.5f, 3.5f, 4.5f, 5f };

	public TileArrayManager() {
		string[] namesArray = new string[] { "t1", "t2", "t3" };
		IDictionary<string, TileArray> d = new Dictionary<string, TileArray> ();

		string path = "TileArrays/";
		string[] allTileArrays = Directory.GetFiles (Application.dataPath + "/Resources/" + path, "*.txt", SearchOption.TopDirectoryOnly);
		string[] temp;
		string fileName;
		string line;

		for(int i = 0; i < allTileArrays.Length; i++) {

			temp = allTileArrays [i].Split ('/');
			fileName = temp [temp.Length - 1].Substring(0, temp[temp.Length - 1].Length - 4);
			path = "TileArrays/" + fileName;

			TextAsset txt = (TextAsset)Resources.Load (path);

			if (txt != null) {
				StreamReader reader = new StreamReader (new MemoryStream (txt.bytes));

				using (reader) {
					do {
						line = reader.ReadLine();

						if(line != null) {
							string[] vectors = line.Split('|');

							if(vectors.Length > 0) {
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
						}
					}

					while(line != null);

					reader.Close();
				}
				 
				d.Add(namesArray[i], new TileArray(new Vector4[,] {
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
				}));

				vectorList.Clear ();
			}
		}

		foreach (TileArray ta in d.Values) {
			tileArrays.Add (ta);
		}
	}

	public TileArray GetRandomTileArray() {
//		string temp = Guid.NewGuid ().ToString ("N");
//		string seed = "";
//
//		foreach (char c in temp) {
//			seed += Convert.ToInt32(c);
//		}
//
//		seed = seed.Substring (0, seed.Length / 10);
//		Debug.Log (seed);
//		int l = Convert.ToInt32 (seed);
//		UnityEngine.Random.InitState (l);
//		Debug.Log (l);
//		int i = UnityEngine.Random.Range (0, tileArrays.Count);
//		Debug.Log (i + "/" + tileArrays.Count);

		int i = UnityEngine.Random.Range (0, tileArrays.Count);
		return tileArrays [i];
	}

	public TileArray CreateRandomTileArray() {
		List<Vector4> vectorList = new List<Vector4> ();
		int obsCount = 0;
		int previousObs = 0;

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
				if (vectorList [i - 3].x != 0) {
					obs = 0;
					obsH = 0f;
				}
				else {
					obsCount++;
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
				if (vectorList [i - 3].x != 0) {
					obs = 0;
					obsH = 0f;
				}
				else {
					obsCount++;
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
				if (vectorList [i - 3].x != 0) {
					obs = 0;
					obsH = 0f;
				}
				else {
					obsCount++;
				}
			}

			j = UnityEngine.Random.Range (0, allBonifications.Length);
			bon = allBonifications [j];
			bonH = 1f;

			Vector4 v3 = new Vector4 (obs, obsH, bon, bonH);

			vectorList.Add (v1);
			vectorList.Add (v2);
			vectorList.Add (v3);

			if (obsCount > 2) {
				j = UnityEngine.Random.Range (0, 3);
				vectorList [i + j] = new Vector4 (0f, 0f, vectorList [i + j].z, vectorList [i + j].w);
			}

			obsCount = 0;
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
		});

		return t;
	}
}
