using UnityEngine;
using System.Collections;

public class CycleThroughImagesMultipleEvent : MonoBehaviour {
	private CycleThroughImagesMultipleEventPiece[] allPieces;
	public string[] eventNames;
	public string[] spriteIndexs;
	public PlayMakerFSM sendEventTo;

	// Use this for initialization
	void Start () {
		allPieces = GetComponentsInChildren<CycleThroughImagesMultipleEventPiece>();
	}

	public void ResetPuzzle() {
		foreach (CycleThroughImagesMultipleEventPiece piece in allPieces) {
			piece.ResetPiece();
		}
	}
	
	public void CheckIfEvent() {
		// We can send the event multiple times.  Handle in the FSM
		string indexString = "";
		for (int i = 0; i < allPieces.Length; i++) {
			indexString += allPieces[i].currentSprite;
		}
		Debug.Log("INdexstring: " + indexString);
		for (int i = 0; i < eventNames.Length; i++) {
			if (spriteIndexs[i].Equals(indexString)) {
				sendEventTo.SendEvent(eventNames[i]);
			}
		}
	}
}
