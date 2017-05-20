using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	private bool showingRCHud;
	private bool muted;
	private float previousFXVol;
	private float previousMusicVol;

	public Text coinCounter;
	public Text kmCounter;
	public Text provinceName;
	public Text nextProvinceName;
	public GameObject leftProvince;
	public GameObject rightProvince;
	public GameObject frontProvince;
	public Animator rcHudAnim;
	public Animator topProvincesAnim;
	public GameObject pauseScreen;
	public Animator startTitle;
	public GameObject tutorial;
	public GameObject[] lifes;
	public GameObject muteButton;

	public static HUDManager Instance;

	private void Awake() {
		Instance = this;

		if ((SoundManager.Instance.GetCurrentMusicVolume () <= -80f) && (SoundManager.Instance.GetCurrentFXVolume () <= -80f))
			MuteButton ();
	}

	private void Start() {
		provinceName.text = SceneManager.Instance.currentProvince.name.ToUpper();

		if (GlobalData.Instance.firstTime) {
			Invoke ("StartTutorial", 2f);
		}

		startTitle.SetTrigger ("correr");
	}

	private void StartTutorial() {
		GlobalData.Instance.firstTime = false;
		tutorial.SetActive (true);
		tutorial.GetComponent<Animator> ().SetTrigger ("tutorial");
	}

	private void Update() {
		coinCounter.text = SceneManager.Instance.coins.ToString();
		kmCounter.text = (SceneManager.Instance.totalKm + SceneManager.Instance.provinceKm).ToString("F1") + " km";

		if (showingRCHud) {
			int lane = PlayerMovement.Instance.GetCurrentLane ();

			if (lane == 0) {
				leftProvince.GetComponent<Image> ().color = Color.white;
				rightProvince.GetComponent<Image> ().color = new Color(0.5f, 0.5f, 0.5f, 1f);
				frontProvince.GetComponent<Image> ().color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
			else if (lane == 1) {
				leftProvince.GetComponent<Image> ().color = new Color(0.5f, 0.5f, 0.5f, 1f);
				rightProvince.GetComponent<Image> ().color = new Color(0.5f, 0.5f, 0.5f, 1f);
				frontProvince.GetComponent<Image> ().color = Color.white;
			}
			else if (lane == 2) {
				leftProvince.GetComponent<Image> ().color = new Color(0.5f, 0.5f, 0.5f, 1f);
				rightProvince.GetComponent<Image> ().color = Color.white;
				frontProvince.GetComponent<Image> ().color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			PauseResumeGame ();
		}
	}

	public void LooseLife() {
		foreach (GameObject life in lifes) {
			if (life.GetComponent<Image> ().color.a >= 1f) {
				life.GetComponent<Image> ().color = new Color (1f, 1f, 1f, 0.2f);
				return;
			}
		}
	}

	public void ShowRoadChangeHUD(string[] provinces, string[] distances) {
		showingRCHud = true;
		float d = 0;

		if (provinces [0] != "null") {
			leftProvince.SetActive (true);
			leftProvince.transform.GetChild(0).GetComponent<Text> ().text = distances[0] + " km\n" + provinces [0].ToUpper();
		}
		else
			leftProvince.SetActive (false);

		if (provinces [1] != "null") {
			frontProvince.SetActive (true);
			frontProvince.transform.GetChild(0).GetComponent<Text> ().text = distances[1] + " km\n" + provinces [1].ToUpper();
		}
		else
			frontProvince.SetActive (false);

		if (provinces [2] != "null") {
			rightProvince.SetActive (true);
			rightProvince.transform.GetChild(0).GetComponent<Text> ().text = distances[2] + " km\n" + provinces [2].ToUpper();
		}
		else
			rightProvince.SetActive (false);

		rcHudAnim.SetBool ("active", true);
	}

	public void ShowNextProvince(string name) {
		showingRCHud = false;
		rcHudAnim.SetBool ("active", false);
		nextProvinceName.text = name.ToUpper();
		topProvincesAnim.SetTrigger("nextAppear");
	}

	public void ChangeProvince() {
		topProvincesAnim.SetTrigger ("change");
		Invoke ("ChangeTopNames", 0.2f);
	}

	public void ChangeTopNames() {
		provinceName.text = nextProvinceName.text;
	}

	public void PauseResumeGame() {
		if (Time.timeScale > 0f) {
			Time.timeScale = 0f;
			pauseScreen.SetActive (true);
			PlayerMovement.Instance.bloquedMov = true;
		}
		else {
			Time.timeScale = 1f;
			pauseScreen.SetActive (false);
			PlayerMovement.Instance.bloquedMov = false;
		}
	}

	public void HideTutorial() {
		tutorial.SetActive (false);
	}

	public void MuteButton() {
		if (!muted) {
			muted = true;
			previousFXVol = SoundManager.Instance.GetCurrentFXVolume ();
			previousMusicVol = SoundManager.Instance.GetCurrentMusicVolume ();
			SoundManager.Instance.MuteFX ();
			SoundManager.Instance.MuteMusic ();
			muteButton.transform.GetChild (0).gameObject.SetActive (false);
			muteButton.transform.GetChild (1).gameObject.SetActive (true);
		}
		else {
			muted = false;
			SoundManager.Instance.ChangeMusicVolume (previousMusicVol/SoundManager.Instance.maxAudioVolume);
			SoundManager.Instance.ChangeFXVolume (previousFXVol/SoundManager.Instance.maxAudioVolume);
			muteButton.transform.GetChild (0).gameObject.SetActive (true);
			muteButton.transform.GetChild (1).gameObject.SetActive (false);
		}
	}
}