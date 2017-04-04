using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GenerationManager : MonoBehaviour {

	private TileArrayManager tileManager;
	private GameObject terrain;
	private GameObject leftTerrain;
	private GameObject rightTerrain;
	private float meshStartDistance;
	private bool forceNextTile;
	private int nextTile;
	private int enviroCount;

	[HideInInspector] public List<GameObject> oceanicTilesPool = new List<GameObject> ();
	[HideInInspector] public List<GameObject> oceanicEnviroPool = new List<GameObject> ();
	[HideInInspector] public List<GameObject> continentalTilesPool = new List<GameObject> ();
	[HideInInspector] public List<GameObject> continentalEnviroPool = new List<GameObject> ();
	[HideInInspector] public List<GameObject> mediterraneanTilesPool = new List<GameObject> ();
	[HideInInspector] public List<GameObject> mediterraneanEnviroPool = new List<GameObject> ();
	[HideInInspector] public List<GameObject> roadChanges = new List<GameObject> ();

	[HideInInspector] public List<GameObject> selectedTilesPool = new List<GameObject>();
	[HideInInspector] public List<GameObject> selectedEnviroPool = new List<GameObject> ();
	[HideInInspector] public GameObject selectedRoadChangePrefab;

	[HideInInspector] public float tileCount;
	[HideInInspector] public float displacementSpeed;
	[HideInInspector] public float generationDistance;
	[HideInInspector] public float destroyDistance;
	[HideInInspector] public bool changingRoad;
	[HideInInspector] public float tileSize;
	[HideInInspector] public bool selectedRoad;
	[HideInInspector] public int laneSelected;
	[HideInInspector] public bool changingProvince;
	[HideInInspector] public bool provinceChanged;
	[HideInInspector] public float generationCount;
	[HideInInspector] public int count;
	[HideInInspector] public float previousEnviroPos;
	[HideInInspector] public GameObject changingRoadStartPos;
	public float defaultSpeed;
	public Transform obstacleParent;
	public Transform environmentParent;
	public int maxEnviroCount;
	public float percentageRandomGeneration;
	public bool justEnteredRoadChange;

	public static GenerationManager Instance;

	private void Awake() {
		Instance = this;

		tileManager = new TileArrayManager ();

		GameObject rc = Instantiate ((GameObject)Resources.Load ("RoadChanges/Oceanic_RoadChange"), new Vector3 (0f, 0f, -200f), Quaternion.identity) as GameObject;
		rc.SetActive (false);
		roadChanges.Add (rc);
		//		roadChanges.Add ((GameObject)Resources.Load ("RoadChanges/Continental_RoadChange"));
		//		roadChanges.Add ((GameObject)Resources.Load ("RoadChanges/Mediterranean_RoadChange"));

		if (SceneManager.Instance.currentProvince.climate == Climate.Oceanic) {
			selectedTilesPool = oceanicTilesPool;
			selectedEnviroPool = oceanicEnviroPool;
			selectedRoadChangePrefab = roadChanges [0];
		}

		if (SceneManager.Instance.currentProvince.climate == Climate.Continental) {
			selectedTilesPool = continentalTilesPool;
			selectedEnviroPool = continentalEnviroPool;
			selectedRoadChangePrefab = roadChanges [1];
		}

		if (SceneManager.Instance.currentProvince.climate == Climate.Mediterranean) {
			selectedTilesPool = mediterraneanTilesPool;
			selectedEnviroPool = mediterraneanEnviroPool;
			selectedRoadChangePrefab = roadChanges [2];
		}

		for (int i = 0; i < 10; i++) {
			GameObject prefab = tileManager.oceanicTileArrays [0];
			GameObject obj1 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			obj1.transform.parent = obstacleParent;
			obj1.SetActive (false);
			GameObject obj2 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			obj2.transform.parent = obstacleParent;
			obj2.SetActive (false);
			GameObject obj3 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			obj3.transform.parent = obstacleParent;
			obj3.SetActive (false);

			oceanicTilesPool.Add (obj1);
			oceanicTilesPool.Add (obj2);
			oceanicTilesPool.Add (obj3);
		}

		for (int i = 0; i < 10; i++) {
			GameObject prefab = tileManager.oceanicEnviros [0];

			GameObject enviro = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			enviro.transform.parent = environmentParent;
			enviro.SetActive (false);

			GameObject enviro2 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			enviro2.transform.parent = environmentParent;
			enviro2.SetActive (false);

			GameObject enviro3 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			enviro3.transform.parent = environmentParent;
			enviro3.SetActive (false);

			oceanicEnviroPool.Add (enviro);
			oceanicEnviroPool.Add (enviro2);
			oceanicEnviroPool.Add (enviro3);
		}
	}

	private void Start () {
		generationDistance = 200f;
		destroyDistance = -30f;
		displacementSpeed = defaultSpeed;
		tileSize = 5f;
		tileCount = tileSize;
		selectedRoad = false;
		generationCount = 0f;
		previousEnviroPos = -1f;

		BuildTerrainMesh (0f);
		BuildEnviroMesh (0f, false);
		BuildEnviroMesh (0f, true);
	}

	private void Update () {
		if (terrain != null && leftTerrain != null && rightTerrain != null) {
			UpdateMesh (terrain, false);
			UpdateMesh (leftTerrain, false);
			UpdateMesh (rightTerrain, true);
		}

		Debug.Log (tileCount);

		if (SceneManager.Instance.provinceKm >= 2f && !selectedRoad && tileCount >= tileSize) {
			selectedRoad = true;
			CreateRoadChange();
		}

		if (SceneManager.Instance.provinceKm >= 6f && !provinceChanged && tileCount >= tileSize) {
			provinceChanged = true;
			CreateProvinceChange ();
		}

		if (tileCount >= tileSize && !changingRoad) {
			GenerateEnvironment ();
			GenerateTile ();
			tileCount = 0;
		}
	}

	public void ForceNextTile(int nextTile) {
		this.nextTile = nextTile;
		forceNextTile = true;
	}

	private void GenerateEnvironment() {
		if (changingProvince)
			return;

//		Mesh mesh = terrain.GetComponent<MeshFilter> ().sharedMesh;

		float pos = terrain.GetComponent<MeshFilter> ().mesh.vertices [terrain.GetComponent<MeshFilter> ().mesh.vertices.Length - 1].z + meshStartDistance;

//		GameObject leftParent = null;
//		GameObject rightParent = null;
//
//		if (parentLeft == null || parentRight == null) {
//			leftParent = new GameObject ("LeftEnvironment");
//			leftParent.AddComponent<ParentDestroy> ();
//			rightParent = new GameObject ("RightEnvironment");
//			rightParent.AddComponent<ParentDestroy> ();
//			leftParent.transform.parent = environmentParent;
//			rightParent.transform.parent = environmentParent;
//		}
//		else {
//			leftParent = parentLeft.gameObject;
//			rightParent = parentRight.gameObject;
//		}
//
//		GameObject prefab = MyResources.Instance.GetEnviro (SceneManager.Instance.currentProvince.climate, enviroCount);
//		GameObject leftEnviro = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;

		GameObject enviro = selectedEnviroPool [enviroCount];

		enviro.SetActive (true);

		enviro.transform.position = new Vector3 (0f, 0f, pos);
		enviro.transform.eulerAngles = Vector3.zero;
		enviro.transform.parent = environmentParent;
//		GameObject rightEnviro = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
//
//		if (!displaceActive) {
//			for (int i = 0; i < rightEnviro.transform.childCount; i++) {
//				rightEnviro.transform.GetChild (i).GetComponent<Displacement> ().enabled = false;
//			}
//		}

		enviroCount += 3;
		if (enviroCount >= ((maxEnviroCount * 3) - 1))
			enviroCount = 0;
	}

	private void GenerateTile() {
//		Mesh mesh = terrain.GetComponent<MeshFilter> ().sharedMesh;

		float pos = terrain.GetComponent<MeshFilter> ().mesh.vertices [terrain.GetComponent<MeshFilter> ().mesh.vertices.Length - 1].z + meshStartDistance;

//		GameObject obsParent = null;

//		if (parent == null) {
//			obsParent = new GameObject ("ObstaclesTiles");
//			obsParent.AddComponent<ParentDestroy> ();
			//		GameObject bonParent = new GameObject ("BonificationTiles");
			//		bonParent.AddComponent<ParentDestroy> ();
//			obsParent.transform.parent = obstacleParent;
			//		bonParent.transform.parent = bonificationParent;
//		}
//		else {
//			obsParent = parent.gameObject;
//		}

//		GameObject prefab = tileManager.GetRandomTileArray ();
//		GameObject obs = Instantiate (prefab, Vector3.zero, prefab.transform.rotation) as GameObject;

		GameObject obs = null;

		while (obs == null) {
			obs = selectedTilesPool [Random.Range (0, selectedTilesPool.Count)];
			if (obs.activeInHierarchy)
				obs = null;
		}

		obs.SetActive (true);

		for (int i = 0; i < obs.transform.childCount; i++) {
			obs.transform.GetChild (i).gameObject.SetActive (true);
		}

		Vector3 finalPos = new Vector3 (0f, 0f, pos);
		obs.transform.position = finalPos;
		obs.transform.eulerAngles = Vector3.zero;

//		float porc = Random.Range (0f, 101f);
//		if (porc >= 0f && porc <= percentageRandomGeneration) {
//			if (forceNextTile) {
//				forceNextTile = false;
//				tileManager.GetSpecificTileArray (nextTile).GenerateTiles (pos, obsParent.transform, bonParent.transform);
//			}
//			else
//				tileManager.CreateRandomTileArray ().GenerateTiles (pos, obsParent.transform, bonParent.transform);
//		}
//		else {
//			if (forceNextTile) {
//				forceNextTile = false;
//				tileManager.GetSpecificTileArray (nextTile).GenerateTiles (pos, obsParent.transform, bonParent.transform);
//			}
//			else
//				tileManager.GetRandomTileArray ().GenerateTiles (pos, obsParent.transform, bonParent.transform);
//		}
	}

	public void ChangeDisplacementSpeed(float newSpeed, bool returnToDefault) {
		if (returnToDefault) {
			displacementSpeed = defaultSpeed;
		}
		else {
			displacementSpeed = newSpeed;
		}
	}

	private void UpdateMesh(GameObject terrain, bool right) {
//		Mesh mesh = terrain.GetComponent<MeshFilter> ().sharedMesh;
//		Mesh newMesh = new Mesh ();
//		newMesh.Clear ();

		float maxLeft;
		float maxRight;

		if (terrain.name != "Terrain") {
			maxLeft = (right) ? 5f : -60f;
			maxRight = (right) ? 60f : -5f;
		}
		else {
			maxLeft = -5f;
			maxRight = 5f;
		}

		Vector3[] vertices = new Vector3[terrain.GetComponent<MeshFilter> ().mesh.vertexCount];
		int[] triangles = new int[terrain.GetComponent<MeshFilter> ().mesh.triangles.Length];
		Vector2[] uv = new Vector2[terrain.GetComponent<MeshFilter> ().mesh.uv.Length];

		vertices = terrain.GetComponent<MeshFilter> ().mesh.vertices;
		uv = terrain.GetComponent<MeshFilter> ().mesh.uv;

		for (int j = 0; j < terrain.GetComponent<MeshFilter> ().mesh.vertexCount; j++) {
			vertices [j] = new Vector3 (vertices [j].x, vertices [j].y, vertices [j].z - displacementSpeed * Time.deltaTime);

			if (vertices [j].z < (destroyDistance - meshStartDistance)) {
				for (int i = 0; i < terrain.GetComponent<MeshFilter> ().mesh.vertexCount; i += 2) {
					if (i < terrain.GetComponent<MeshFilter> ().mesh.vertexCount - 2) {
						vertices [i].Set (terrain.GetComponent<MeshFilter> ().mesh.vertices [i + 2].x, terrain.GetComponent<MeshFilter> ().mesh.vertices [i + 2].y, terrain.GetComponent<MeshFilter> ().mesh.vertices [i + 2].z - displacementSpeed * Time.deltaTime);
						vertices [i + 1].Set (terrain.GetComponent<MeshFilter> ().mesh.vertices [i + 3].x, terrain.GetComponent<MeshFilter> ().mesh.vertices [i + 3].y, terrain.GetComponent<MeshFilter> ().mesh.vertices [i + 3].z - displacementSpeed * Time.deltaTime);
						uv [i] = (uv [i] == Vector2.zero) ? new Vector2 (0, 1) : Vector2.zero;
						uv [i + 1] = (uv [i + 1] == Vector2.one) ? new Vector2 (1, 0) : Vector2.one;
					}
					else {
						vertices [i] = new Vector3 (maxLeft, 0f, vertices [i - 1].z + 10f);
						vertices [i + 1] = new Vector3 (maxRight, 0f, vertices [i - 1].z + 10f);
						if(right)
							tileCount++;
					}
				}

				break;
			}
		}

		triangles = terrain.GetComponent<MeshFilter> ().mesh.triangles;
//
//		newMesh.vertices = vertices;
//		newMesh.triangles = triangles;
//		newMesh.uv = uv;
//		newMesh.RecalculateBounds ();
//		newMesh.RecalculateNormals ();
//		terrain.GetComponent<MeshFilter> ().sharedMesh.Clear ();
//		terrain.GetComponent<MeshFilter> ().sharedMesh = newMesh;
//		terrain.GetComponent<MeshFilter> ().sharedMesh.UploadMeshData (false);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetVertices(vertices.ToList<Vector3>());
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetTriangles (triangles.ToList<int>(), 0);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetUVs (0, uv.ToList<Vector2>());
		terrain.GetComponent<MeshFilter> ().mesh.Clear();
		terrain.GetComponent<MeshFilter> ().mesh.vertices = vertices;
		terrain.GetComponent<MeshFilter> ().mesh.triangles = triangles;
		terrain.GetComponent<MeshFilter> ().mesh.uv = uv;
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);
	}

	public void ChangeTerrainMat() {
		Material mat = MyResources.Instance.GetMaterial (SceneManager.Instance.currentProvince.climate, true);
		terrain.GetComponent<MeshRenderer>().sharedMaterial = mat;
	}

	private void CreateRoadChange() {
		changingRoad = true;

//		Mesh mesh = terrain.GetComponent<MeshFilter> ().sharedMesh;
		List<Vector3> vertices = new List<Vector3> (terrain.GetComponent<MeshFilter> ().mesh.vertices);
		vertices.RemoveAt (vertices.Count - 1);
		vertices.RemoveAt (vertices.Count - 1);

		float pos = vertices [vertices.Count - 1].z + meshStartDistance;
		changingRoadStartPos = new GameObject ("ChangingRoadStartPos");
		changingRoadStartPos.transform.position = new Vector3 (0f, 0f, pos);
		Displacement d = changingRoadStartPos.AddComponent<Displacement> ();
		d.destroyDistance = -100f;

		List<Vector2> uv = new List<Vector2> (terrain.GetComponent<MeshFilter> ().mesh.uv);
		uv.RemoveAt (uv.Count - 1);
		uv.RemoveAt(uv.Count - 1);

		List<int> triangles = new List<int> (terrain.GetComponent<MeshFilter> ().mesh.triangles);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);

