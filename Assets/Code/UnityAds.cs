using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UnityAds : MonoBehaviourSingleton<UnityAds> {

	public void ShowTicketAd() {
		if (Advertisement.IsReady ("rewardedVideo")) {
			ShowOptions options = new ShowOptions { resultCallback = HandleTicketResult };
			Advertisement.Show ("rewardedVideo", options);
		}
	}

	public void ShowCoinAd() {
		if (Advertisement.IsReady ("rewardedVideo")) {
			ShowOptions options = new ShowOptions { resultCallback = HandleCoinResult };
			Advertisement.Show ("rewardedVideo", options);
		}
	}

	private void HandleTicketResult(ShowResult result) {
		switch (result) {
			case ShowResult.Failed:
				break;

			case ShowResult.Skipped:
				break;

			case ShowResult.Finished:
				GlobalData.Instance.tickets += 5;
				GameObject.Find ("TicketCount").transform.GetChild (0).GetComponent<Text> ().text = GlobalData.Instance.tickets.ToString ();
				GameObject.Find ("ProvinceSelectionScreen").GetComponent<ProvinceSelectionScreen> ().ShowTicketAdPopUp (false);
				break;
		}
	}

	private void HandleCoinResult(ShowResult result) {
		switch (result) {
			case ShowResult.Failed:
				break;

			case ShowResult.Skipped:
				break;

			case ShowResult.Finished:
				GlobalData.Instance.coins += SceneManager.Instance.coins;
				GameObject.Find ("ResultsScreen").GetComponent<ResultsScreen> ().ShowCoinAd (false);
				GameObject.Find ("ResultsScreen").GetComponent<ResultsScreen> ().RewardObtained ();
				break;
		}
	}
}
