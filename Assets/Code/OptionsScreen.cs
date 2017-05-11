using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour {

	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider fxSlider;
	[SerializeField] private GameObject musicMute;
	[SerializeField] private GameObject fxMute;

	private void Start() {
		musicSlider.value = SoundManager.Instance.GetCurrentMusicVolume () / 20f;
		fxSlider.value = SoundManager.Instance.GetCurrentFXVolume () / 20f;
		SoundManager.Instance.ChangeMusicVolume (musicSlider.value);
		SoundManager.Instance.ChangeFXVolume (fxSlider.value);
	}

	public void OnMusicChanged() {
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
		Application.OpenURL (url);
	}
}
