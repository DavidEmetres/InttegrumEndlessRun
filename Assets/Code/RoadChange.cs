using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadChange {

	private List<Neighbours> neighbours = new List<Neighbours>();
	private List<Direction> directions = new List<Direction>();
	private RoadChangeBehaviour rcb;

	public RoadChange(GameObject roadChangePrefab, GameObject sign, float distance) {
		neighbours = sign.GetComponent<RoadChangeSign> ().neighbours;
		directions = sign.GetComponent<RoadChangeSign> ().directions;

		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (180f, 0f, 0f);

		Vector3 pos = new Vector3 (0f, 0.01f, distance);

		roadChangePrefab.SetActive (true);
		roadChangePrefab.transform.rotation = rot;
		roadChangePrefab.transform.position = pos;

		for (int i = 0; i < neighbours.Count; i++) {
			if (neighbours [i].nullNeighbour)
				roadChangePrefab.transform.GetChild (4 - i).gameObject.SetActive (true);
			else if(roadChangePrefab.transform.GetChild(4 - i).gameObject.activeInHierarchy)
				roadChangePrefab.transform.GetChild (4 - i).gameObject.SetActive (false);
		}

		roadChangePrefab.AddComponent<RoadChangeBehaviour> ();
		rcb = roadChangePrefab.GetComponent<RoadChangeBehaviour> ();
		rcb.Initialize (this);
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
}
