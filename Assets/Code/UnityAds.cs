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
}
