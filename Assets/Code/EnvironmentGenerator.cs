using UnityEngine;
using System.Collections;

public class EnvironmentGenerator : MonoBehaviour {

	private float meshStartDistance;
	private float tileSize;
	private float tileCount;
	private int enviroCount;

	[HideInInspector] public GameObject leftTerrain;
	[HideInInspector] public GameObject rightTerrain;
	public Transform environmentParent;
	public int maxEnviroCount;

	public static EnvironmentGenerator Instance;

	private void Awake() {
		Instance = this;
	}

	private void Start () {
		leftTerrain = BuildTerrainMesh (0f, false);
		rightTerrain = BuildTerrainMesh (0f, true);
		tileSize = GenerationManager.Instance.tileSize;
	}
	
	private void Update () {
		if (leftTerrain != null)
			UpdateMesh (leftTerrain, false);
		if (rightTerrain != null)
			UpdateMesh (rightTerrain, true);
	}

	public void GenerateEnvironment() {
		GameObject leftParent = new GameObject ("LeftEnvironment");
		leftParent.AddComponent<ParentDestroy> ();
		leftParent.transform.parent = environmentParent;
		GameObject rightParent = new GameObject ("RightEnvironment");
		rightParent.AddComponent<ParentDestroy> ();
		rightParent.transform.parent = environmentParent;

		float pos = leftTerrain.GetComponent<MeshFilter>().mesh.vertices [leftTerrain.GetComponent<MeshFilter>().mesh.vertices.Length - 1].z + meshStartDistance;
		string path = "Prefabs/" + SceneManager.Instance.currentProvince.climate.ToString () + "/Enviro" + enviroCount;

		GameObject leftEnviro = Instantiate (Resources.Load (path), Vector3.zero, Quaternion.identity) as GameObject;
		leftEnviro.transform.position = new Vector3 (0f, 0f, pos);
		leftEnviro.transform.parent = leftParent.transform;
		GameObject rightEnviro = Instantiate (Resources.Load (path), Vector3.zero, Quaternion.identity) as GameObject;
		rightEnviro.transform.position = new Vector3 (0f, 0f, pos);
		rightEnviro.transform.localScale = new Vector3 (-1f, 1f, 1f);
		rightEnviro.transform.parent = rightParent.transform;

		enviroCount++;
		if (enviroCount > maxEnviroCount)
			enviroCount = 0;
	}

