using UnityEngine;
using System.Collections;
using System;

public class CycleThroughImagesManager : PuzzleController {
	private CycleThroughImages[] _allPieces;

	public int numberCorrect{ get { return _numberCorrect; } }
	private int _numberCorrect;

	public bool preventWonEvent = false; // Used only if the puzzle is accessible but the player cannot win it.
   
	public Action winEvent;

	[SerializeField,Tooltip("Localization keys for text to show when an alternitve image is showing that is " +
		"not the correct solution. Length and order must match CycleThroughImages.alternativeSolutionSprites.")]
	private string[] _alternativeImageLocKeys;

	// Use this for initialization
	void Start () {
		_allPieces = GetComponentsInChildren<CycleThroughImages>();
	}

	public void UpdateNumberCorrect() {
		_numberCorrect = 0;
		foreach (CycleThroughImages piece in _allPieces) {
			if (piece.isCorrect()) {
				_numberCorrect++;
			}
		}
	}

	public override void ResetPuzzle() {
		Debug.Log ("Reset");
		foreach (CycleThroughImages piece in _allPieces) {
			piece.ResetPiece();
		}
		UpdateNumberCorrect();
	}

	public void CheckIfWin() {

		foreach (CycleThroughImages piece in _allPieces) {
			if (!piece.isCorrect()) {
				//Debug.Log("Incorrect Cycle Piece: " + piece.name);
				return;
			}
		}

		if (!preventWonEvent) {
			Win ();
		} 
	}

	public void CheckIfAlternativeImage(int pAlternativeImageIndex){
		foreach (CycleThroughImages piece in _allPieces) {
			if (!piece.isShowingAlternativeImage (pAlternativeImageIndex)) {
				return;
			} 
		}
			
		if (pAlternativeImageIndex >= 0 && pAlternativeImageIndex < _alternativeImageLocKeys.Length) {
			Helper.LocalizeKeyToTopBar (_alternativeImageLocKeys [pAlternativeImageIndex]);
		}
	}

	protected override void Win () {
		base.Win ();
		foreach (CycleThroughImages piece in _allPieces) {
			piece.enabled = false;
		}
	}

	public override void Skip () {
		foreach (CycleThroughImages piece in _allPieces) {
			piece.SetCorrect ();
		}
		Win ();
	}
}
