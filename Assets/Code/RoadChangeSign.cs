using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoadChangeSign : MonoBehaviour {

	public List<Neighbours> neighbours = new List<Neighbours>();
	public List<Direction> directions = new List<Direction>();

	public void Initialize(float distance) {
		Direction displacementDirection = SceneManager.Instance.displacementDirection;
		Province currentProvince = SceneManager.Instance.currentProvince;
		int i;
		Neighbours northN;
		Neighbours southN;
		Neighbours eastN;
		Neighbours westN;

		Neighbours nullN = new Neighbours (false);

		neighbours.Clear ();

		switch (displacementDirection) {
			case Direction.north:
				if (currentProvince.westNeighbours != null) {
					i = Random.Range (0, currentProvince.westNeighbours.Length);
					westN = currentProvince.westNeighbours [i];
					neighbours.Add (westN);
					directions.Add (Direction.west);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.west);
				}
				if (currentProvince.northNeighbours != null) {
					i = Random.Range (0, currentProvince.northNeighbours.Length);
					northN = currentProvince.northNeighbours [i];
					neighbours.Add (northN);
					directions.Add (Direction.north);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.north);
				}
				if (currentProvince.eastNeighbours != null) {
					i = Random.Range (0, currentProvince.eastNeighbours.Length);
					eastN = currentProvince.eastNeighbours [i];
					neighbours.Add (eastN);
					directions.Add (Direction.east);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.east);
				}
				break;

			case Direction.south:
				if (currentProvince.eastNeighbours != null) {
					i = Random.Range (0, currentProvince.eastNeighbours.Length);
					eastN = currentProvince.eastNeighbours [i];
					neighbours.Add (eastN);
					directions.Add (Direction.east);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.east);
				}
				if (currentProvince.southNeighbours != null) {
					i = Random.Range (0, currentProvince.southNeighbours.Length);
					southN = currentProvince.southNeighbours [i];
					neighbours.Add (southN);
					directions.Add (Direction.south);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.south);
				}
				if (currentProvince.westNeighbours != null) {
					i = Random.Range (0, currentProvince.westNeighbours.Length);
					westN = currentProvince.westNeighbours [i];
					neighbours.Add (westN);
					directions.Add (Direction.west);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.west);
				}
				break;

			case Direction.east:
				if (currentProvince.northNeighbours != null) {
					i = Random.Range (0, currentProvince.northNeighbours.Length);
					northN = currentProvince.northNeighbours [i];
					neighbours.Add (northN);
					directions.Add (Direction.north);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.north);
				}
				if (currentProvince.eastNeighbours != null) {
					i = Random.Range (0, currentProvince.eastNeighbours.Length);
					eastN = currentProvince.eastNeighbours [i];
					neighbours.Add (eastN);
					directions.Add (Direction.east);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.east);
				}
				if (currentProvince.southNeighbours != null) {
					i = Random.Range (0, currentProvince.southNeighbours.Length);
					southN = currentProvince.southNeighbours [i];
					neighbours.Add (southN);
					directions.Add (Direction.south);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.south);
				}
				break;

			case Direction.west:
				if (currentProvince.southNeighbours != null) {
					i = Random.Range (0, currentProvince.southNeighbours.Length);
					southN = currentProvince.southNeighbours [i];
					neighbours.Add (southN);
					directions.Add (Direction.south);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.south);
				}
				if (currentProvince.westNeighbours != null) {
					i = Random.Range (0, currentProvince.westNeighbours.Length);
					westN = currentProvince.westNeighbours [i];
					neighbours.Add (westN);
					directions.Add (Direction.west);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.west);
				}
				if (currentProvince.northNeighbours != null) {
					i = Random.Range (0, currentProvince.northNeighbours.Length);
					northN = currentProvince.northNeighbours [i];
					neighbours.Add (northN);
					directions.Add (Direction.north);
				}
				else {
					neighbours.Add (nullN);
					directions.Add (Direction.north);
				}
				break;
		}

		Vector3 pos = new Vector3 (3f, 0f, distance + 10f);

		gameObject.SetActive (true);
		transform.position = pos;

		for(int j = 0; j < neighbours.Count; j++) {
			if (!neighbours [j].nullNeighbour) {
				transform.GetChild (0).GetChild (j).GetComponent<Text> ().text = neighbours [j].neighbourProvince.name;
				transform.GetChild (1).GetChild (j).gameObject.SetActive (false);
				Debug.Log (neighbours [j].neighbourProvince.name);
			}
			else {
				transform.GetChild (0).GetChild (j).GetComponent<Text> ().text = "";
				transform.GetChild (1).GetChild (j).gameObject.SetActive (true);
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			string[] n = new string[3];
			string[] d = new string[3];
			for (int i = 0; i < 3; i++) {
				if (!neighbours [i].nullNeighbour) {
					n [i] = neighbours [i].neighbourProvince.name;
					d [i] = neighbours [i].distanceBetweenProvinces.ToString("F1");
				}
				else {
					n [i] = "null";
					d [i] = "null";
				}
			}
			HUDManager.Instance.ShowRoadChangeHUD (n, d);
		}
	}
}
