using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {

	private AudioSource audio;
	private bool waiting;
	private int nextFx;
	private AudioClip fxPlaying;

	public AudioClip[] effects;

	private void Awake() {
		audio = GetComponent<AudioSource> ();
	}

	private void Update() {
		if (waiting) {
			if (audio.isPlaying && audio.clip == fxPlaying) {
				return;
			}
			else {
				waiting = false;
				PlayFX (nextFx);
			}
		}
	}

	public void PlayFX(int fx) {
		audio.Stop ();
		audio.clip = effects [fx];
		audio.Play ();
	}

	public void WaitPlayFX(int fx) {
		if (audio.isPlaying) {
			fxPlaying = audio.clip;
			waiting = true;
			nextFx = fx;
		}
		else
			PlayFX (fx);
	}

	public float GetClipLength(int clip) {
		return effects [clip].length;
	}
}
