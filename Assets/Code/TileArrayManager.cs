using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileArrayManager {

	private List<TileArray> tileArrays = new List<TileArray>();

	public TileArrayManager() {

//		TileArray t1 = new TileArray (new int[,] {
//			{ 0, 1, 0 },
//			{ 0, 1, 0 },
//			{ 0, 1, 0 },
//			{ 0, 1, 0 },
//			{ 0, 1, 0 },
//			{ 1, 0, 1 },
//			{ 1, 0, 1 },
//			{ 1, 0, 1 },
//			{ 1, 0, 1 },
//			{ 1, 0, 1 }
//		});

		TileArray t1 = new TileArray (new Vector4[,] {
			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 0, 0), 		new Vector4 (0, 0, 1, 5f) },
			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 0, 0), 		new Vector4 (0, 0, 1, 5f) },
			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 5f) },
			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 2.5f), 	new Vector4 (0, 0, 1, 5f) },
			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 3.5f), 	new Vector4 (0, 0, 1, 3.5f) },
			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 5f), 		new Vector4 (0, 0, 1, 2.5f) },
			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 3.5f), 	new Vector4 (0, 0, 1, 1.5f) },
			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 2.5f), 	new Vector4 (0, 0, 1, 1.5f) },
			{ new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 1.5f), 	new Vector4 (0, 0, 1, 1.5f) },
		});

		tileArrays.Add (t1);
	}

	public TileArray GetRandomTileArray() {
		int i = Random.Range (0, tileArrays.Count);
		return tileArrays [i];
	}
}
