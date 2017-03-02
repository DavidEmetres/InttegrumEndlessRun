using UnityEngine;
using System.Collections;

public class TileInput : MonoBehaviour {

	public Vector2 tile;

	private void Update() {
		if (!ObstacleEditor.Instance.scrollview.activeInHierarchy) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, Mathf.Infinity, ObstacleEditor.Instance.editorRayCast)) {
				if (hit.transform.gameObject == this.gameObject) {
					if (Input.GetMouseButtonDown (0))
						ObstacleEditor.Instance.SelectTile (tile);
				}
			}
		}
	}

	public void ClearTile() {
		for (int i = 0; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
		}

		GetComponent<SpriteRenderer> ().color = Color.white;
	}
}
