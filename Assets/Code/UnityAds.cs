using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAds : MonoBehaviour {

	public static UnityAds Instance;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy (gameObject);
		}

		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	public void ShowRewardedAd() {
		if (Advertisement.IsReady ("rewardedVideo")) {
			ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result) {
		Debug.Log ("DOUBLE BONUS");
	}
}
