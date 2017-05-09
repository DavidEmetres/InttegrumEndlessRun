using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip mainMenuMusic;
	public AudioClip stampCollectionMusic;
	public AudioClip oceanicClimateMusic;
	public AudioClip continentalClimateMusic;
	public AudioClip mediterraneanClimateMusic;

	public AudioSource music;
	public AudioSource fx;

	public static SoundManager Instance;

	private void Awake() {
		if (Instance != null && Instance != this)
			Destroy (gameObject);

		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	public void ChangeMusic(string clip) {
		music.Stop ();
		switch (clip) {
			case "mainMenu":
				music.clip = mainMenuMusic;
				break;
			case "stampCollection":
				music.clip = stampCollectionMusic;
				break;
			case "oceanicClimate":
				music.clip = oceanicClimateMusic;
				break;
			case "continentalClimate":
				music.clip = continentalClimateMusic;
				break;
			case "mediterraneanClimate":
				music.clip = mediterraneanClimateMusic;
				break;
		}
		music.Play ();
	}
}
