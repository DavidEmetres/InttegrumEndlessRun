using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadChange {

	private List<Neighbours> neighbours = new List<Neighbours>();

	public RoadChange(Province currentProvince, Direction displacementDirection) {
		int i;
		Neighbours northN;
		Neighbours southN;
		Neighbours eastN;
		Neighbours westN;

		switch (displacementDirection) {
			case Direction.north:
				i = Random.Range (0, currentProvince.westNeighbours.Length);
				westN = currentProvince.westNeighbours [i];
				neighbours.Add (westN);

				i = Random.Range (0, currentProvince.northNeighbours.Length);
				northN = currentProvince.northNeighbours [i];
				neighbours.Add (northN);

				i = Random.Range (0, currentProvince.eastNeighbours.Length);
				eastN = currentProvince.eastNeighbours [i];
				neighbours.Add (eastN);
				break;

			case Direction.south:
				i = Random.Range (0, currentProvince.eastNeighbours.Length);
				eastN = currentProvince.eastNeighbours [i];
				neighbours.Add (eastN);

				i = Random.Range (0, currentProvince.southNeighbours.Length);
				southN = currentProvince.southNeighbours [i];
				neighbours.Add (southN);

				i = Random.Range (0, currentProvince.westNeighbours.Length);
				westN = currentProvince.westNeighbours [i];
				neighbours.Add (westN);
				break;

			case Direction.east:
				i = Random.Range (0, currentProvince.northNeighbours.Length);
				northN = currentProvince.northNeighbours [i];
				neighbours.Add (northN);

				i = Random.Range (0, currentProvince.eastNeighbours.Length);
				eastN = currentProvince.eastNeighbours [i];
				neighbours.Add (eastN);

				i = Random.Range (0, currentProvince.southNeighbours.Length);
				southN = currentProvince.southNeighbours [i];
				neighbours.Add (southN);
				break;

			case Direction.west:
				i = Random.Range (0, currentProvince.southNeighbours.Length);
				southN = currentProvince.southNeighbours [i];
				neighbours.Add (southN);

				i = Random.Range (0, currentProvince.westNeighbours.Length);
				westN = currentProvince.westNeighbours [i];
				neighbours.Add (westN);

				i = Random.Range (0, currentProvince.northNeighbours.Length);
				northN = currentProvince.northNeighbours [i];
				neighbours.Add (northN);
				break;
		}
	}

	public Neighbours GetNeighbour(int lane) {
		return neighbours [lane];
	}
}
