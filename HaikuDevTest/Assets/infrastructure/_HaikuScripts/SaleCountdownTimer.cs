using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class SaleCountdownTimer : MonoBehaviour {
	public TextMeshPro textLabel;
	public string kSaleStringPlayerPrefsKey;
	private float timeLeft = 300;
	GameObject grandParent;

	void Start() {
		grandParent = gameObject.transform.parent.transform.parent.gameObject;
		grandParent.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0,1,1));
		if (PlayerPrefs.HasKey (kSaleStringPlayerPrefsKey)) {
			Debug.Log("Destroying sale");
			Destroy (grandParent);
		} else {
			PlayerPrefs.SetInt (kSaleStringPlayerPrefsKey, 1);
		}
		#if UNITY_ANDROID
		if (!HaikuBuildSettings.isGooglePlay) {
			Destroy(grandParent);
		}
		#endif
	}
	/*
	void OnLevelWasLoaded(int level) {
		GameObject[] allSales = GameObject.FindGameObjectsWithTag ("SaleIcon");
		if (allSales.Length > 1) {
			Debug.Log("Sale already exists");
			Destroy (grandParent);
		} else {
			HideOrShowUI ();
		}
	}

	private void HideOrShowUI() {
		string sceneName = Application.loadedLevelName;
		if (sceneName == "0Splash" ||
		    sceneName == "BoulderTest" ||
		    sceneName == "Level1b Barbican" ||
		    sceneName == "ActComplete" ||
		    sceneName == "leaveReview" ||
		    sceneName == "Intro" ||
		    sceneName == "SelectAct" ||
		    sceneName == "endingDialogue" ||
		    sceneName == "zEndingScene") {
			ShowUI (false);
		} else {
			ShowUI(true);
		}
	}

	private void ShowUI(bool showUI) {
		SpriteRenderer[] renderers = grandParent.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer renderer in renderers) {
			renderer.enabled = showUI;
		}
		Collider2D collider = gameObject.transform.parent.GetComponent<Collider2D> ();
		collider.enabled = showUI;
	}
	*/

	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		TimeSpan timeSpan = TimeSpan.FromSeconds (timeLeft);
		int seconds = timeSpan.Seconds;
		if (seconds < 10) {
			textLabel.text = timeSpan.Minutes + ":0" + seconds;
		} else {
			textLabel.text = timeSpan.Minutes + ":" + seconds;
		}

		if (timeLeft < 0) {
			Debug.Log("Destroying self");
			Destroy (grandParent);
		}
		if (timeLeft < 60) {
			if (seconds % 2 != 0) {
				textLabel.color = Color.red;
			} else {
				textLabel.color = Color.black;
			}
		}

	}

//	void UpdateLabel() {
//		DateTime.
//		textLabel.text = timeLeft.ToString ();
//
//	}
}