//		mesh.Clear ();
//		mesh.vertices = vertices.ToArray();
//		mesh.triangles = triangles.ToArray ();
//		mesh.uv = uv.ToArray();
//		mesh.RecalculateBounds ();
//		mesh.RecalculateNormals ();
//		terrain.GetComponent<MeshFilter> ().sharedMesh.UploadMeshData (false);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetVertices(vertices);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetTriangles (triangles, 0);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetUVs (0, uv);
		terrain.GetComponent<MeshFilter> ().mesh.Clear();
		terrain.GetComponent<MeshFilter> ().mesh.vertices = vertices.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.triangles = triangles.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.uv = uv.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);

		RoadChange rc = new RoadChange (selectedRoadChangePrefab, SceneManager.Instance.currentProvince, SceneManager.Instance.displacementDirection, pos);

		StartCoroutine (LoadRoadChangeAssets (rc));
	}

	private IEnumerator LoadRoadChangeAssets(RoadChange rc) {
		//LEFT ROAD 1;

		GameObject leftRoadEnviro1 = selectedEnviroPool [enviroCount];

		leftRoadEnviro1.SetActive (true);
		leftRoadEnviro1.transform.position = new Vector3 (0f, 0f, changingRoadStartPos.transform.position.z + 50f);
		leftRoadEnviro1.transform.eulerAngles = Vector3.zero;
		leftRoadEnviro1.transform.parent = environmentParent;

		GameObject leftRoadObs1 = null;

		while (leftRoadObs1 == null) {
			leftRoadObs1 = selectedTilesPool [Random.Range (0, selectedTilesPool.Count)];
			if (leftRoadObs1.activeInHierarchy)
				leftRoadObs1 = null;
		}

		leftRoadObs1.SetActive (true);

		for (int i = 0; i < leftRoadObs1.transform.childCount; i++) {
			leftRoadObs1.transform.GetChild (i).gameObject.SetActive (true);
		}

		leftRoadObs1.transform.position = new Vector3(0f, 0f, changingRoadStartPos.transform.position.z + 50f);
		leftRoadObs1.transform.eulerAngles = Vector3.zero;

		leftRoadEnviro1.transform.RotateAround (rc.GetCenter ().position, Vector3.up, -90f);
		leftRoadObs1.transform.RotateAround (rc.GetCenter ().position, Vector3.up, -90f);

		yield return new WaitForSeconds (0.25f);

		//FRONT ROAD 1;

		GameObject frontRoadEnviro1 = selectedEnviroPool [enviroCount + 1];

		frontRoadEnviro1.SetActive (true);
		frontRoadEnviro1.transform.position = new Vector3 (0f, 0f, changingRoadStartPos.transform.position.z + 50f);
		frontRoadEnviro1.transform.eulerAngles = Vector3.zero;
		frontRoadEnviro1.transform.parent = environmentParent;

		GameObject frontRoadObs1 = null;

		while (frontRoadObs1 == null) {
			frontRoadObs1 = selectedTilesPool [Random.Range (0, selectedTilesPool.Count)];
			if (frontRoadObs1.activeInHierarchy)
				frontRoadObs1 = null;
		}

		frontRoadObs1.SetActive (true);

		for (int i = 0; i < leftRoadObs1.transform.childCount; i++) {
			frontRoadObs1.transform.GetChild (i).gameObject.SetActive (true);
		}

		frontRoadObs1.transform.position = new Vector3(0f, 0f, changingRoadStartPos.transform.position.z + 50f);
		frontRoadObs1.transform.eulerAngles = Vector3.zero;

		//RIGHT ROAD 1;

		GameObject rightRoadEnviro1 = selectedEnviroPool [enviroCount + 2];

		rightRoadEnviro1.SetActive (true);
		rightRoadEnviro1.transform.position = new Vector3 (0f, 0f, changingRoadStartPos.transform.position.z + 50f);
		rightRoadEnviro1.transform.eulerAngles = Vector3.zero;
		rightRoadEnviro1.transform.parent = environmentParent;

		GameObject rightRoadObs1 = null;

		while (rightRoadObs1 == null) {
			rightRoadObs1 = selectedTilesPool [Random.Range (0, selectedTilesPool.Count)];
			if (rightRoadObs1.activeInHierarchy)
				rightRoadObs1 = null;
		}

		rightRoadObs1.SetActive (true);

		for (int i = 0; i < rightRoadObs1.transform.childCount; i++) {
			rightRoadObs1.transform.GetChild (i).gameObject.SetActive (true);
		}

		rightRoadObs1.transform.position = new Vector3(0f, 0f, changingRoadStartPos.transform.position.z + 50f);
		rightRoadObs1.transform.eulerAngles = Vector3.zero;

		enviroCount += 3;
		if (enviroCount >= ((maxEnviroCount * 3) - 1))
			enviroCount = 0;

		rightRoadEnviro1.transform.RotateAround (rc.GetCenter ().position, Vector3.up, 90f);
		rightRoadObs1.transform.RotateAround (rc.GetCenter ().position, Vector3.up, 90f);

		yield return new WaitForSeconds (0.25f);

		//LEFT ROAD 2;

		GameObject leftRoadEnviro2 = selectedEnviroPool [enviroCount];

		leftRoadEnviro2.SetActive (true);
		leftRoadEnviro2.transform.position = new Vector3 (0f, 0f, changingRoadStartPos.transform.position.z + 100f);
		leftRoadEnviro2.transform.eulerAngles = Vector3.zero;
		leftRoadEnviro2.transform.parent = environmentParent;

		GameObject leftRoadObs2 = null;

		while (leftRoadObs2 == null) {
			leftRoadObs2 = selectedTilesPool [Random.Range (0, selectedTilesPool.Count)];
			if (leftRoadObs2.activeInHierarchy)
				leftRoadObs2 = null;
		}

		leftRoadObs2.SetActive (true);

		for (int i = 0; i < leftRoadObs2.transform.childCount; i++) {
			leftRoadObs2.transform.GetChild (i).gameObject.SetActive (true);
		}

		leftRoadObs2.transform.position = new Vector3(0f, 0f, changingRoadStartPos.transform.position.z + 100f);
		leftRoadObs2.transform.eulerAngles = Vector3.zero;

		leftRoadEnviro2.transform.RotateAround (rc.GetCenter ().position, Vector3.up, -90f);
		leftRoadObs2.transform.RotateAround (rc.GetCenter ().position, Vector3.up, -90f);

		yield return new WaitForSeconds (0.25f);

		//FRONT ROAD 2;

		GameObject frontRoadEnviro2 = selectedEnviroPool [enviroCount + 1];

		frontRoadEnviro2.SetActive (true);
		frontRoadEnviro2.transform.position = new Vector3 (0f, 0f, changingRoadStartPos.transform.position.z + 100f);
		frontRoadEnviro2.transform.eulerAngles = Vector3.zero;
		frontRoadEnviro2.transform.parent = environmentParent;

		GameObject frontRoadObs2 = null;

		while (frontRoadObs2 == null) {
			frontRoadObs2 = selectedTilesPool [Random.Range (0, selectedTilesPool.Count)];
			if (frontRoadObs2.activeInHierarchy)
				frontRoadObs2 = null;
		}

		frontRoadObs2.SetActive (true);

		for (int i = 0; i < frontRoadObs2.transform.childCount; i++) {
			frontRoadObs2.transform.GetChild (i).gameObject.SetActive (true);
		}

		frontRoadObs2.transform.position = new Vector3(0f, 0f, changingRoadStartPos.transform.position.z + 100f);
		frontRoadObs2.transform.eulerAngles = Vector3.zero;

		//RIGHT ROAD 2;

		GameObject rightRoadEnviro2 = selectedEnviroPool [enviroCount + 2];

		rightRoadEnviro2.SetActive (true);
		rightRoadEnviro2.transform.position = new Vector3 (0f, 0f, changingRoadStartPos.transform.position.z + 100f);
		rightRoadEnviro2.transform.eulerAngles = Vector3.zero;
		rightRoadEnviro2.transform.parent = environmentParent;

		GameObject rightRoadObs2 = null;

		while (rightRoadObs2 == null) {
			rightRoadObs2 = selectedTilesPool [Random.Range (0, selectedTilesPool.Count)];
			if (rightRoadObs2.activeInHierarchy)
				rightRoadObs2 = null;
		}

		rightRoadObs2.SetActive (true);

		for (int i = 0; i < rightRoadObs2.transform.childCount; i++) {
			rightRoadObs2.transform.GetChild (i).gameObject.SetActive (true);
		}

		rightRoadObs2.transform.position = new Vector3(0f, 0f, changingRoadStartPos.transform.position.z + 100f);
		rightRoadObs2.transform.eulerAngles = Vector3.zero;

		enviroCount += 3;
		if (enviroCount >= ((maxEnviroCount * 3) - 1))
			enviroCount = 0;

		rightRoadEnviro2.transform.RotateAround (rc.GetCenter ().position, Vector3.up, 90f);
		rightRoadObs2.transform.RotateAround (rc.GetCenter ().position, Vector3.up, 90f);
	}

