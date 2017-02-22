using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadChange {

	private List<Neighbours> neighbours = new List<Neighbours>();

	public RoadChange(Province currentProvince, Direction displacementDirection, float distance) {
		int i;
		Neighbours northN;
		Neighbours southN;
		Neighbours eastN;
		Neighbours westN;

		Neighbours nullN = new Neighbours (false);

		switch (displacementDirection) {
		case Direction.north:
			if (currentProvince.westNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.westNeighbours.Length);
				westN = currentProvince.westNeighbours [i];
				neighbours.Add (westN);
			}
			else {
				neighbours.Add (nullN);
			}
			if (currentProvince.northNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.northNeighbours.Length);
				northN = currentProvince.northNeighbours [i];
				neighbours.Add (northN);
			}
			else {
				neighbours.Add (nullN);
			}
			if (currentProvince.eastNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.eastNeighbours.Length);
				eastN = currentProvince.eastNeighbours [i];
				neighbours.Add (eastN);
			}
			else {
				neighbours.Add (nullN);
			}
				break;

		case Direction.south:
			if (currentProvince.eastNeighbours != null) {
				i = Random.Range (0, currentProvince.eastNeighbours.Length);
				eastN = currentProvince.eastNeighbours [i];
				neighbours.Add (eastN);
			}
			else {
				neighbours.Add (nullN);
			}
			if (currentProvince.southNeighbours != null) {
				i = Random.Range (0, currentProvince.southNeighbours.Length);
				southN = currentProvince.southNeighbours [i];
				neighbours.Add (southN);
			}
			else {
				neighbours.Add (nullN);
			}
			if (currentProvince.westNeighbours != null) {
				i = Random.Range (0, currentProvince.westNeighbours.Length);
				westN = currentProvince.westNeighbours [i];
				neighbours.Add (westN);
			}
			else {
				neighbours.Add (nullN);
			}
				break;

		case Direction.east:
			if (currentProvince.northNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.northNeighbours.Length);
				northN = currentProvince.northNeighbours [i];
				neighbours.Add (northN);
			}
			else {
				neighbours.Add (nullN);
			}
			if (currentProvince.eastNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.eastNeighbours.Length);
				eastN = currentProvince.eastNeighbours [i];
				neighbours.Add (eastN);
			}
			else {
				neighbours.Add (nullN);
			}
			if (currentProvince.southNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.southNeighbours.Length);
				southN = currentProvince.southNeighbours [i];
				neighbours.Add (southN);
			}
			else {
				neighbours.Add (nullN);
			}
				break;

		case Direction.west:
			if (currentProvince.southNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.southNeighbours.Length);
				southN = currentProvince.southNeighbours [i];
				neighbours.Add (southN);
			}
			else {
				neighbours.Add (nullN);
			}
			if (currentProvince.westNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.westNeighbours.Length);
				westN = currentProvince.westNeighbours [i];
				neighbours.Add (westN);
			}
			else {
				neighbours.Add (nullN);
			}
			if (currentProvince.northNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.northNeighbours.Length);
				northN = currentProvince.northNeighbours [i];
				neighbours.Add (northN);
			}
			else {
				neighbours.Add (nullN);
			}
				break;
		}

		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (180f, 0f, 0f);

		Vector3 pos = new Vector3 (0f, 0f, distance);
		GameObject obj = MonoBehaviour.Instantiate(Resources.Load ("Prefabs/RoadChange"), pos, rot) as GameObject;
		obj.GetComponent<RoadChangeBehaviour> ().Initialize (this);

		string s = "";
		for(int j = 0; j < neighbours.Count; j++) {
			if (!neighbours [j].nullNeighbour)
				s += neighbours [j].neighbourProvince.name + " ";
			else
				s += "null ";
		}
		Debug.Log (s);
	}

	public Neighbours GetNeighbour(int lane) {
		return neighbours [lane];
	}
}
