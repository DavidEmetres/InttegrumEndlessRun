﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileArray {

	private Vector4[,] tiles = new Vector4[10, 3];
	public string tileArrayName;
	public int nextTileArray = -1;
	public bool forceNextTile;
	public string nextTileName;

	public TileArray(Vector4[,] t, string tileArrayName, bool forceNextTile, string nextTileName) {
		tiles = t;
		this.tileArrayName = tileArrayName;
		this.forceNextTile = forceNextTile;
		this.nextTileName = nextTileName;
	}

	public List<GameObject> GenerateTiles(float distance, Transform obsParent, Transform bonParent) {
		return GenerateTiles (distance, obsParent, bonParent, true);
	}

	public List<GameObject> GenerateTiles(float distance, Transform obsParent, Transform bonParent, bool displaceActive) {
		Vector3 pos = Vector3.zero;
		Quaternion rot = Quaternion.identity;
		List<GameObject> objList = new List<GameObject> ();

		for (int i = 0; i < tiles.GetLength(0); i++) {
			for (int j = 0; j < tiles.GetLength(1); j++) {
				Vector4 info = (Vector4)tiles.GetValue (i, j);

				string t = info.x.ToString ();

				if (t != "0") {
					string[] temp = t.Split ('.');
					pos = new Vector3 (SceneManager.Instance.lanes [2-j].x, info.y, distance + (GenerationManager.Instance.tileSize * i));
					Tile tile1 = new Tile (Int32.Parse(temp[0]), Int32.Parse(temp[1]), pos, Quaternion.identity, obsParent, displaceActive);
					objList.Add (tile1.obj);
				}

				t = info.z.ToString ();

				if (t == "-1") {
					pos = new Vector3 (SceneManager.Instance.lanes [2-j].x, info.w, distance + (GenerationManager.Instance.tileSize * i));
					rot.eulerAngles = new Vector3 (90f, 0f, 0f);
					Tile tile2 = new Tile (-1, 0, pos, rot, bonParent, displaceActive);
					objList.Add (tile2.obj);
				}
			}
		}

		return objList;
	}

	public void SetNextTileIndex(int tileIndex) {
		nextTileArray = tileIndex;
	}
}