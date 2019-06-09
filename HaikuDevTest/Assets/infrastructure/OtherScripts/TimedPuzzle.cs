using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class TimedPuzzle : MonoBehaviour {
	float kTimePerBar = 5;
	float timeLeft;
	int bars = 5;
	TextMeshPro textLabel;

	// Use this for initialization
	void Start () {
		timeLeft = kTimePerBar;
		textLabel = gameObject.GetComponentInChildren<TextMeshPro> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		TimeSpan timeSpan = TimeSpan.FromSeconds (timeLeft);
		int seconds = timeSpan.Seconds;

		/*
		if (seconds < 10) {
			textLabel.text = timeSpan.Minutes + ":0" + seconds;
		} else {
			textLabel.text = timeSpan.Minutes + ":" + seconds;
		}
		*/
		
		if (timeLeft < 0) {
			if (bars > 0) {
				bars--;
				timeLeft = kTimePerBar;
				textLabel.text = bars.ToString();
			} else {
				Debug.Log("Destroying self");
				Destroy (gameObject);
			}
		}

		float scale = timeLeft / kTimePerBar;
		Debug.Log ("Scale " + scale);
//		gameObject.transform.localScale.Set (scale, 1, 1);
		gameObject.transform.localScale = new Vector3 (scale, 1.0f, 1.0f);
	}

	void PuzzleComplete() {

        //Todo: Send collect stars transaction and wait for result
		//ChapterUIManager.instance.moteManager.UpdateMoteCount (bars);
		Destroy (gameObject);
	}
}
