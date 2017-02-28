using UnityEngine;
using System.Collections;

public class TileArray {

	private Vector4[,] tiles = new Vector4[10, 3];

	public TileArray(Vector4[,] t) {
		tiles = t;
	}

	public void GenerateTiles(float distance) {
		string path;
		Vector3 pos = Vector3.zero;
		Quaternion rot = Quaternion.identity;

		for (int i = 0; i < tiles.GetLength(0); i++) {
			for (int j = 0; j < tiles.GetLength(1); j++) {
				Vector4 info = (Vector4)tiles.GetValue (i, j);

				//OBSTACLES SWITCH;
				switch ((int)info.x) {
				case 2:
					path = "Prefabs/" + SceneManager.Instance.currentProvince.climate + "/Obstacle";
					pos = new Vector3 (SceneManager.Instance.lanes [j].x, info.y, distance + (GenerationManager.Instance.tileSize * i));
					Tile tile = new Tile (path, pos, rot, GenerationManager.Instance.obstacleParent);
					break;
				}

				//COINS SWITCH;
				switch ((int)info.z) {
				case 1:
					path = "Prefabs/Bonification";
					pos = new Vector3 (SceneManager.Instance.lanes [j].x, info.w, distance + (GenerationManager.Instance.tileSize * i));
					rot.eulerAngles = new Vector3 (90f, 0f, 0f);
					Tile tile = new Tile (path, pos, rot, GenerationManager.Instance.bonificationParent);
					break;
				}
			}
		}
	}
}
