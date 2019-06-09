using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TapToAddPieceManagerSaveable : MonoBehaviour {
	private List<TapToAddPieceSaveable> allPieces;
	public TapToAddPieceSaveable[] nonChildrenPieces; // Children are automatically added

	public PlayMakerFSM sendWonEvent;
	// Use this for initialization
	void Start () {
		allPieces = new List<TapToAddPieceSaveable>(GetComponentsInChildren<TapToAddPieceSaveable>());
		for (int i = 0; i < nonChildrenPieces.Length; i++) {
			allPieces.Add(nonChildrenPieces[i]);
		}
	}

	public void CheckIfAllCorrect() {
		foreach (TapToAddPieceSaveable piece in allPieces) {
			if (!piece.isCorrect) {
				return;
			}
		}
		foreach (TapToAddPieceSaveable piece in allPieces) {
			piece.puzzleWasWon = true;
		}
		sendWonEvent.SendEvent("won");
	}
}
