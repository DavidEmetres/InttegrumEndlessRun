using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TileArrayManager {

	private List<TileArray> tileArrays = new List<TileArray>();
	private List<Vector4> vectorList = new List<Vector4> ();

	public TileArrayManager() {
		string path = Application.dataPath + "/Resources/TileArrays/";

		string line;

		StreamReader reader = new StreamReader(path + "TileArray.txt", true);

		using(reader) {
			do {
				line = reader.ReadLine();

				if(line != null) {
					string[] vectors = line.Split('|');

					if(vectors.Length > 0) {
						foreach(string v in vectors) {
							string[] tile = v.Split(',');

							int n1, n2, n3, n4;
							bool b = int.TryParse(tile[0], out n1);
							b = int.TryParse(tile[1], out n2);
							b = int.TryParse(tile[2], out n3);
							b = int.TryParse(tile[3], out n4);
							Vector4 vector = new Vector4(n1, n2, n3, n4);
							vectorList.Add(vector);
						}
					}
				}
			}

			while(line != null);

			reader.Close();
		}

		TileArray t = new TileArray(new Vector4[,] {
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
		});

//		TileArray t1 = new TileArray (new Vector4[,] {
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 0, 0), 		new Vector4 (0, 0, 1, 5f) },
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 0, 0), 		new Vector4 (0, 0, 1, 5f) },
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 5f) },
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 2.5f), 	new Vector4 (0, 0, 1, 5f) },
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 3.5f), 	new Vector4 (0, 0, 1, 3.5f) },
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 5f), 		new Vector4 (0, 0, 1, 2.5f) },
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 3.5f), 	new Vector4 (0, 0, 1, 1.5f) },
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 2.5f), 	new Vector4 (0, 0, 1, 1.5f) },
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 1.5f) },
//			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 1.5f) },
//		});

		tileArrays.Add (t);
	}

	public TileArray GetRandomTileArray() {
		int i = Random.Range (0, tileArrays.Count);
		return tileArrays [i];
	}
}
