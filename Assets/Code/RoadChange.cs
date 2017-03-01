using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadChange {

	private List<Neighbours> neighbours = new List<Neighbours>();
	private List<Direction> directions = new List<Direction>();
	private RoadChangeBehaviour rcb;

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
				directions.Add (Direction.west);
			}
			else {
				neighbours.Add (nullN);
				directions.Add (Direction.west);
			}
			if (currentProvince.northNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.northNeighbours.Length);
				northN = currentProvince.northNeighbours [i];
				neighbours.Add (northN);
				directions.Add (Direction.north);
			}
			else {
				neighbours.Add (nullN);
				directions.Add (Direction.north);
			}
			if (currentProvince.eastNeighbours.Length > 0) {
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
			if (currentProvince.northNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.northNeighbours.Length);
				northN = currentProvince.northNeighbours [i];
				neighbours.Add (northN);
				directions.Add (Direction.north);
			}
			else {
				neighbours.Add (nullN);
				directions.Add (Direction.north);
			}
			if (currentProvince.eastNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.eastNeighbours.Length);
				eastN = currentProvince.eastNeighbours [i];
				neighbours.Add (eastN);
				directions.Add (Direction.east);
			}
			else {
				neighbours.Add (nullN);
				directions.Add (Direction.east);
			}
			if (currentProvince.southNeighbours.Length > 0) {
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
			if (currentProvince.southNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.southNeighbours.Length);
				southN = currentProvince.southNeighbours [i];
				neighbours.Add (southN);
				directions.Add (Direction.south);
			}
			else {
				neighbours.Add (nullN);
				directions.Add (Direction.south);
			}
			if (currentProvince.westNeighbours.Length > 0) {
				i = Random.Range (0, currentProvince.westNeighbours.Length);
				westN = currentProvince.westNeighbours [i];
				neighbours.Add (westN);
				directions.Add (Direction.west);
			}
			else {
				neighbours.Add (nullN);
				directions.Add (Direction.west);
			}
			if (currentProvince.northNeighbours.Length > 0) {
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

		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (180f, 0f, 0f);

		Vector3 pos = new Vector3 (0f, 0f, distance);

		GameObject obj = MonoBehaviour.Instantiate(Resources.Load ("Prefabs/RoadChange"), pos, rot) as GameObject;
		rcb = obj.GetComponent<RoadChangeBehaviour> ();
		rcb.Initialize (this);

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

	public Direction GetNewDirection(int lane) {
		return directions [lane];
	}

	public Transform GetCenter() {
		return rcb.centerPivot;
	}

	public void SetLeftRightObstacles(GameObject leftObs, GameObject rightObs) {
		rcb.leftRoadObs = leftObs;
		rcb.rightRoadObs = rightObs;
	}
}
