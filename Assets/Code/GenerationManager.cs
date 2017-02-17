using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerationManager : MonoBehaviour {

	private GameObject terrain;
	private Vector3[] lanes;
	private float timer;
	private bool lane0Empty;
	private bool lane1Empty;
	private bool lane2Empty;

	[HideInInspector] public float generationDistance;
	[HideInInspector] public float destroyDistance;

	public static GenerationManager Instance;

	private void Awake() {
		Instance = this;
	}

	private void Start () {
		lanes = SceneManager.Instance.lanes;
		generationDistance = 100f;
		destroyDistance = -10f;

		BuildTerrainMesh ();
		InvokeRepeating ("UpdateMesh", 0f, 1f);
	}
	
	private void Update () {
		
	}

	private void UpdateMesh() {
		Mesh mesh = terrain.GetComponent<MeshFilter> ().mesh;

		List<Vector3> newVertices = new List<Vector3>();
		newVertices.AddRange (mesh.vertices);
		newVertices.Add (new Vector3 (-5f, 0f, generationDistance));
		newVertices.Add (new Vector3 (5f, 0f, generationDistance));

		List<Vector2> newUv = new List<Vector2> ();
		newUv.AddRange (mesh.uv);
		newUv.Add (new Vector2 (0, 0));
		newUv.Add (new Vector2 (1, 0));

		int lastIndex = newVertices.Count - 1;

		List<int> newTriangles = new List<int> ();
		newTriangles.AddRange (mesh.triangles);
		newTriangles.Add (lastIndex - 2);
		newTriangles.Add (lastIndex - 3);
		newTriangles.Add (lastIndex - 1);

		newTriangles.Add (lastIndex - 1);
		newTriangles.Add (lastIndex);
		newTriangles.Add (lastIndex - 2);

		mesh.Clear ();

		Mesh m = new Mesh ();
		m.SetVertices (newVertices);
		m.SetUVs (0, newUv);
		m.SetTriangles (newTriangles, 0);
		terrain.GetComponent<MeshFilter> ().mesh = m;
	}

	private void BuildTerrainMesh() {
		terrain = new GameObject ("Terrain");
		terrain.transform.SetParent (transform.GetChild (0));
		terrain.AddComponent<MeshFilter> ();
		terrain.AddComponent<MeshRenderer> ();

		Mesh mesh = new Mesh();
		mesh.Clear ();

		int numTiles = (int)(generationDistance / 10f);
		Vector3[] tempVertices = new Vector3[(int)((numTiles+1) * 2)];
		Vector2[] tempUv = new Vector2[tempVertices.Length];
		int[] tempTriangles = new int[(int)((numTiles * 2) * 3)];
		int triangleCount = 0;
		bool b = false;

		tempVertices [0] = new Vector3 (-5f, 0f, generationDistance);
		tempVertices [1] = new Vector3 (5f, 0f, generationDistance);
		tempUv [0] = new Vector2 (0, 1);
		tempUv [1] = new Vector2 (1, 1);

		for (int i = 1; i <= numTiles; i++) {
			tempVertices [2 * i] = new Vector3 (-5f, 0f, -20 + (i * 10));
			tempVertices [(2 * i) + 1] = new Vector3 (5f, 0f, -20 + (i * 10));

			tempTriangles [triangleCount] = (2 * i) - 2;
			tempTriangles [triangleCount + 1] = (2 * i) - 1;
			tempTriangles [triangleCount + 2] = (2 * i);

			tempTriangles [triangleCount + 3] = (2 * i) - 1;
			tempTriangles [triangleCount + 4] = (2 * i) + 1;
			tempTriangles [triangleCount + 5] = (2 * i);

			triangleCount += 6;

			if (!b) {
				tempUv [2 * i] = new Vector2 (0, 0);
				tempUv [(2 * i) + 1] = new Vector2 (1, 0);
				b = true;
			}
			else {
				tempUv [2 * i] = new Vector2 (0, 1);
				tempUv [(2 * i) + 1] = new Vector2 (1, 1);
				b = false;
			}
		}

		mesh.vertices = tempVertices;
		mesh.triangles = tempTriangles;
		mesh.uv = tempUv;
		terrain.GetComponent<MeshFilter> ().mesh = mesh;
		terrain.AddComponent<Displacement> ();
	}
}
