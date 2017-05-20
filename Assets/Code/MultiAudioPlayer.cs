using UnityEngine;
using System.Collections;

public class MultiAudioPlayer : MonoBehaviour {
	
	private AudioSource[] audio;
	private bool[] waiting;
	private int[] nextFx;
	private AudioClip[] fxPlaying;

	public AudioClip[] effects;

	private void Awake() {
		audio = GetComponents<AudioSource> ();

		waiting = new bool[audio.Length];
		nextFx = new int[audio.Length];
		fxPlaying = new AudioClip[audio.Length];
	}

	private void Update() {
		for (int i = 0; i < audio.Length; i++) {
			if (waiting [i]) {
				if (audio [i].isPlaying && audio [i].clip == fxPlaying [i]) {
					return;
				}
				else {
					waiting [i] = false;
					PlayFX (nextFx [i], i);
				}
			}
		}
	}

	public void PlayFX(int fx, int source) {
		audio[source].Stop ();
		audio[source].clip = effects [fx];
		audio[source].Play ();
	}

	public void WaitPlayFX(int fx, int source) {
		if (audio[source].isPlaying) {
			fxPlaying[source] = audio[source].clip;
			waiting[source] = true;
			nextFx[source] = fx;
		}
		else
			PlayFX (fx, source);
	}

	public float GetClipLength(int clip) {
		return effects [clip].length;
	}
}
