using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour {

	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider fxSlider;
	[SerializeField] private GameObject musicMute;
	[SerializeField] private GameObject fxMute;
	private AudioPlayer audio;

	private void Start() {
		audio = GetComponent<AudioPlayer> ();

		musicSlider.value = SoundManager.Instance.GetCurrentMusicVolume () / 20f;
		fxSlider.value = SoundManager.Instance.GetCurrentFXVolume () / 20f;
		SoundManager.Instance.ChangeMusicVolume (musicSlider.value);
		SoundManager.Instance.ChangeFXVolume (fxSlider.value);
	}

	public void OnMusicChanged() {
		if(audio != null)
			audio.WaitPlayFX (0);

		if (musicSlider.value <= -1) {
			musicMute.SetActive (true);
			SoundManager.Instance.MuteMusic ();
		}
		else {
			musicMute.SetActive (false);
			SoundManager.Instance.ChangeMusicVolume (musicSlider.value);
		}
	}

	public void OnFXChanged() {
		if(audio != null)
			audio.WaitPlayFX (0);

		if (fxSlider.value <= -1) {
			fxMute.SetActive (true);
			SoundManager.Instance.MuteFX ();
		}
		else {
			fxMute.SetActive (false);
			SoundManager.Instance.ChangeFXVolume (fxSlider.value);
		}
	}

	public void OpenURL(string url) {
		audio.PlayFX (1);
		Application.OpenURL (url);
	}
}
