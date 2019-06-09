using UnityEngine;
using System.Collections;

public class TapToAddPieceManager : MonoBehaviour {
	private TapToAddPiece[] allPieces;

	public PlayMakerFSM sendWonEvent;
	// Use this for initialization
	void Start () {
		allPieces = GetComponentsInChildren<TapToAddPiece>();
	}

	public void CheckIfAllCorrect() {
		foreach (TapToAddPiece piece in allPieces) {
			if (!piece.isCorrect) {
				Debug.Log("Incorrect piece at : " + piece.name);
				return;
			}
		}
		sendWonEvent.SendEvent("won");
	}
}
