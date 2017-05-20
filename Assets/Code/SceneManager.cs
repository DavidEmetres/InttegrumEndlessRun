using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

	private Neighbours neighbour;
	private bool neighbourChoosed;
	private Direction nextDirection;
	private bool changingProvince;
	private List<Province> provinces = new List<Province> ();

	public Province currentProvince;
	public Direction displacementDirection;
	public bool gameOver;

	public List<Province> provincesRunned = new List<Province>();
	public delegate void OnRoadChangeStarted ();
	public event OnRoadChangeStarted onRoadChangeStarted;
	public delegate void OnRoadChangeFinished();
	public event OnRoadChangeFinished onRoadChangeFinished;

	public float[] difSpeed;
	public int[] changeDifKm;
	public int dif;
	public float kmDifCount;
	public bool slowed;

	[Header("Scene Settings")]
	public Vector3[] lanes;
	public Vector3[] cameraLanes;

	[Header("Player Variables")]
	public int life;
	public int coins;
	public float totalKm;
	public float provinceKm;

	public GUIStyle style;

	public static SceneManager Instance;

	private void Awake() {
		Instance = this;
		Application.targetFrameRate = 60;

		//COMUNIDAD VALENCIANA;
		Province castellon = new Province ("castellón", Climate.Mediterranean);
		provinces.Add (castellon);
		Province valencia = new Province ("valencia", Climate.Mediterranean);
		provinces.Add (valencia);
		Province alicante = new Province ("alicante", Climate.Mediterranean);
		provinces.Add (alicante);
		//CATALUÑA;
		Province tarragona = new Province ("tarragona", Climate.Mediterranean);
		provinces.Add (tarragona);
		Province barcelona = new Province ("barcelona", Climate.Mediterranean);
		provinces.Add (barcelona);
		Province gerona = new Province ("gerona", Climate.Mediterranean);
		provinces.Add (gerona);
		Province lerida = new Province ("lérida", Climate.Continental);
		provinces.Add (lerida);
		//ARAGÓN;
		Province huesca = new Province ("huesca", Climate.Continental);
		provinces.Add (huesca);
		Province zaragoza = new Province ("zaragoza", Climate.Continental);
		provinces.Add (zaragoza);
		Province teruel = new Province ("teruel", Climate.Continental);
		provinces.Add (teruel);
		//NAVARRA;
		Province navarra = new Province ("navarra", Climate.Oceanic);
		provinces.Add (navarra);
		//PAIS VASCO;
		Province guipuzcoa = new Province ("guipúzcoa", Climate.Oceanic);
		provinces.Add (guipuzcoa);
		Province vizcaya = new Province ("vizcaya", Climate.Oceanic);
		provinces.Add (vizcaya);
		Province alava = new Province ("álava", Climate.Oceanic);
		provinces.Add (alava);
		//LA RIOJA;
		Province larioja = new Province ("la rioja", Climate.Continental);
		provinces.Add (larioja);
		//CANTABRIA;
		Province cantabria = new Province ("cantabria", Climate.Oceanic);
		provinces.Add (cantabria);
		//ASTURIAS;
		Province asturias = new Province ("asturias", Climate.Oceanic);
		provinces.Add (asturias);
		//GALICIA;
		Province lacoruna = new Province ("la coruña", Climate.Oceanic);
		provinces.Add (lacoruna);
		Province lugo = new Province ("lugo", Climate.Oceanic);
		provinces.Add (lugo);
		Province pontevedra = new Province ("pontevedra", Climate.Oceanic);
		provinces.Add (pontevedra);
		Province ourense = new Province ("ourense", Climate.Oceanic);
		provinces.Add (ourense);
		//CASTILLA Y LEÓN;
		Province leon = new Province ("león", Climate.Continental);
		provinces.Add (leon);
		Province palencia = new Province ("palencia", Climate.Continental);
		provinces.Add (palencia);
		Province burgos = new Province ("burgos", Climate.Continental);
		provinces.Add (burgos);
		Province soria = new Province ("soria", Climate.Continental);
		provinces.Add (soria);
		Province segovia = new Province ("segovia", Climate.Continental);
		provinces.Add (segovia);
		Province valladolid = new Province ("valladolid", Climate.Continental);
		provinces.Add (valladolid);
		Province zamora = new Province ("zamora", Climate.Continental);
		provinces.Add (zamora);
		Province salamanca = new Province ("salamanca", Climate.Continental);
		provinces.Add (salamanca);
		Province avila = new Province ("ávila", Climate.Continental);
		provinces.Add (avila);
		//MADRID;
		Province madrid = new Province ("madrid", Climate.Continental);
		provinces.Add (madrid);
		//CASTILLA LA MANCHA;
		Province guadalajara = new Province ("guadalajara", Climate.Continental);
		provinces.Add (guadalajara);
		Province cuenca = new Province ("cuenca", Climate.Continental);
		provinces.Add (cuenca);
		Province toledo = new Province ("toledo", Climate.Continental);
		provinces.Add (toledo);
		Province ciudadreal = new Province ("ciudad real", Climate.Continental);
		provinces.Add (ciudadreal);
		Province albacete = new Province ("albacete", Climate.Continental);
		provinces.Add (albacete);
		//EXTREMADURA;
		Province caceres = new Province ("cáceres", Climate.Continental);
		provinces.Add (caceres);
		Province badajoz = new Province ("badajoz", Climate.Continental);
		provinces.Add (badajoz);
		//ANDALUCÍA;
		Province huelva = new Province ("huelva", Climate.Mediterranean);
		provinces.Add (huelva);
		Province sevilla = new Province ("sevilla", Climate.Continental);
		provinces.Add (sevilla);
		Province cadiz = new Province ("cádiz", Climate.Mediterranean);
		provinces.Add (cadiz);
		Province cordoba = new Province ("córdoba", Climate.Continental);
		provinces.Add (cordoba);
		Province malaga = new Province ("málaga", Climate.Mediterranean);
		provinces.Add (malaga);
		Province jaen = new Province ("jaén", Climate.Continental);
		provinces.Add (jaen);
		Province granada = new Province ("granada", Climate.Mediterranean);
		provinces.Add (granada);
		Province almeria = new Province ("almería", Climate.Mediterranean);
		provinces.Add (almeria);
		//MURCIA;
		Province murcia = new Province ("murcia", Climate.Mediterranean);
		provinces.Add (murcia);

		//SET NEIGHBOURS;
		castellon.SetNeighbours(new Neighbours[] {new Neighbours(tarragona, 167.05f)}, 
			new Neighbours[] {new Neighbours(valencia, 63.87f)}, 
			null, 
			new Neighbours[] {new Neighbours(teruel, 98.27f)});
		valencia.SetNeighbours (new Neighbours[] { new Neighbours (castellon, 63.87f), new Neighbours (teruel, 115.73f)}, 
			new Neighbours[] { new Neighbours (alicante, 125.51f)}, 
			null, 
			new Neighbours[] { new Neighbours (albacete, 138.34f), new Neighbours (cuenca, 164.84f)});
		alicante.SetNeighbours (new Neighbours[] { new Neighbours (valencia, 125.51f) }, 
			null, 
			null, 
			new Neighbours[] { new Neighbours (albacete, 139.08f), new Neighbours (murcia, 68.48f)});
		tarragona.SetNeighbours (new Neighbours[]{ new Neighbours (lerida, 76.19f), new Neighbours (barcelona, 83.20f) }, 
			new Neighbours[]{ new Neighbours (castellon, 167.05f) }, 
			null, 
			new Neighbours[] { new Neighbours (teruel, 216.17f), new Neighbours (zaragoza, 187.76f)});
		barcelona.SetNeighbours (new Neighbours[]{ new Neighbours (gerona, 85.33f)}, 
			new Neighbours[]{ new Neighbours (tarragona, 83.20f)}, 
			null, 
			new Neighbours[]{ new Neighbours (lerida, 132.06f) });
		gerona.SetNeighbours (null, 
			new Neighbours[]{new Neighbours(barcelona, 85.33f)}, 
			null, 
			new Neighbours[]{new Neighbours(lerida, 187.07f)});
		lerida.SetNeighbours (null, 
			new Neighbours[]{new Neighbours(tarragona, 76.19f)}, 
			new Neighbours[]{new Neighbours(barcelona, 132.06f), new Neighbours(gerona, 187.07f)}, 
			new Neighbours[]{new Neighbours(huesca, 102.64f)});
		huesca.SetNeighbours (null, 
			new Neighbours[]{new Neighbours(zaragoza, 66.96f)}, 
			new Neighbours[]{new Neighbours(lerida, 102.64f)}, 
			new Neighbours[]{new Neighbours(navarra, 121.65f)});
		zaragoza.SetNeighbours (new Neighbours[]{new Neighbours(huesca, 66.96f), new Neighbours(navarra, 133.33f)}, 
			new Neighbours[]{new Neighbours(teruel, 146.21f), new Neighbours(guadalajara, 221.41f)}, 
			new Neighbours[]{new Neighbours(tarragona, 187.76f)}, 
			new Neighbours[]{new Neighbours(soria, 132.72f)});
		teruel.SetNeighbours (new Neighbours[]{new Neighbours(zaragoza, 146.21f)}, 
			new Neighbours[]{new Neighbours(valencia, 115.73f), new Neighbours(cuenca, 92.84f)}, 
			new Neighbours[]{new Neighbours(tarragona, 216.17f), new Neighbours(castellon, 98.27f)}, 
			new Neighbours[]{new Neighbours(guadalajara, 176.79f)});
		navarra.SetNeighbours (null,
			new Neighbours[]{new Neighbours(larioja, 84.20f), new Neighbours(zaragoza, 133.33f)},
			new Neighbours[]{new Neighbours(huesca, 121.65f)},
			new Neighbours[]{new Neighbours(guipuzcoa, 61.54f), new Neighbours(alava, 86.86f)});
		guipuzcoa.SetNeighbours (null,
			new Neighbours[]{ new Neighbours (alava, 42.82f) },
			new Neighbours[]{ new Neighbours (navarra, 61.54f) },
			new Neighbours[]{ new Neighbours (vizcaya, 41.79f) });
		vizcaya.SetNeighbours (null,
			new Neighbours[]{ new Neighbours (burgos, 127.19f), new Neighbours (alava, 34.55f) },
			new Neighbours[]{ new Neighbours (guipuzcoa, 41.79f) },
			new Neighbours[]{ new Neighbours (cantabria, 104.72f) });
		alava.SetNeighbours (new Neighbours[]{ new Neighbours (vizcaya, 34.55f), new Neighbours (guipuzcoa, 42.82f) },
			new Neighbours[]{ new Neighbours (larioja, 70.55f) },
			new Neighbours[]{ new Neighbours (navarra, 86.86f) },
			new Neighbours[]{ new Neighbours (burgos, 103.24f) });
		larioja.SetNeighbours (new Neighbours[]{ new Neighbours (alava, 70.55f), new Neighbours (navarra, 84.20f) },
			new Neighbours[]{ new Neighbours (soria, 58.16f) },
			null,
			new Neighbours[]{ new Neighbours (burgos, 95.47f) });
		cantabria.SetNeighbours (null,
			new Neighbours[]{ new Neighbours (burgos, 96.37f), new Neighbours (palencia, 137.90f) },
			new Neighbours[]{ new Neighbours (vizcaya, 104.72f) },
			new Neighbours[]{ new Neighbours (asturias, 152.98f), new Neighbours (leon, 144.28f) });
		asturias.SetNeighbours (null,
			new Neighbours[]{ new Neighbours (leon, 88.18f) },
			new Neighbours[]{ new Neighbours (cantabria, 152.98f) },
			new Neighbours[]{ new Neighbours (lugo, 143.23f) });
		lacoruna.SetNeighbours (null,
			new Neighbours[]{ new Neighbours (pontevedra, 105.52f) },
			new Neighbours[]{ new Neighbours (lugo, 79.71f) },
			null);
		lugo.SetNeighbours (null,
			new Neighbours[]{ new Neighbours (ourense, 79.12f) },
			new Neighbours[]{ new Neighbours (asturias, 143.23f), new Neighbours (leon, 168.80f) },
			new Neighbours[]{ new Neighbours (lacoruna, 79.71f), new Neighbours (pontevedra, 109.92f) });
		pontevedra.SetNeighbours (new Neighbours[]{ new Neighbours (lacoruna, 105.52f) },
			null,
			new Neighbours[]{ new Neighbours (lugo, 109.92f), new Neighbours (ourense, 65.03f) },
			null);
		ourense.SetNeighbours (new Neighbours[]{ new Neighbours (lugo, 79.12f) },
			null,
			new Neighbours[]{ new Neighbours (leon, 190.85f), new Neighbours (zamora, 198.34f) },
			new Neighbours[]{ new Neighbours (pontevedra, 65.03f) });
		leon.SetNeighbours (new Neighbours[]{ new Neighbours (asturias, 88.18f) },
			new Neighbours[]{ new Neighbours (zamora, 122.82f), new Neighbours (valladolid, 126.25f) },
			new Neighbours[]{ new Neighbours (palencia, 107.73f), new Neighbours (cantabria, 144.28f) },
			new Neighbours[]{ new Neighbours (lugo, 168.80f), new Neighbours (ourense, 190.85f) });
		palencia.SetNeighbours (new Neighbours[]{ new Neighbours (cantabria, 137.90f) },
			new Neighbours[]{ new Neighbours (valladolid, 42.97f) },
			new Neighbours[]{ new Neighbours (burgos, 78.07f) },
			new Neighbours[]{ new Neighbours (leon, 107.73f) });
		burgos.SetNeighbours (new Neighbours[]{ new Neighbours (cantabria, 96.37f), new Neighbours (vizcaya, 127.19f) },
			new Neighbours[]{ new Neighbours (segovia, 159.69f), new Neighbours (soria, 119.43f) },
			new Neighbours[]{ new Neighbours (alava, 103.24f), new Neighbours (larioja, 95.47f) },
			new Neighbours[]{ new Neighbours (valladolid, 114.70f), new Neighbours (palencia, 78.07f) });
		soria.SetNeighbours (new Neighbours[]{ new Neighbours (larioja, 58.16f) },
			new Neighbours[]{ new Neighbours (guadalajara, 138.53f) },
			new Neighbours[]{ new Neighbours (zaragoza, 132.72f), new Neighbours (navarra, 122.76f) },
			new Neighbours[]{ new Neighbours (burgos, 119.43f), new Neighbours (segovia, 164.17f) });
		segovia.SetNeighbours (new Neighbours[]{ new Neighbours (burgos, 159.69f) },
			new Neighbours[]{ new Neighbours (madrid, 67.82f) },
			new Neighbours[]{ new Neighbours (soria, 164.17f), new Neighbours (guadalajara, 87.10f) },
			new Neighbours[]{ new Neighbours (valladolid, 94.28f), new Neighbours (avila, 57.65f) });
		valladolid.SetNeighbours (new Neighbours[]{ new Neighbours (palencia, 42.97f), new Neighbours (leon, 126.25f) },
			new Neighbours[]{ new Neighbours (avila, 112.56f) },
			new Neighbours[]{ new Neighbours (burgos, 114.70f), new Neighbours (segovia, 94.28f) },
			new Neighbours[]{ new Neighbours (zamora, 86.73f), new Neighbours (salamanca, 109.23f) });
		zamora.SetNeighbours (new Neighbours[]{ new Neighbours (leon, 122.82f) },
			new Neighbours[]{ new Neighbours (salamanca, 59.79f) },
			new Neighbours[]{ new Neighbours (valladolid, 86.73f) },
			new Neighbours[]{ new Neighbours (ourense, 198.34f) });
		salamanca.SetNeighbours (new Neighbours[]{ new Neighbours (zamora, 59.79f), new Neighbours (valladolid, 109.23f) },
			new Neighbours[]{ new Neighbours (caceres, 176.96f) },
			new Neighbours[]{ new Neighbours (avila, 91.65f) },
			null);
		avila.SetNeighbours (new Neighbours[]{ new Neighbours (valladolid, 112.56f) },
			new Neighbours[]{ new Neighbours (caceres, 194.97f), new Neighbours (toledo, 102.31f) },
			new Neighbours[]{ new Neighbours (segovia, 57.65f), new Neighbours (madrid, 85.17f) },
			new Neighbours[]{ new Neighbours (salamanca, 91.65f) });
		madrid.SetNeighbours (new Neighbours[]{ new Neighbours (segovia, 67.82f) },
			new Neighbours[]{ new Neighbours (toledo, 67.53f) },
			new Neighbours[]{ new Neighbours (guadalajara, 51.89f), new Neighbours (cuenca, 138.56f) },
			new Neighbours[]{ new Neighbours (avila, 85.17f) });
		guadalajara.SetNeighbours (new Neighbours[]{ new Neighbours (soria, 138.53f) },
			new Neighbours[]{ new Neighbours (cuenca, 106.97f) },
			new Neighbours[]{ new Neighbours (zaragoza, 221.41f), new Neighbours (teruel, 176.79f) },
			new Neighbours[]{ new Neighbours (madrid, 51.89f), new Neighbours (segovia, 87.10f) });
		cuenca.SetNeighbours (new Neighbours[]{ new Neighbours (guadalajara, 106.97f) },
			new Neighbours[]{ new Neighbours (albacete, 122.15f) },
			new Neighbours[]{ new Neighbours (teruel, 92.84f), new Neighbours (valencia, 164.84f) },
			new Neighbours[] { new Neighbours (madrid, 138.56f), new Neighbours (toledo, 162.90f), new Neighbours (ciudadreal, 195.50f)});
		toledo.SetNeighbours (new Neighbours[]{ new Neighbours (madrid, 67.53f), new Neighbours (avila, 102.31f) },
			new Neighbours[]{ new Neighbours (ciudadreal, 98.15f) },
			new Neighbours[]{ new Neighbours (cuenca, 162.90f) },
			new Neighbours[]{ new Neighbours (caceres, 205.51f) });
		ciudadreal.SetNeighbours (new Neighbours[]{ new Neighbours (toledo, 98.15f) },
			new Neighbours[]{ new Neighbours (cordoba, 142.89f), new Neighbours (jaen, 134.70f) },
			new Neighbours[]{ new Neighbours (cuenca, 195.50f), new Neighbours (albacete, 178.97f) },
			new Neighbours[]{ new Neighbours (badajoz, 263.80f) });
		albacete.SetNeighbours (new Neighbours[]{ new Neighbours (cuenca, 122.15f) },
			new Neighbours[]{ new Neighbours (murcia, 128.33f), new Neighbours (granada, 253.28f) },
			new Neighbours[]{ new Neighbours (valencia, 138.34f), new Neighbours (alicante, 139.08f) },
			new Neighbours[]{ new Neighbours (ciudadreal, 178.97f), new Neighbours (jaen, 215.73f) });
		caceres.SetNeighbours (new Neighbours[]{ new Neighbours (salamanca, 176.96f) },
			new Neighbours[]{ new Neighbours (badajoz, 84.06f) },
			new Neighbours[]{ new Neighbours (avila, 194.97f), new Neighbours (toledo, 205.51f) },
			null);
		badajoz.SetNeighbours (new Neighbours[]{ new Neighbours (caceres, 84.06f) },
			new Neighbours[]{ new Neighbours (huelva, 180.13f), new Neighbours (sevilla, 187.04f) },
			new Neighbours[]{ new Neighbours (ciudadreal, 263.80f), new Neighbours (cordoba, 220.75f) },
			null);
		huelva.SetNeighbours (new Neighbours[]{ new Neighbours (badajoz, 180.13f) },
			null,
			new Neighbours[]{ new Neighbours (sevilla, 86.18f), new Neighbours (cadiz, 100.47f) },
			null);
		sevilla.SetNeighbours (new Neighbours[]{ new Neighbours (badajoz, 187.04f) },
			new Neighbours[]{ new Neighbours (cadiz, 99.70f), new Neighbours (malaga, 157.51f) },
			new Neighbours[]{ new Neighbours (cordoba, 119.88f) },
			new Neighbours[]{ new Neighbours (huelva, 86.18f) });
		cadiz.SetNeighbours (new Neighbours[]{ new Neighbours (sevilla, 99.70f) },
			null,
			new Neighbours[]{ new Neighbours (malaga, 168.21f) },
			new Neighbours[]{ new Neighbours (huelva, 100.47f) });
		cordoba.SetNeighbours (new Neighbours[]{ new Neighbours (badajoz, 220.75f), new Neighbours (ciudadreal, 142.89f) },
			new Neighbours[]{ new Neighbours (malaga, 133.70f), new Neighbours (granada, 130.87f) },
			new Neighbours[]{ new Neighbours (jaen, 88.26f) },
			new Neighbours[]{ new Neighbours (sevilla, 119.88f) });
		malaga.SetNeighbours (new Neighbours[]{ new Neighbours (sevilla, 157.51f), new Neighbours (cordoba, 133.70f) },
			null,
			new Neighbours[]{ new Neighbours (granada, 89.08f) },
			new Neighbours[]{ new Neighbours (cadiz, 168.21f) });
		jaen.SetNeighbours (new Neighbours[]{ new Neighbours (ciudadreal, 134.70f) },
			new Neighbours[]{ new Neighbours (granada, 69.02f) },
			new Neighbours[]{ new Neighbours (albacete, 215.73f) },
			new Neighbours[]{ new Neighbours (cordoba, 88.26f) });
		granada.SetNeighbours (new Neighbours[]{ new Neighbours (jaen, 69.02f), new Neighbours (albacete, 253.28f) },
			null,
			new Neighbours[]{ new Neighbours (almeria, 118.59f) },
			new Neighbours[]{ new Neighbours (cordoba, 130.87f), new Neighbours (malaga, 89.08f) });
		almeria.SetNeighbours (new Neighbours[]{ new Neighbours (murcia, 163.65f) },
			null,
			null,
			new Neighbours[]{ new Neighbours (granada, 118.59f) });
		murcia.SetNeighbours (new Neighbours[]{ new Neighbours (albacete, 128.33f) },
			new Neighbours[]{ new Neighbours (almeria, 163.65f) },
			new Neighbours[]{ new Neighbours (alicante, 68.48f) },
			null);


		foreach (Province p in provinces) {
			if (p.name == ProvincesData.Instance.selectedProvince)
				currentProvince = p;
		}

//		currentProvince = malaga;
		displacementDirection = (Direction)Random.Range(0, 4);
		provincesRunned.Add (currentProvince);
//		RandomNeighbourSelection ();
	}

	void Start() {
		if (currentProvince.climate == Climate.Oceanic)
			SoundManager.Instance.ChangeMusic ("oceanicClimate");
		else if (currentProvince.climate == Climate.Continental)
			SoundManager.Instance.ChangeMusic ("continentalClimate");
		else if (currentProvince.climate == Climate.Mediterranean)
			SoundManager.Instance.ChangeMusic ("mediterraneanClimate");
	}

	private void Update () {
		if (!gameOver) {
			provinceKm += (GenerationManager.Instance.displacementSpeed / 25f) * Time.deltaTime;

			if (neighbourChoosed) {
				if (provinceKm >= (neighbour.distanceBetweenProvinces) && !changingProvince) {
					changingProvince = true;
					GenerationManager.Instance.CreateProvinceChange ();
				}
			}

			if (!slowed) {
				kmDifCount += (GenerationManager.Instance.displacementSpeed / 50f) * Time.deltaTime;
				if (kmDifCount >= changeDifKm [dif] && dif < 3) {
					GenerationManager.Instance.displacementSpeed = difSpeed [dif];
					kmDifCount = 0;
					dif++;
				}
			}
		}
	}

	public void ProvinceChange() {
		Province previousProvince = currentProvince;
		currentProvince = neighbour.neighbourProvince;
		displacementDirection = nextDirection;
		provincesRunned.Add (currentProvince);
		neighbourChoosed = false;
		totalKm += provinceKm;
		provinceKm = 0f;
		changingProvince = false;
		GenerationManager.Instance.selectedRoad = false;
		GenerationManager.Instance.changingProvince = false;

		if (currentProvince.climate == Climate.Oceanic) {
			GenerationManager.Instance.oceanicTiles.transform.parent = GenerationManager.Instance.obstacleParent;
			GenerationManager.Instance.oceanicEnviro.transform.parent = GenerationManager.Instance.environmentParent;
			GenerationManager.Instance.continentalTiles.transform.parent = null;
			GenerationManager.Instance.continentalEnviro.transform.parent = null;
			GenerationManager.Instance.mediterraneanTiles.transform.parent = null;
			GenerationManager.Instance.mediterraneanEnviro.transform.parent = null;
			GenerationManager.Instance.selectedTilesParent = GenerationManager.Instance.oceanicTiles.transform;
			GenerationManager.Instance.selectedEnviroParent = GenerationManager.Instance.oceanicEnviro.transform;
			GenerationManager.Instance.selectedTilesPool = GenerationManager.Instance.oceanicTilesPool;
			GenerationManager.Instance.selectedEnviroPool = GenerationManager.Instance.oceanicEnviroPool;
			GenerationManager.Instance.selectedMaterialsPool = GenerationManager.Instance.oceanicMaterialsPool;
			GenerationManager.Instance.selectedRoadChangePrefab = GenerationManager.Instance.roadChanges [0];
		}

		if (currentProvince.climate == Climate.Continental) {
			GenerationManager.Instance.oceanicTiles.transform.parent = null;
			GenerationManager.Instance.oceanicEnviro.transform.parent = null;
			GenerationManager.Instance.continentalTiles.transform.parent = GenerationManager.Instance.obstacleParent;
			GenerationManager.Instance.continentalEnviro.transform.parent = GenerationManager.Instance.environmentParent;
			GenerationManager.Instance.mediterraneanTiles.transform.parent = null;
			GenerationManager.Instance.mediterraneanEnviro.transform.parent = null;
			GenerationManager.Instance.selectedTilesParent = GenerationManager.Instance.continentalTiles.transform;
			GenerationManager.Instance.selectedEnviroParent = GenerationManager.Instance.continentalEnviro.transform;
			GenerationManager.Instance.selectedTilesPool = GenerationManager.Instance.continentalTilesPool;
			GenerationManager.Instance.selectedEnviroPool = GenerationManager.Instance.continentalEnviroPool;
			GenerationManager.Instance.selectedMaterialsPool = GenerationManager.Instance.continentalMaterialsPool;
			GenerationManager.Instance.selectedRoadChangePrefab = GenerationManager.Instance.roadChanges [1];
		}

		if (currentProvince.climate == Climate.Mediterranean) {
			GenerationManager.Instance.oceanicTiles.transform.parent = null;
			GenerationManager.Instance.oceanicEnviro.transform.parent = null;
			GenerationManager.Instance.continentalTiles.transform.parent = null;
			GenerationManager.Instance.continentalEnviro.transform.parent = null;
			GenerationManager.Instance.mediterraneanTiles.transform.parent = GenerationManager.Instance.obstacleParent;
			GenerationManager.Instance.mediterraneanEnviro.transform.parent = GenerationManager.Instance.environmentParent;
			GenerationManager.Instance.selectedTilesParent = GenerationManager.Instance.mediterraneanTiles.transform;
			GenerationManager.Instance.selectedEnviroParent = GenerationManager.Instance.mediterraneanEnviro.transform;
			GenerationManager.Instance.selectedTilesPool = GenerationManager.Instance.mediterraneanTilesPool;
			GenerationManager.Instance.selectedEnviroPool = GenerationManager.Instance.mediterraneanEnviroPool;
			GenerationManager.Instance.selectedMaterialsPool = GenerationManager.Instance.mediterraneanMaterialsPool;
			GenerationManager.Instance.selectedRoadChangePrefab = GenerationManager.Instance.roadChanges [2];
		}

		if (currentProvince.climate == Climate.Oceanic && previousProvince.climate != Climate.Oceanic)
			SoundManager.Instance.FadeOut ("oceanicClimate");
		else if (currentProvince.climate == Climate.Continental && previousProvince.climate != Climate.Continental)
			SoundManager.Instance.FadeOut ("continentalClimate");
		else if (currentProvince.climate == Climate.Mediterranean && previousProvince.climate != Climate.Mediterranean)
			SoundManager.Instance.FadeOut ("mediterraneanClimate");
	}

	public void ChooseNextNeighbour(Neighbours n, Direction newDir) {
		neighbour = n;
		nextDirection = newDir;
		neighbourChoosed = true;
	}

	private void RandomNeighbourSelection() {
		List<Neighbours> tempNeighbours = new List<Neighbours>();

		while(tempNeighbours.Count <= 0) {
			int temp = Random.Range (0, 4);
			switch (temp) {
			case 0:
				if (currentProvince.northNeighbours == null)
					continue;
				else if (currentProvince.northNeighbours.Length > 0) {
					foreach (Neighbours n in currentProvince.northNeighbours)
						tempNeighbours.Add (n);
				}

				break;
			case 1:
				if (currentProvince.southNeighbours == null)
					continue;
				else if (currentProvince.southNeighbours.Length > 0) {
					foreach (Neighbours n in currentProvince.southNeighbours)
						tempNeighbours.Add (n);
				}

				break;
			case 2:
				if (currentProvince.eastNeighbours == null)
					continue;
				else if (currentProvince.eastNeighbours.Length > 0) {
					foreach (Neighbours n in currentProvince.eastNeighbours)
						tempNeighbours.Add (n);
				}

				break;
			case 3:
				if (currentProvince.westNeighbours == null)
					continue;
				else if (currentProvince.westNeighbours.Length > 0) {
					foreach (Neighbours n in currentProvince.westNeighbours)
						tempNeighbours.Add (n);
				}

				break;
			}
		}

		int t = Random.Range (0, tempNeighbours.Count);
		neighbour = tempNeighbours [t];
		neighbourChoosed = true;
	}

	public void GameOver() {
		gameOver = true;
		GenerationManager.Instance.displacementSpeed = 0f;
		PlayerMovement.Instance.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		ResultsScreen.Instance.ShowScreen ();
		GlobalData.Instance.coins += coins;
		GlobalData.Instance.kmRunned += totalKm;
		GlobalData.Instance.SaveGame ();
	}

	public void RoadChangeStarted() {
		onRoadChangeStarted ();
	}

	public void RoadChangeFinished() {
		onRoadChangeFinished ();
	}

	public void PauseResumeGame() {
		if (Time.timeScale > 0f) {
			Time.timeScale = 0f;

		}
		else
			Time.timeScale = 1f;
	}
}

public enum Direction {
	north,
	south,
	east,
	west
}
