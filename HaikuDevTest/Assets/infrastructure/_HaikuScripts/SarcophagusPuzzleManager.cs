using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SarcophagusPuzzleManager : MonoBehaviour {

	// Private properties
	private SarcophagusJar selectedJar = null;

	// Public properties
	public List<SarcophagusQuadrant> quadrants;
	public GameObject overlay;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	private void checkWon() {
		foreach (SarcophagusQuadrant q in this.quadrants) {
			if (q.type != SarcophagusQuadrantType.center) {
				if (q.correctJar != q.currentJar) { return; }
			}
		}
		// Won
		Debug.Log("Sarcophagus puzzle completed");
		gameObject.GetComponent<PlayMakerFSM>().SendEvent("won");
	}

	private bool isMovementValid(SarcophagusQuadrant from, SarcophagusQuadrant to) {
		List<SarcophagusQuadrantType> types;

		switch (from.type) {
			case SarcophagusQuadrantType.topLeft: 
			types = new List<SarcophagusQuadrantType> { SarcophagusQuadrantType.topRight, SarcophagusQuadrantType.bottomLeft, SarcophagusQuadrantType.center };
			break;
			case SarcophagusQuadrantType.topRight: 
			types = new List<SarcophagusQuadrantType> { SarcophagusQuadrantType.topLeft, SarcophagusQuadrantType.bottomRight, SarcophagusQuadrantType.center };
			break;			
			case SarcophagusQuadrantType.bottomLeft: 
			types = new List<SarcophagusQuadrantType> { SarcophagusQuadrantType.topLeft, SarcophagusQuadrantType.bottomRight };
			break;			
			case SarcophagusQuadrantType.bottomRight:
			types = new List<SarcophagusQuadrantType> { SarcophagusQuadrantType.bottomLeft, SarcophagusQuadrantType.topRight };
			break;			
			case SarcophagusQuadrantType.center:
			types = new List<SarcophagusQuadrantType> { SarcophagusQuadrantType.topLeft, SarcophagusQuadrantType.topRight };
			break;			
			default: return false;
		}

		return types.Contains(to.type);
	}

	// Public methods

	public void didTapJar(SarcophagusJar jar) {
		// Reset if tapping the same jar twice
		this.selectedJar = (this.selectedJar == jar) ? null : jar;
		Debug.Log("Jar selected:" + this.selectedJar);
	}

	public void didTapQuadrant(SarcophagusQuadrant quadrant) {
		Debug.Log("User did tap Quadrant:" + quadrant.type);
		Debug.Log(quadrant.gameObject);

		// If nothing is selected, nothing happens,
		// or we want to move a jar to its own location
		if (this.selectedJar == null ||
			this.selectedJar.type == quadrant.currentJar) {
			Debug.Log("No Jar selected");
			return; 
		}

		// If tapping a spot take by another jar, we show error
		if (quadrant.currentJar != JarType.none) { 
			Debug.Log("Quadrant is already occupied by another jar");
			gameObject.GetComponent<PlayMakerFSM>().SendEvent("error");
			return;
		}

		if (!this.isMovementValid(this.selectedJar.currentQuadrant, quadrant)) {
			Debug.Log("Jar cannot move to that quadrant");
			return;	
		}

		// Else we slide the jar to the new positon
		Debug.Log("Sliding Jar:" + this.selectedJar.type + " to quadrant:" + quadrant.type);
		this.overlay.SetActive(true);
		Vector3 qPosition = quadrant.gameObject.GetComponent<BoxCollider2D>().bounds.center;
		qPosition.z = this.selectedJar.gameObject.GetComponent<BoxCollider2D>().bounds.center.z;
		iTween.MoveTo(this.selectedJar.gameObject, iTween.Hash("position", qPosition, "time", 0.75, "easeType", "linear", "oncomplete", "OnMoveComplete", "oncompletetarget", gameObject));
		gameObject.GetComponent<PlayMakerFSM>().SendEvent("play sound");

		// Assign jar to the quadrant it moved to
		quadrant.currentJar = this.selectedJar.type;
		// Assign to the jar the new quadrant it occupies
		this.selectedJar.currentQuadrant.currentJar = JarType.none;
		this.selectedJar.currentQuadrant = quadrant;
		// Deselect current jar
		this.selectedJar = null;
	}

	public void OnMoveComplete() {
		// Disable overlay
		this.overlay.SetActive(false);
		this.checkWon();
	}

}
