using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerationManager : MonoBehaviour {

	private TileArrayManager tileManager;
	private GameObject terrain;
	private Vector3[] lanes;
	private float timer;
	private bool lane0Empty;
	private bool lane1Empty;
	private bool lane2Empty;
	private float meshStartDistance;
	private bool forceNextTile;
	private int nextTile;

	[HideInInspector] public float tileCount;
	public float defaultSpeed;
	[HideInInspector] public float displacementSpeed;
	[HideInInspector] public float generationDistance;
	[HideInInspector] public float destroyDistance;
	[HideInInspector] public bool changingRoad;
	public Transform defaultBonificationParent;
	public Transform defaultObstacleParent;
	public Transform defaultEnvironmentParent;
	[HideInInspector] public Transform bonificationParent;
	[HideInInspector] public Transform obstacleParent;
	[HideInInspector] public Transform environmentParent;
	[HideInInspector] public float tileSize;
	[HideInInspector] public bool selectedRoad;
	[HideInInspector] public int laneSelected;
	public float percentageRandomGeneration;

	public static GenerationManager Instance;

	private void Awake() {
		Instance = this;

		tileManager = new TileArrayManager ();
	}

	private void Start () {
		lanes = SceneManager.Instance.lanes;
		generationDistance = 200f;
		destroyDistance = -30f;
		displacementSpeed = defaultSpeed;
		tileSize = 5f;
		tileCount = tileSize;
		selectedRoad = false;
		bonificationParent = defaultBonificationParent;
		obstacleParent = defaultObstacleParent;
		environmentParent = defaultEnvironmentParent;

		BuildTerrainMesh (0f);
	}
	
	private void Update () {
		if(terrain != null)
			UpdateMesh ();

		if (tileCount >= tileSize) {
			GenerateTile ();
			EnvironmentGenerator.Instance.GenerateEnvironment (-1f, null, null);
		}

		if (SceneManager.Instance.provinceKm >= 2f && !selectedRoad) {
			selectedRoad = true;
			CreateRoadChange();
		}
	}

	public void ForceNextTile(int nextTile) {
		this.nextTile = nextTile;
		forceNextTile = true;
	}

	private void GenerateTile() {
		tileCount = 0;
		Mesh mesh = terrain.GetComponent<MeshFilter> ().mesh;

		float pos = mesh.vertices [mesh.vertices.Length - 1].z + meshStartDistance;

		float porc = Random.Range (0f, 101f);
		if (porc >= 0f && porc <= percentageRandomGeneration) {
			GameObject obsParent = new GameObject ("ObstaclesTiles");
			obsParent.AddComponent<ParentDestroy> ();
			GameObject bonParent = new GameObject ("BonificationTiles");
			bonParent.AddComponent<ParentDestroy> ();
			obsParent.transform.parent = obstacleParent;
			bonParent.transform.parent = bonificationParent;

			if (forceNextTile) {
				forceNextTile = false;
				tileManager.GetSpecificTileArray (nextTile).GenerateTiles (pos, obsParent.transform, bonParent.transform);
			}
			else
				tileManager.CreateRandomTileArray ().GenerateTiles (pos, obsParent.transform, bonParent.transform);
		}
		else {
			GameObject obsParent = new GameObject ("ObstaclesTiles");
			obsParent.AddComponent<ParentDestroy> ();
			GameObject bonParent = new GameObject ("BonificationTiles");
			bonParent.AddComponent<ParentDestroy> ();
			obsParent.transform.parent = obstacleParent;
			bonParent.transform.parent = bonificationParent;

			if (forceNextTile) {
				forceNextTile = false;
				tileManager.GetSpecificTileArray (nextTile).GenerateTiles (pos, obsParent.transform, bonParent.transform);
			}
			else
				tileManager.GetRandomTileArray ().GenerateTiles (pos, obsParent.transform, bonParent.transform);
		}
	}

	public void ChangeDisplacementSpeed(float newSpeed, bool returnToDefault) {
		if (returnToDefault) {
			displacementSpeed = defaultSpeed;
		}
		else {
			displacementSpeed = newSpeed;
		}
	}

	private void UpdateMesh() {
		Mesh mesh = terrain.GetComponent<MeshFilter> ().mesh;
		Mesh newMesh = new Mesh ();
		newMesh.Clear ();

		Vector3[] vertices = new Vector3[mesh.vertexCount];
		int[] triangles = new int[mesh.triangles.Length];
		Vector2[] uv = new Vector2[mesh.uv.Length];

		vertices = mesh.vertices;
		uv = mesh.uv;

		for (int j = 0; j < mesh.vertexCount; j++) {
			vertices [j] = new Vector3 (vertices [j].x, vertices [j].y, vertices [j].z - displacementSpeed * Time.deltaTime);

			if (!changingRoad) {
				if (vertices [j].z < (destroyDistance - meshStartDistance)) {
					for (int i = 0; i < mesh.vertexCount; i += 2) {
						if (i < mesh.vertexCount - 2) {
							vertices [i].Set (mesh.vertices [i + 2].x, mesh.vertices [i + 2].y, mesh.vertices [i + 2].z - displacementSpeed * Time.deltaTime);
							vertices [i + 1].Set (mesh.vertices [i + 3].x, mesh.vertices [i + 3].y, mesh.vertices [i + 3].z - displacementSpeed * Time.deltaTime);
							uv [i] = (uv [i] == Vector2.zero) ? new Vector2 (0, 1) : Vector2.zero;
							uv [i + 1] = (uv [i + 1] == Vector2.one) ? new Vector2 (1, 0) : Vector2.one;
						}
						else {
							vertices [i] = new Vector3 (-5f, 0f, vertices [i - 1].z + 10f);
							vertices [i + 1] = new Vector3 (5f, 0f, vertices [i - 1].z + 10f);
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
		terrain.GetComponent<MeshFilter> ().mesh.Clear ();
		terrain.GetComponent<MeshFilter> ().mesh = newMesh;
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);
	}

	public void ChangeTerrainMat() {
		string matPath = "3D/Materials/" + SceneManager.Instance.currentProvince.climate.ToString () + "/TerrainMat";
		terrain.GetComponent<MeshRenderer>().material = (Material)Resources.Load(matPath);
	}

	private void CreateRoadChange() {
		tileCount = 0;
		changingRoad = true;

		Mesh mesh = terrain.GetComponent<MeshFilter> ().mesh;
		List<Vector3> vertices = new List<Vector3> (mesh.vertices);
		vertices.RemoveAt (vertices.Count - 1);
		vertices.RemoveAt (vertices.Count - 1);

		float pos = vertices [vertices.Count - 1].z + meshStartDistance;

		List<Vector2> uv = new List<Vector2> (mesh.uv);
		uv.RemoveAt (uv.Count - 1);
		uv.RemoveAt(uv.Count - 1);

		List<int> triangles = new List<int> (mesh.triangles);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);

		mesh.Clear ();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray ();
		mesh.uv = uv.ToArray();
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);

		RoadChange rc = new RoadChange (SceneManager.Instance.currentProvince, SceneManager.Instance.displacementDirection, pos);

		//FRONT ROAD OBSTACLES;
		GameObject frontObsParent = new GameObject("FrontRoadObstacleTiles");
		frontObsParent.AddComponent<ParentDestroy> ();
		GameObject frontBonParent = new GameObject ("FrontRoadBonificationTiles");
		frontBonParent.AddComponent<ParentDestroy> ();
		frontObsParent.transform.parent = obstacleParent;
		frontBonParent.transform.parent = bonificationParent;
		tileManager.GetRandomTileArray ().GenerateTiles (pos + 50f, frontObsParent.transform, frontBonParent.transform);
//		EnvironmentGenerator.Instance.GenerateEnvironment (pos + 50f, null, null);
		tileManager.GetRandomTileArray ().GenerateTiles (pos + 100f, frontObsParent.transform, frontBonParent.transform);
//		EnvironmentGenerator.Instance.GenerateEnvironment (pos + 100f, null, null);
//		tileManager.GetRandomTileArray ().GenerateTiles (pos + 150f, frontObsParent.transform, frontBonParent.transform);
//		EnvironmentGenerator.Instance.GenerateEnvironment (pos + 150f, null, null);

		//RIGHT ROAD OBSTACLES;
		GameObject roadchange = GameObject.Find ("RoadChange(Clone)");
		GameObject rightObsParent = new GameObject("RightRoadObstacleTiles");
		rightObsParent.AddComponent<ParentDestroy> ();
		GameObject rightBonParent = new GameObject ("RightRoadBonificationTiles");
		rightBonParent.AddComponent<ParentDestroy> ();
		GameObject rightLEnviroParent = new GameObject ("RightRoadLeftEnvironmentTiles");
		rightLEnviroParent.AddComponent<ParentDestroy> ();
		GameObject rightREnviroParent = new GameObject ("RightRoadRightEnvironmentTiles");
		rightREnviroParent.AddComponent<ParentDestroy> ();
		rightObsParent.transform.parent = roadchange.transform;
		rightBonParent.transform.parent = roadchange.transform;
		rightLEnviroParent.transform.parent = roadchange.transform;
		rightREnviroParent.transform.parent = roadchange.transform;
		tileManager.GetRandomTileArray ().GenerateTiles (pos + 50f, rightObsParent.transform, rightBonParent.transform);
//		EnvironmentGenerator.Instance.GenerateEnvironment (pos + 50f, rightLEnviroParent, rightREnviroParent);
		tileManager.GetRandomTileArray ().GenerateTiles (pos + 100f, rightObsParent.transform, rightBonParent.transform);
//		EnvironmentGenerator.Instance.GenerateEnvironment (pos + 100f, rightLEnviroParent, rightREnviroParent);
		tileManager.GetRandomTileArray ().GenerateTiles (pos + 150f, rightObsParent.transform, rightBonParent.transform);
//		EnvironmentGenerator.Instance.GenerateEnvironment (pos + 150f, rightLEnviroParent, rightREnviroParent);
		rightObsParent.transform.RotateAround (rc.GetCenter ().position, Vector3.up, 90f);
		rightBonParent.transform.RotateAround (rc.GetCenter ().position, Vector3.up, 90f);
		rightLEnviroParent.transform.RotateAround (rc.GetCenter ().position, Vector3.up, 90f);
		rightREnviroParent.transform.RotateAround (rc.GetCenter ().position, Vector3.up, 90f);
		rightObsParent.transform.parent = obstacleParent;
		rightBonParent.transform.parent = bonificationParent;
		rightLEnviroParent.transform.parent = environmentParent;
		rightREnviroParent.transform.parent = environmentParent;

		//LEFT ROAD OBSTACLES;
		GameObject leftObsParent = new GameObject("LeftRoadObstacleTiles");
		leftObsParent.AddComponent<ParentDestroy> ();
		GameObject leftBonParent = new GameObject ("LeftRoadBonificationTiles");
		leftBonParent.AddComponent<ParentDestroy> ();
		GameObject leftLEnviroParent = new GameObject ("LeftRoadLeftEnvironmentTiles");
		leftLEnviroParent.AddComponent<ParentDestroy> ();
		GameObject leftREnviroParent = new GameObject ("LeftRoadRightEnvironmentTiles");
		leftREnviroParent.AddComponent<ParentDestroy> ();
		leftObsParent.transform.parent = roadchange.transform;
		leftBonParent.transform.parent = roadchange.transform;
		leftLEnviroParent.transform.parent = roadchange.transform;
		leftREnviroParent.transform.parent = roadchange.transform;
		tileManager.GetRandomTileArray ().GenerateTiles (pos + 50f, leftObsParent.transform, leftBonParent.transform);
//		EnvironmentGenerator.Instance.GenerateEnvironment (pos + 50f, leftLEnviroParent, leftREnviroParent);
		tileManager.GetRandomTileArray ().GenerateTiles (pos + 100f, leftObsParent.transform, leftBonParent.transform);
//		EnvironmentGenerator.Instance.GenerateEnvironment (pos + 100f, leftLEnviroParent, leftREnviroParent);
		tileManager.GetRandomTileArray ().GenerateTiles (pos + 150f, leftObsParent.transform, leftBonParent.transform);
//		EnvironmentGenerator.Instance.GenerateEnvironment (pos + 150f, leftLEnviroParent, leftREnviroParent);
		leftObsParent.transform.RotateAround (rc.GetCenter ().position, Vector3.up, -90f);
		leftBonParent.transform.RotateAround (rc.GetCenter ().position, Vector3.up, -90f);
		leftLEnviroParent.transform.RotateAround (rc.GetCenter ().position, Vector3.up, -90f);
		leftREnviroParent.transform.RotateAround (rc.GetCenter ().position, Vector3.up, -90f);
		leftObsParent.transform.parent = obstacleParent;
		leftBonParent.transform.parent = bonificationParent;
		leftLEnviroParent.transform.parent = environmentParent;
		leftREnviroParent.transform.parent = environmentParent;

		GameObject[] obsParent = new GameObject[3] { leftObsParent, frontObsParent, rightObsParent };
		GameObject[] bonParent = new GameObject[3] { leftBonParent, frontBonParent, rightBonParent };
		StartCoroutine(LoadRoadChangeAssets(obsParent, bonParent, pos, 0));
	}

	private IEnumerator LoadRoadChangeAssets(GameObject[] obsParent, GameObject[] bonParent, float pos, int count) {
		while (changingRoad)
			yield return null;

		Transform parentObs = obsParent [laneSelected].transform;
		Transform parentBon = bonParent [laneSelected].transform;

		switch (laneSelected) {
			case 0:
				EnvironmentGenerator.Instance.GenerateEnvironment (150f, null, null);
				tileManager.GetRandomTileArray ().GenerateTiles (200f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (200f, null, null);
				tileManager.GetRandomTileArray ().GenerateTiles (250f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (250f, null, null);
				tileManager.GetRandomTileArray ().GenerateTiles (300f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (300f, null, null);
				break;
			case 1:
				tileManager.GetRandomTileArray ().GenerateTiles (150f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (150f, null, null);
				tileManager.GetRandomTileArray ().GenerateTiles (200f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (200f, null, null);
				tileManager.GetRandomTileArray ().GenerateTiles (250f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (250f, null, null);
				tileManager.GetRandomTileArray ().GenerateTiles (300f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (300f, null, null);
				break;
			case 2:
				EnvironmentGenerator.Instance.GenerateEnvironment (150f, null, null);
				tileManager.GetRandomTileArray ().GenerateTiles (200f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (200f, null, null);
				tileManager.GetRandomTileArray ().GenerateTiles (250f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (250f, null, null);
				tileManager.GetRandomTileArray ().GenerateTiles (300f, parentObs, parentBon);
				EnvironmentGenerator.Instance.GenerateEnvironment (300f, null, null);
				break;
		}
	}

	public void ChangeObsBonParent(Transform obsParent, Transform bonParent, bool returnToDefault) {
		if (!returnToDefault) {
			obstacleParent = obsParent;
			bonificationParent = bonParent;
		}
		else {
			obstacleParent = defaultObstacleParent;
			bonificationParent = defaultBonificationParent;
		}
	}

	public void CreateProvinceChange() {
		changingRoad = true;

		Mesh mesh = terrain.GetComponent<MeshFilter> ().mesh;
		List<Vector3> vertices = new List<Vector3> (mesh.vertices);
		vertices.RemoveAt (vertices.Count - 1);
		vertices.RemoveAt (vertices.Count - 1);

		float pos = vertices [vertices.Count - 1].z + meshStartDistance;

		List<Vector2> uv = new List<Vector2> (mesh.uv);
		uv.RemoveAt (uv.Count - 1);
		uv.RemoveAt(uv.Count - 1);

		List<int> triangles = new List<int> (mesh.triangles);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);

		mesh.Clear ();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray ();
		mesh.uv = uv.ToArray();
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);

		ProvinceChange pc = new ProvinceChange (pos);
	}

	public void DestroyTerrainMesh() {
		Destroy (terrain);
	}

	public void BuildTerrainMesh(float startDistance) {
		terrain = new GameObject ("Terrain");
		terrain.layer = LayerMask.NameToLayer("Walkable");
		terrain.transform.SetParent (transform.GetChild (0));
		terrain.AddComponent<MeshFilter> ();
		terrain.AddComponent<MeshRenderer> ();
		terrain.AddComponent<BoxCollider> ().size = new Vector3(10f, 0f, 1f);
		terrain.GetComponent<BoxCollider> ().center = new Vector3(0f, 0f, 0f - startDistance);

		Mesh mesh = new Mesh();
		mesh.Clear ();

		int numTiles = (int)(generationDistance / 10f);
		Vector3[] tempVertices = new Vector3[(int)((numTiles+1) * 2)];
		Vector2[] tempUv = new Vector2[tempVertices.Length];
		int[] tempTriangles = new int[(int)((numTiles * 2) * 3)];
		int triangleCount = 0;
		bool b = false;

		tempVertices [0] = new Vector3 (-5f, 0f, -10f);
		tempVertices [1] = new Vector3 (5f, 0f, -10f);
		tempUv [0] = new Vector2 (0, 0);
		tempUv [1] = new Vector2 (1, 0);

		for (int i = 1; i <= numTiles; i++) {
			tempVertices [2 * i] = new Vector3 (-5f, 0f, -10f + (i * 10));
			tempVertices [(2 * i) + 1] = new Vector3 (5f, 0f, -10f + (i * 10));

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

		string matPath = "3D/Materials/" + SceneManager.Instance.currentProvince.climate.ToString () + "/TerrainMat";
		terrain.GetComponent<MeshRenderer>().material = (Material)Resources.Load (matPath);

		terrain.transform.position = new Vector3 (terrain.transform.position.x, terrain.transform.position.y, startDistance);
		meshStartDistance = startDistance;
	}
}
