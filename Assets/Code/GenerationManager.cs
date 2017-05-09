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

	[HideInInspector] public List<GameObject> oceanicTilesPool = new List<GameObject> ();
	[HideInInspector] public List<GameObject> oceanicEnviroPool = new List<GameObject> ();
	[HideInInspector] public List<Material> oceanicMaterialsPool = new List<Material>();
	[HideInInspector] public List<GameObject> continentalTilesPool = new List<GameObject> ();
	[HideInInspector] public List<GameObject> continentalEnviroPool = new List<GameObject> ();
	[HideInInspector] public List<Material> continentalMaterialsPool = new List<Material>();
	[HideInInspector] public List<GameObject> mediterraneanTilesPool = new List<GameObject> ();
	[HideInInspector] public List<GameObject> mediterraneanEnviroPool = new List<GameObject> ();
	[HideInInspector] public List<Material> mediterraneanMaterialsPool = new List<Material>();
	[HideInInspector] public List<GameObject> roadChanges = new List<GameObject> ();
	[HideInInspector] public GameObject tunnel;
	[HideInInspector] public GameObject roadChangeSign;
	[HideInInspector] public GameObject provinceChangeSign;

	[HideInInspector] public List<GameObject> selectedTilesPool = new List<GameObject>();
	[HideInInspector] public List<GameObject> selectedEnviroPool = new List<GameObject> ();
	[HideInInspector] public List<Material> selectedMaterialsPool = new List<Material>();
	[HideInInspector] public GameObject selectedRoadChangePrefab;
	[HideInInspector] public Transform selectedTilesParent;
	[HideInInspector] public Transform selectedEnviroParent;
	[HideInInspector] public GameObject oceanicTiles;
	[HideInInspector] public GameObject oceanicEnviro;
	[HideInInspector] public GameObject continentalTiles;
	[HideInInspector] public GameObject continentalEnviro;
	[HideInInspector] public GameObject mediterraneanTiles;
	[HideInInspector] public GameObject mediterraneanEnviro;

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
	[HideInInspector] public int enviroCount;
	[HideInInspector] public GameObject lastTile;
	[HideInInspector] public GameObject lastEnviro;
	public float defaultSpeed;
	public Transform obstacleParent;
	public Transform environmentParent;
	public int maxEnviroCount;
	public float percentageRandomGeneration;
	public bool justEnteredRoadChange;
	public bool signAppeared;

	public static GenerationManager Instance;

	private void Awake() {
		Instance = this;

		tileManager = new TileArrayManager ();

		//ROAD CHANGE POOL;

		GameObject rc = Instantiate ((GameObject)Resources.Load ("RoadChanges/Oceanic_RoadChange"), new Vector3 (0f, 0f, -200f), Quaternion.identity) as GameObject;
		rc.SetActive (false);
		roadChanges.Add (rc);
		rc = Instantiate ((GameObject)Resources.Load ("RoadChanges/Continental_RoadChange"), new Vector3 (0f, 0f, -200f), Quaternion.identity) as GameObject;
		rc.SetActive (false);
		roadChanges.Add (rc);
//		rc = Instantiate ((GameObject)Resources.Load ("RoadChanges/Mediterranean_RoadChange"), new Vector3 (0f, 0f, -200f), Quaternion.identity) as GameObject;
//		rc.SetActive (false);
//		roadChanges.Add (rc);

		GameObject pref = (GameObject)Resources.Load ("Signs/RoadChangeSign");
		roadChangeSign = Instantiate (pref, new Vector3 (0f, 0f, -20f), pref.transform.rotation) as GameObject;
		roadChangeSign.SetActive (false);

		//TUNNEL POOL;

		tunnel = Instantiate ((GameObject)Resources.Load ("Tunnel/Tunnel"), new Vector3 (0f, 0f, -200f), Quaternion.identity) as GameObject;
		tunnel.SetActive (false);

		//MATERIALS POOL;

		oceanicMaterialsPool.Add ((Material)Resources.Load ("Materials/Oceanic_TerrainMat"));
		oceanicMaterialsPool.Add ((Material)Resources.Load ("Materials/Oceanic_EnviroMat"));
		continentalMaterialsPool.Add ((Material)Resources.Load ("Materials/Continental_TerrainMat"));
		continentalMaterialsPool.Add ((Material)Resources.Load ("Materials/Continental_EnviroMat"));
//		mediterraneanMaterialsPool.Add ((Material)Resources.Load ("Materials/Mediterranean_TerrainMat"));
//		mediterraneanMaterialsPool.Add ((Material)Resources.Load ("Materials/Mediterranean_EnviroMat"));

		//OBSTACLES POOL;

		oceanicTiles = new GameObject ("Oceanic_Tiles");

		for (int i = 0; i < tileManager.oceanicTileArrays.Count; i++) {
			GameObject prefab = tileManager.oceanicTileArrays [i];
			GameObject obj1 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			obj1.transform.parent = oceanicTiles.transform;
			obj1.SetActive (false);
			GameObject obj2 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			obj2.transform.parent = oceanicTiles.transform;
			obj2.SetActive (false);
			GameObject obj3 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			obj3.transform.parent = oceanicTiles.transform;
			obj3.SetActive (false);

			oceanicTilesPool.Add (obj1);
			oceanicTilesPool.Add (obj2);
			oceanicTilesPool.Add (obj3);
		}

		continentalTiles = new GameObject ("Continental_Tiles");

		for (int i = 0; i < tileManager.continentalTileArrays.Count; i++) {
			GameObject prefab = tileManager.continentalTileArrays [i];
			GameObject obj1 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			obj1.transform.parent = continentalTiles.transform;
			obj1.SetActive (false);
			GameObject obj2 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			obj2.transform.parent = continentalTiles.transform;
			obj2.SetActive (false);
			GameObject obj3 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			obj3.transform.parent = continentalTiles.transform;
			obj3.SetActive (false);

			continentalTilesPool.Add (obj1);
			continentalTilesPool.Add (obj2);
			continentalTilesPool.Add (obj3);
		}

		//ENVIRONMENT POOL;

		oceanicEnviro = new GameObject ("Oceanic_Enviro");
		int count = 0;

		for (int i = 0; i < 10; i++) {
			GameObject prefab = tileManager.oceanicEnviros [count];

			GameObject enviro = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			enviro.transform.parent = oceanicEnviro.transform;
			enviro.SetActive (false);

			GameObject enviro2 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			enviro2.transform.parent = oceanicEnviro.transform;
			enviro2.SetActive (false);

			GameObject enviro3 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			enviro3.transform.parent = oceanicEnviro.transform;
			enviro3.SetActive (false);

			oceanicEnviroPool.Add (enviro);
			oceanicEnviroPool.Add (enviro2);
			oceanicEnviroPool.Add (enviro3);

			count++;
			if (count >= tileManager.oceanicEnviros.Count)
				count = 0;
		}

		continentalEnviro = new GameObject ("Continental_Enviro");
		count = 0;

		for (int i = 0; i < 10; i++) {
			GameObject prefab = tileManager.continentalEnviros [count];

			GameObject enviro = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			enviro.transform.parent = continentalEnviro.transform;
			enviro.SetActive (false);

			GameObject enviro2 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			enviro2.transform.parent = continentalEnviro.transform;
			enviro2.SetActive (false);

			GameObject enviro3 = Instantiate (prefab, new Vector3 (0f, 0f, -10f), prefab.transform.rotation) as GameObject;
			enviro3.transform.parent = continentalEnviro.transform;
			enviro3.SetActive (false);

			continentalEnviroPool.Add (enviro);
			continentalEnviroPool.Add (enviro2);
			continentalEnviroPool.Add (enviro3);

			count++;
			if (count >= tileManager.continentalEnviros.Count)
				count = 0;
		}

		mediterraneanTiles = new GameObject ("Mediterranean_Tiles");
		mediterraneanEnviro = new GameObject ("Mediterranean_Enviro");

		//SELECT CURRENT CLIMATE POOL;

		if (SceneManager.Instance.currentProvince.climate == Climate.Oceanic) {
			oceanicTiles.transform.parent = obstacleParent;
			selectedTilesParent = oceanicTiles.transform;
			oceanicEnviro.transform.parent = environmentParent;
			selectedEnviroParent = oceanicEnviro.transform;
			selectedTilesPool = oceanicTilesPool;
			selectedEnviroPool = oceanicEnviroPool;
			selectedMaterialsPool = oceanicMaterialsPool;
			selectedRoadChangePrefab = roadChanges [0];
		}

		if (SceneManager.Instance.currentProvince.climate == Climate.Continental) {
			continentalTiles.transform.parent = obstacleParent;
			selectedTilesParent = continentalTiles.transform;
			continentalEnviro.transform.parent = environmentParent;
			selectedEnviroParent = continentalEnviro.transform;
			selectedTilesPool = continentalTilesPool;
			selectedEnviroPool = continentalEnviroPool;
			selectedMaterialsPool = continentalMaterialsPool;
			selectedRoadChangePrefab = roadChanges [1];
		}

		if (SceneManager.Instance.currentProvince.climate == Climate.Mediterranean) {
//			mediterraneanTiles.transform.parent = obstacleParent;
//			selectedTilesParent = mediterraneanTiles.transform;
//			mediterraneanEnviro.transform.parent = environmentParent;
//			selectedEnviroParent = mediterraneanEnviro.transform;
			selectedTilesPool = mediterraneanTilesPool;
			selectedEnviroPool = mediterraneanEnviroPool;
			selectedMaterialsPool = mediterraneanMaterialsPool;
			selectedRoadChangePrefab = roadChanges [2];
		}

		//INITIAL OBSTACLES;

		enviroCount = 0;

		for (int i = 0; i < 4; i++) {
			GameObject enviro = selectedEnviroPool [enviroCount];
			enviro.SetActive (true);
			enviro.transform.position = new Vector3 (0f, 0f, -10f + (50f * i));
			enviro.transform.eulerAngles = Vector3.zero;
			enviro.transform.parent = selectedEnviroParent;

			if (i >= 2) {
				GameObject obs = null;

				while (obs == null) {
					obs = selectedTilesPool [Random.Range (0, selectedTilesPool.Count)];
					if (obs.activeInHierarchy)
						obs = null;
				}

				obs.SetActive (true);
				obs.transform.position = new Vector3 (0f, 0f, -10f + (50f * i));
				obs.transform.eulerAngles = Vector3.zero;
				obs.transform.parent = selectedTilesParent;
			}

			enviroCount += 3;
			if (enviroCount > ((maxEnviroCount * 3) - 1))
				enviroCount = 0;
		}
	}

	private void Start () {
		generationDistance = 200f;
		destroyDistance = -30f;
		displacementSpeed = SceneManager.Instance.difSpeed[SceneManager.Instance.dif];
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

		if (SceneManager.Instance.provinceKm >= 5f && !signAppeared) {
			signAppeared = true;
			CreateRoadChangeSign ();
		}

		if (SceneManager.Instance.provinceKm >= 10f && !selectedRoad && tileCount >= tileSize) {
			selectedRoad = true;
			CreateRoadChange();
		}

//		if (SceneManager.Instance.provinceKm >= 6f && !provinceChanged && tileCount >= tileSize) {
//			provinceChanged = true;
//			CreateProvinceChange ();
//		}

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

	private void CreateRoadChangeSign() {
		float pos = terrain.GetComponent<MeshFilter> ().mesh.vertices [terrain.GetComponent<MeshFilter> ().mesh.vertices.Length - 1].z + meshStartDistance;

		roadChangeSign.GetComponent<RoadChangeSign> ().Initialize (pos);
	}

	private void GenerateEnvironment() {
		if (changingProvince)
			return;

		float pos = terrain.GetComponent<MeshFilter> ().mesh.vertices [terrain.GetComponent<MeshFilter> ().mesh.vertices.Length - 1].z + meshStartDistance;

		GameObject enviro = selectedEnviroPool [enviroCount];

		enviro.SetActive (true);

		enviro.transform.position = new Vector3 (0f, 0f, pos);
		enviro.transform.eulerAngles = Vector3.zero;
		lastEnviro = enviro;

		enviroCount += 3;
		if (enviroCount >= ((maxEnviroCount * 3) - 1))
			enviroCount = 0;
	}

	private void GenerateTile() {
		if (changingProvince)
			return;

		float pos = terrain.GetComponent<MeshFilter> ().mesh.vertices [terrain.GetComponent<MeshFilter> ().mesh.vertices.Length - 1].z + meshStartDistance;

		GameObject obs = null;

		while (obs == null) {
			obs = selectedTilesPool [Random.Range (0, selectedTilesPool.Count)];
			if (obs.activeInHierarchy) {
				obs = null;
			}
		}

		obs.SetActive (true);

		for (int i = 0; i < obs.transform.childCount; i++) {
			obs.transform.GetChild (i).gameObject.SetActive (true);
		}

		lastTile = obs;

		Vector3 finalPos = new Vector3 (0f, 0f, pos);
		obs.transform.position = finalPos;
		obs.transform.eulerAngles = Vector3.zero;
	}

	public void ChangeDisplacementSpeed(float newSpeed, bool returnToDefault) {
		if (returnToDefault) {
			displacementSpeed = SceneManager.Instance.difSpeed[SceneManager.Instance.dif];
		}
		else {
			displacementSpeed = newSpeed;
		}
	}

	private void UpdateMesh(GameObject terrain, bool right) {
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

		terrain.GetComponent<MeshFilter> ().mesh.Clear();
		terrain.GetComponent<MeshFilter> ().mesh.vertices = vertices;
		terrain.GetComponent<MeshFilter> ().mesh.triangles = triangles;
		terrain.GetComponent<MeshFilter> ().mesh.uv = uv;
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);
	}

	private void CreateRoadChange() {
		changingRoad = true;

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

		terrain.GetComponent<MeshFilter> ().mesh.Clear();
		terrain.GetComponent<MeshFilter> ().mesh.vertices = vertices.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.triangles = triangles.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.uv = uv.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);

		RoadChange rc = new RoadChange (selectedRoadChangePrefab, roadChangeSign, pos);

		StartCoroutine (LoadRoadChangeAssets (rc));
	}

	private IEnumerator LoadRoadChangeAssets(RoadChange rc) {
		//LEFT ROAD 1;

		GameObject leftRoadEnviro1 = selectedEnviroPool [enviroCount];

		leftRoadEnviro1.SetActive (true);
		leftRoadEnviro1.transform.position = new Vector3 (0f, 0f, changingRoadStartPos.transform.position.z + 50f);
		leftRoadEnviro1.transform.eulerAngles = Vector3.zero;

		GameObject leftRoadObs1 = null;
		int r = Random.Range (0, selectedTilesPool.Count);

		while (leftRoadObs1 == null) {
			leftRoadObs1 = selectedTilesPool [r];
			if (leftRoadObs1.activeInHierarchy) {
				leftRoadObs1 = null;
				r++;
			}
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

		GameObject frontRoadObs1 = null;
		r = Random.Range (0, selectedTilesPool.Count);

		while (frontRoadObs1 == null) {
			frontRoadObs1 = selectedTilesPool [r];
			if (frontRoadObs1.activeInHierarchy) {
				frontRoadObs1 = null;
				r++;
			}
		}

		frontRoadObs1.SetActive (true);

		for (int i = 0; i < frontRoadObs1.transform.childCount; i++) {
			frontRoadObs1.transform.GetChild (i).gameObject.SetActive (true);
		}

		frontRoadObs1.transform.position = new Vector3(0f, 0f, changingRoadStartPos.transform.position.z + 50f);
		frontRoadObs1.transform.eulerAngles = Vector3.zero;

		//RIGHT ROAD 1;

		GameObject rightRoadEnviro1 = selectedEnviroPool [enviroCount + 2];

		rightRoadEnviro1.SetActive (true);
		rightRoadEnviro1.transform.position = new Vector3 (0f, 0f, changingRoadStartPos.transform.position.z + 50f);
		rightRoadEnviro1.transform.eulerAngles = Vector3.zero;

		GameObject rightRoadObs1 = null;
		r = Random.Range (0, selectedTilesPool.Count);

		while (rightRoadObs1 == null) {
			rightRoadObs1 = selectedTilesPool [r];
			if (rightRoadObs1.activeInHierarchy) {
				rightRoadObs1 = null;
				r++;
			}
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

		GameObject leftRoadObs2 = null;
		r = Random.Range (0, selectedTilesPool.Count);

		while (leftRoadObs2 == null) {
			leftRoadObs2 = selectedTilesPool [r];
			if (leftRoadObs2.activeInHierarchy) {
				leftRoadObs2 = null;
				r++;
			}
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

		GameObject frontRoadObs2 = null;
		r = Random.Range (0, selectedTilesPool.Count);

		while (frontRoadObs2 == null) {
			frontRoadObs2 = selectedTilesPool [r];
			if (frontRoadObs2.activeInHierarchy) {
				frontRoadObs2 = null;
				r++;
			}
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

		GameObject rightRoadObs2 = null;
		r = Random.Range (0, selectedTilesPool.Count);

		while (rightRoadObs2 == null) {
			rightRoadObs2 = selectedTilesPool [r];
			if (rightRoadObs2.activeInHierarchy) {
				rightRoadObs2 = null;
				r++;
			}
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

	public void CreateProvinceChange() {
		provinceChanged = true;
		changingProvince = true;

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

		terrain.GetComponent<MeshFilter> ().mesh.Clear();
		terrain.GetComponent<MeshFilter> ().mesh.vertices = vertices.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.triangles = triangles.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.uv = uv.ToArray ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);

		new ProvinceChange (tunnel);
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

		terrain.GetComponent<MeshFilter> ().mesh = mesh;
		terrain.GetComponent<MeshFilter> ().mesh.vertices = tempVertices;
		terrain.GetComponent<MeshFilter> ().mesh.triangles = tempTriangles;
		terrain.GetComponent<MeshFilter> ().mesh.uv = tempUv;
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);

		Material mat = selectedMaterialsPool [0];
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

		terrain.GetComponent<MeshFilter> ().mesh = mesh;
		terrain.GetComponent<MeshFilter> ().mesh.vertices = tempVertices;
		terrain.GetComponent<MeshFilter> ().mesh.triangles = tempTriangles;
		terrain.GetComponent<MeshFilter> ().mesh.uv = tempUv;
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		terrain.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();
		terrain.GetComponent<MeshFilter> ().mesh.UploadMeshData (false);

		Material mat = selectedMaterialsPool [1];
		terrain.GetComponent<MeshRenderer>().sharedMaterial = mat;

		terrain.transform.position = new Vector3 (terrain.transform.position.x, terrain.transform.position.y, startDistance);
		meshStartDistance = startDistance;
	}
}
