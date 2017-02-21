using UnityEngine;
using System.Collections;

public class Province {

	public string name;
	public Climate climate;
	public Neighbours[] northNeighbours;
	public Neighbours[] southNeighbours;
	public Neighbours[] eastNeighbours;
	public Neighbours[] westNeighbours;

	public Province(string name, Climate climate) {
		this.name = name;
		this.climate = climate;
	}

	public void SetNeighbours(Neighbours[] northNeighbours, Neighbours[] southNeighbours, Neighbours[] eastNeighbours, Neighbours[] westNeighbours) {
		this.northNeighbours = northNeighbours;
		this.southNeighbours = southNeighbours;
		this.eastNeighbours = eastNeighbours;
		this.westNeighbours = westNeighbours;
	}
}

public struct Neighbours {
	public Province neighbourProvince;
	public float distanceBetweenProvinces;
	public bool nullNeighbour;

	public Neighbours(Province p, float dist) {
		neighbourProvince = p;
		distanceBetweenProvinces = dist;
		nullNeighbour = false;
	}

	public Neighbours(bool b) {
		nullNeighbour = true;
		neighbourProvince = null;
		distanceBetweenProvinces = 0f;
	}
};

public enum Climate {
	Mediterranean,
	Continental,
	Oceanic
}
