using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StampCollectionScreen : MonoBehaviour {

	private int currentPage;
	private bool rotating;
	private int direction;
	private float angle;
	private int pageRotating;
	[SerializeField] private GameObject[] pages;
	[SerializeField] private GameObject pivot;
	[SerializeField] private GameObject nextButton;
	[SerializeField] private GameObject previousButton;
	[SerializeField] private Text coinsText;
	[SerializeField] private ScrollRect scroll;
	[SerializeField] private Animator unlockProvinceAnim;
	[SerializeField] private Sprite[] provincesShapes;
	private float scrollPos;
	private AudioPlayer audio;
	private bool animPlaying;

	public int[] stampPrices;

	private void Start() {
		audio = GetComponent<AudioPlayer> ();

		for (int i = 0; i < pages.Length; i++) {
			if (i != 0) {
				pages [i].transform.RotateAround (pivot.transform.position, transform.up, -90f);
				pages [i].SetActive (false);
			}

			for (int j = 0; j < 5; j++) {
				pages [i].transform.GetChild (j + 1).GetChild(0).GetComponent<Text> ().text = stampPrices [j].ToString ();
			}
		}

		currentPage = 0;
		previousButton.SetActive (false);

		coinsText.text = GlobalData.Instance.coins.ToString();
		List<int[]> su = GlobalData.Instance.stampsUnlocked;

		if (su != null && su.Count > 0) {
			for (int i = 0; i < su.Count; i++) {
				for (int j = 0; j < su [i].Length; j++) {
					if (su [i] [j] == 1) {
						UnlockStamp (i, j, false);
					}
				}
			}
		}

		CheckPrices (currentPage);

		SoundManager.Instance.ChangeMusic ("stampCollection");
	}

	private void Update() {
		if(rotating) {
			if (angle <= 90f) {
				pages [pageRotating].SetActive (true);
				if(currentPage > 0)
					pages [pageRotating - 1].SetActive (true);
				
				pages [pageRotating].transform.RotateAround (pivot.transform.position, transform.up, direction * 100f * Time.deltaTime);
				angle += 100f * Time.deltaTime;
			}
			else {
				rotating = false;
				pages [pageRotating].transform.eulerAngles = (direction == 1) ? Vector3.zero : new Vector3 (0f, -90f, 0f);

				if (direction == -1)
					pages [pageRotating].SetActive (false);
				else
					pages [pageRotating - 1].SetActive (false);
				
				angle = 0f;
				currentPage += 1 * direction;
				bool f = (currentPage == 0) ? false : true;
				previousButton.SetActive (f);
				f = (currentPage == pages.Length - 1) ? false : true;
				nextButton.SetActive (f);
			}
		}

		if (scroll.horizontalNormalizedPosition >= (scrollPos + 0.02f) || scroll.horizontalNormalizedPosition <= (scrollPos - 0.02f)) {
			audio.WaitPlayFX (2);
			scrollPos = scroll.horizontalNormalizedPosition;
		}

		if (animPlaying) {
			if (unlockProvinceAnim.GetCurrentAnimatorStateInfo (0).IsName ("unlockProvince") && unlockProvinceAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f) {
				animPlaying = false;
			}
		}
	}

	public void UnlockStamp(int stamp) {
		UnlockStamp (currentPage, stamp, true);
	}

	public void UnlockStamp(int page, int stamp, bool purchase) {
		if (!animPlaying) {
			if (purchase) {
				if (GlobalData.Instance.coins >= stampPrices [stamp] && GlobalData.Instance.stampsUnlocked [page] [stamp] == 0) {
					GlobalData.Instance.stampsUnlocked [page] [stamp] = 1;

					GlobalData.Instance.coins -= stampPrices [stamp];
					coinsText.text = GlobalData.Instance.coins.ToString ();

					int count = 0;
					for (int i = 0; i < 5; i++) {
						if (GlobalData.Instance.stampsUnlocked [page] [i] == 1) {
							count++;
						}
					}
					if (count == 5) {
						GlobalData.Instance.provincesUnlocked [page] = 1;
						if (currentPage != 27) {	//MADRID IS ALREADY UNLOCKED;
							unlockProvinceAnim.gameObject.transform.GetChild (1).GetComponent<Image> ().sprite = provincesShapes [currentPage];
							unlockProvinceAnim.gameObject.transform.GetChild (1).GetComponent<RectTransform> ().sizeDelta = new Vector2 (provincesShapes [currentPage].rect.width, provincesShapes [currentPage].rect.height);
							string txt = "¡" + pages [currentPage].transform.GetChild (0).GetComponent<Text> ().text + "\n" + "disponible!";
							unlockProvinceAnim.gameObject.transform.GetChild (2).GetComponent<Text> ().text = txt;
							unlockProvinceAnim.SetTrigger ("unlockProvince");
							animPlaying = true;
						}
						GlobalData.Instance.SaveGame ();
					}

					if (animPlaying)
						audio.PlayFX (4);
					else
						audio.PlayFX (3);

					CheckPrices (currentPage);
				}
				else
					return;
			}

			GameObject obj = transform.GetChild (page).GetChild (stamp + 1).gameObject;
			obj.transform.GetChild (4).gameObject.SetActive (false);
			string[] t = obj.transform.GetChild (3).GetComponent<Image> ().sprite.name.Split ('_');
			obj.transform.GetChild (0).GetComponent<Text> ().text = t [1];
			obj.transform.GetChild (1).gameObject.SetActive (false);

			GlobalData.Instance.SaveGame ();
		}
	}

	private void CheckPrices(int page) {
		for (int i = 0; i < 5; i++) {
			if (GlobalData.Instance.coins < stampPrices [i] && GlobalData.Instance.stampsUnlocked[page][i] == 0) {
				GameObject obj = transform.GetChild (page).GetChild (i + 1).gameObject;
				obj.transform.GetChild (0).GetComponent<Text> ().color = Color.red;
			}
			else {
				GameObject obj = transform.GetChild (page).GetChild (i + 1).gameObject;
				obj.transform.GetChild (0).GetComponent<Text> ().color = Color.black;
			}
		}
	}

	public void NextPage() {
		if (!rotating && !animPlaying) {
			pageRotating = currentPage + 1;
			rotating = true;
			direction = 1;
			angle = 0f;
			audio.PlayFX (1);
			CheckPrices (pageRotating);
		}
	}

	public void PreviousPage() {
		if (!rotating && !animPlaying) {
			pageRotating = currentPage;
			rotating = true;
			direction = -1;
			angle = 0f;
			audio.PlayFX (1);
			CheckPrices (currentPage - 1);
		}
	}

	public void GoToPage(int page) {
		if (!animPlaying) {
			if (page == currentPage)
				return;

			direction = (currentPage < page) ? 1 : -1;

			if (direction == 1) {
				for (int i = currentPage + 1; i <= page; i++) {
					pages [i].transform.RotateAround (pivot.transform.position, transform.up, direction * 90f);
					pages [i - 1].SetActive (false);
					if (i == page)
						pages [i].SetActive (true);
				}
			}
			else {
				for (int i = currentPage; i > page; i--) {
					pages [i].transform.RotateAround (pivot.transform.position, transform.up, direction * 90f);
					pages [i].SetActive (false);
				}

				pages [page].SetActive (true);
			}

			currentPage = page;
			bool f = (currentPage == 0) ? false : true;
			previousButton.SetActive (f);
			f = (currentPage == pages.Length - 1) ? false : true;
			nextButton.SetActive (f);

			audio.PlayFX (1);

			CheckPrices (page);
		}
	}

	public void BackToMainMenu() {
		if (!animPlaying) {
			audio.PlayFX (0);
			UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
		}
	}
}
