using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviourSingleton<SoundManager> {

	private bool fadingOut;
	private bool fadingIn;
	private string nextClip;
	private float currentVolume;
	private float startingVolume;

	public float maxAudioVolume;
	public AudioMixer audio;
	public AudioClip mainMenuMusic;
	public AudioClip stampCollectionMusic;
	public AudioClip oceanicClimateMusic;
	public AudioClip continentalClimateMusic;
	public AudioClip mediterraneanClimateMusic;

	public AudioSource music;
	public AudioSource fx;

	private void Awake() {
		maxAudioVolume = 20f;

		AudioSource[] audios = GetComponents<AudioSource> ();
		music = audios [0];
		fx = audios [1];

		mainMenuMusic = Resources.Load<AudioClip> ("Audio/Music/main_menu_music");
		stampCollectionMusic = Resources.Load<AudioClip> ("Audio/Music/stamp_collection_music");
		oceanicClimateMusic = Resources.Load<AudioClip> ("Audio/Music/oceanic_climate_music");
		continentalClimateMusic = Resources.Load<AudioClip> ("Audio/Music/continental_climate_music");
		mediterraneanClimateMusic = Resources.Load<AudioClip> ("Audio/Music/mediterranean_climate_music");
		audio = music.outputAudioMixerGroup.audioMixer;
	}

	private void Update() {
		if (fadingOut) {
			currentVolume -= 10f * Time.deltaTime;
			audio.SetFloat ("MusicVolume", currentVolume);
			if (currentVolume <= -20f) {
				currentVolume = -20f;
				audio.SetFloat ("MusicVolume", -20f);
				fadingOut = false;
				ChangeMusic (nextClip);
				fadingIn = true;
			}
		}

		if (fadingIn) {
			currentVolume += 10f * Time.deltaTime;
			audio.SetFloat ("MusicVolume", currentVolume);
			if (currentVolume >= startingVolume) {
				fadingIn = false;
				currentVolume = startingVolume;
				audio.SetFloat ("MusicVolume", currentVolume);
			}
		}
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

	public void FadeOut(string clip) {
		fadingOut = true;
		nextClip = clip;
		audio.GetFloat ("MusicVolume", out currentVolume);
		startingVolume = currentVolume;
	}

	public void ChangeMusicVolume(float volume) {
		audio.SetFloat ("MusicVolume", volume * maxAudioVolume);
	}

	public void ChangeFXVolume(float volume) {
		audio.SetFloat ("FXVolume", volume * maxAudioVolume);
	}

	public void MuteMusic() {
		audio.SetFloat ("MusicVolume", -80f);
	}

	public void MuteFX() {
		audio.SetFloat ("FXVolume", -80f);
	}

	public float GetCurrentMusicVolume() {
		float v;
		audio.GetFloat ("MusicVolume", out v);
		return v;
	}

	public float GetCurrentFXVolume() {
		float v;
		audio.GetFloat ("FXVolume", out v);
		return v;
	}
}
