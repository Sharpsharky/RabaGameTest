using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarDiagnosticBar : MonoBehaviour {
	public Transform startPosition;
	public Transform endPosition;
	public GameObject oneBar;

	private int numberOfBars;
	private List<GameObject> allBars = new List<GameObject>();
	// Use this for initialization

	public void InitializeWithUnits(int bars) {
		Debug.Log("Initializing Diagnostic Bar with bars: " + bars);
		numberOfBars = bars;
		for (int i = 0; i < bars; i++) {
			GameObject newBar = (GameObject) Instantiate(oneBar, new Vector3(0f, 0f, 0f), Quaternion.identity);
			newBar.transform.parent = this.transform;
			allBars.Add(newBar);
		}
		LayoutBars();
		ShowBars(0);
	}

	private void LayoutBars() {
		for (int i = 0; i < allBars.Count; i++) {
			GameObject bar = allBars[i];
			bar.transform.localPosition = new Vector3(startPosition.localPosition.x,
			                                          (startPosition.localPosition.y + (endPosition.localPosition.y - startPosition.localPosition.y) / (float) numberOfBars * i),
			                                          startPosition.localPosition.z);
		}
	}

	public void ShowBars(int bars) {
		for (int i = 0; i < allBars.Count; i++) {
			GameObject bar = allBars[i];
			SpriteRenderer barRenderer = bar.GetComponent<SpriteRenderer>();
			if (i < bars) {
				barRenderer.enabled = true;
			} else {
				barRenderer.enabled = false;
			}
		}
	}
}