//	public void ChangeObsBonParent(Transform obsParent, Transform bonParent, bool returnToDefault) {
//		if (!returnToDefault) {
//			obstacleParent = obsParent;
//			bonificationParent = bonParent;
//		}
//		else {
//			obstacleParent = defaultObstacleParent;
//		}
//	}

	public void CreateProvinceChange() {
		changingProvince = true;

//		Mesh mesh = terrain.GetComponent<MeshFilter> ().sharedMesh;
		List<Vector3> vertices = new List<Vector3> (terrain.GetComponent<MeshFilter> ().mesh.vertices);
		vertices.RemoveAt (vertices.Count - 1);
		vertices.RemoveAt (vertices.Count - 1);

		float pos = vertices [vertices.Count - 1].z + meshStartDistance;

		List<Vector2> uv = new List<Vector2> (terrain.GetComponent<MeshFilter> ().mesh.uv);
		uv.RemoveAt (uv.Count - 1);
		uv.RemoveAt(uv.Count - 1);

		List<int> triangles = new List<int> (terrain.GetComponent<MeshFilter> ().mesh.triangles);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);
		triangles.RemoveAt (triangles.Count - 1);

//		mesh.Clear ();
//		mesh.vertices = vertices.ToArray();
//		mesh.triangles = triangles.ToArray ();
//		mesh.uv = uv.ToArray();
//		mesh.RecalculateBounds ();
//		mesh.RecalculateNormals ();
//		terrain.GetComponent<MeshFilter> ().sharedMesh.UploadMeshData (false);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetVertices(vertices);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetTriangles (triangles, 0);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetUVs (0, uv);
		terrain.GetComponent<MeshFilter> ().mesh.Clear();
		terrain.GetComponent<MeshFilter> ().mesh.vertices = vertices.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.triangles = triangles.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.uv = uv.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);

		ProvinceChange pc = new ProvinceChange (pos);
	}

	public void DestroyTerrainMesh() {
		terrain.GetComponent<MeshFilter> ().mesh.Clear ();
		Destroy (terrain.GetComponent<MeshFilter> ().mesh);
		Destroy (terrain);

		leftTerrain.GetComponent<MeshFilter> ().mesh.Clear ();
		Destroy (leftTerrain.GetComponent<MeshFilter> ().mesh);
		Destroy (leftTerrain);

		rightTerrain.GetComponent<MeshFilter> ().mesh.Clear ();
		Destroy (rightTerrain.GetComponent<MeshFilter> ().mesh);
		Destroy (rightTerrain);
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

//		mesh.vertices = tempVertices;
//		mesh.triangles = tempTriangles;
//		mesh.uv = tempUv;
//		mesh.RecalculateBounds ();
//		mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh = mesh;
		terrain.GetComponent<MeshFilter> ().mesh.vertices = tempVertices;
		terrain.GetComponent<MeshFilter> ().mesh.triangles = tempTriangles;
		terrain.GetComponent<MeshFilter> ().mesh.uv = tempUv;
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetVertices(tempVertices.ToList<Vector3>());
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetTriangles (tempTriangles.ToList<int> (), 0);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetUVs (0, tempUv.ToList<Vector2> ());

		Material mat = MyResources.Instance.GetMaterial (SceneManager.Instance.currentProvince.climate, true);
		terrain.GetComponent<MeshRenderer>().sharedMaterial = mat;

		terrain.transform.position = new Vector3 (terrain.transform.position.x, terrain.transform.position.y, startDistance);
		meshStartDistance = startDistance;
	}

	public void BuildEnviroMesh(float startDistance, bool right) {
		GameObject terrain = null;

		if (right) {
			terrain = new GameObject ("RightTerrain");
			rightTerrain = terrain;
		}
		else {
			terrain = new GameObject ("LeftTerrain");
			leftTerrain = terrain;
		}

		terrain.layer = LayerMask.NameToLayer("Walkable");
		terrain.transform.SetParent (transform.GetChild (0));
		terrain.AddComponent<MeshFilter> ();
		terrain.AddComponent<MeshRenderer> ();

		float maxLeft = (right) ? 5f : -60f;
		float maxRight = (right) ? 60f : -5f;

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

//		mesh.vertices = tempVertices;
//		mesh.triangles = tempTriangles;
//		mesh.uv = tempUv;
//		mesh.RecalculateBounds ();
//		mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh = mesh;
		terrain.GetComponent<MeshFilter> ().mesh.vertices = tempVertices;
		terrain.GetComponent<MeshFilter> ().mesh.triangles = tempTriangles;
		terrain.GetComponent<MeshFilter> ().mesh.uv = tempUv;
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetVertices(tempVertices.ToList<Vector3>());
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetTriangles (tempTriangles.ToList<int>(), 0);
//		terrain.GetComponent<MeshFilter> ().sharedMesh.SetUVs (0, tempUv.ToList<Vector2>());

		Material mat = MyResources.Instance.GetMaterial (SceneManager.Instance.currentProvince.climate, false);
		terrain.GetComponent<MeshRenderer>().sharedMaterial = mat;

		terrain.transform.position = new Vector3 (terrain.transform.position.x, terrain.transform.position.y, startDistance);
		meshStartDistance = startDistance;
	}
}
