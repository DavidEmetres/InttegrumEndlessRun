using UnityEngine;
using System.Collections;

public class TileInput : MonoBehaviour {

	public Vector2 tile;

	private void Update() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, Mathf.Infinity, ObstacleEditor.Instance.editorRayCast)) {
			if (hit.transform.gameObject == this.gameObject) {
				if(Input.GetMouseButtonDown(0))
					ObstacleEditor.Instance.SelectTile (tile);
			}
		}
	}
}
