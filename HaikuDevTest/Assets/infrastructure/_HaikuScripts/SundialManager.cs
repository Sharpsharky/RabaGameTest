using UnityEngine;
using System.Collections;

public class SundialManager : MonoBehaviour {
	public SundialPiece[] sundialPieces;
	public PlayMakerFSM sendWonEvent;

	public void CheckIfWin () {
		foreach (SundialPiece sundialPiece in sundialPieces) {
			if (sundialPiece.IsCorrect()) {
				continue;
			} else {
				return;
			}
		}
		sendWonEvent.SendEvent("won");
	}
}