	private void UpdateMesh(GameObject obj, bool right) {
		Mesh mesh = obj.GetComponent<MeshFilter> ().mesh;
		Mesh newMesh = new Mesh ();
		newMesh.Clear ();

		float maxLeft = (right) ? 5f : -45f;
		float maxRight = (right) ? 45f : -5f;

		Vector3[] vertices = new Vector3[mesh.vertexCount];
		int[] triangles = new int[mesh.triangles.Length];
		Vector2[] uv = new Vector2[mesh.uv.Length];

		vertices = mesh.vertices;
		uv = mesh.uv;

		for (int j = 0; j < mesh.vertexCount; j++) {
			vertices [j] = new Vector3 (vertices [j].x, vertices [j].y, vertices [j].z - GenerationManager.Instance.displacementSpeed * Time.deltaTime);

			if (!GenerationManager.Instance.changingRoad) {
				if (vertices [j].z < (GenerationManager.Instance.destroyDistance - meshStartDistance)) {
					for (int i = 0; i < mesh.vertexCount; i += 2) {
						if (i < mesh.vertexCount - 2) {
							vertices [i].Set (mesh.vertices [i + 2].x, mesh.vertices [i + 2].y, mesh.vertices [i + 2].z - GenerationManager.Instance.displacementSpeed * Time.deltaTime);
							vertices [i + 1].Set (mesh.vertices [i + 3].x, mesh.vertices [i + 3].y, mesh.vertices [i + 3].z - GenerationManager.Instance.displacementSpeed * Time.deltaTime);
							uv [i] = (uv [i] == Vector2.zero) ? new Vector2 (0, 1) : Vector2.zero;
							uv [i + 1] = (uv [i + 1] == Vector2.one) ? new Vector2 (1, 0) : Vector2.one;
						}
						else {
							vertices [i] = new Vector3 (maxLeft, 0f, vertices [i - 1].z + 10f);
							vertices [i + 1] = new Vector3 (maxRight, 0f, vertices [i - 1].z + 10f);
							tileCount++;
						}
					}

					break;
				}
			}
		}

		triangles = mesh.triangles;

		newMesh.vertices = vertices;
		newMesh.triangles = triangles;
		newMesh.uv = uv;
		newMesh.RecalculateBounds ();
		newMesh.RecalculateNormals ();
		obj.GetComponent<MeshFilter> ().mesh.Clear ();
		obj.GetComponent<MeshFilter> ().mesh = newMesh;
		obj.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);
	}

	public void DestroyTerrainMeshes() {
		Destroy (leftTerrain);
		Destroy (rightTerrain);
	}

	public GameObject BuildTerrainMesh(float startDistance, bool right) {
		string name = (right) ? "RightTerrain" : "LeftTerrain";

		GameObject terrain = new GameObject ();
		terrain = new GameObject (name);
		terrain.layer = LayerMask.NameToLayer("Walkable");
		terrain.transform.SetParent (transform.GetChild (0));
		terrain.AddComponent<MeshFilter> ();
		terrain.AddComponent<MeshRenderer> ();

		float maxLeft = (right) ? 5f : -45f;
		float maxRight = (right) ? 45f : -5f;

		Mesh mesh = new Mesh();
		mesh.Clear ();

		int numTiles = (int)(GenerationManager.Instance.generationDistance / 10f);
		Vector3[] tempVertices = new Vector3[(int)((numTiles+1) * 2)];
		Vector2[] tempUv = new Vector2[tempVertices.Length];
		int[] tempTriangles = new int[(int)((numTiles * 2) * 3)];
		int triangleCount = 0;
		bool b = false;

		tempVertices [0] = new Vector3 (maxLeft, 0f, -10f);
		tempVertices [1] = new Vector3 (maxRight, 0f, -10f);
		tempUv [0] = new Vector2 (0, 0);
		tempUv [1] = new Vector2 (1, 0);

		for (int i = 1; i <= numTiles; i++) {
			tempVertices [2 * i] = new Vector3 (maxLeft, 0f, -10f + (i * 10));
			tempVertices [(2 * i) + 1] = new Vector3 (maxRight, 0f, -10f + (i * 10));

			tempTriangles [triangleCount] = (2 * i) - 2;
			tempTriangles [triangleCount + 1] = (2 * i) + 1;
			tempTriangles [triangleCount + 2] = (2 * i) - 1;

			tempTriangles [triangleCount + 3] = (2 * i) - 2;
			tempTriangles [triangleCount + 4] = (2 * i);
			tempTriangles [triangleCount + 5] = (2 * i) + 1;

			triangleCount += 6;

			if (!b) {
				tempUv [2 * i] = new Vector2 (0, 1);
				tempUv [(2 * i) + 1] = new Vector2 (1, 1);
				b = true;
			}
			else {
				tempUv [2 * i] = new Vector2 (0, 0);
				tempUv [(2 * i) + 1] = new Vector2 (1, 0);
				b = false;
			}
		}

		mesh.vertices = tempVertices;
		mesh.triangles = tempTriangles;
		mesh.uv = tempUv;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh = mesh;

		string matPath = "3D/Materials/" + SceneManager.Instance.currentProvince.climate.ToString () + "/EnviroMat";
		terrain.GetComponent<MeshRenderer>().material = (Material)Resources.Load(matPath);

		terrain.transform.position = new Vector3 (terrain.transform.position.x, terrain.transform.position.y, startDistance);
		meshStartDistance = startDistance;
		return terrain;
	}
}
