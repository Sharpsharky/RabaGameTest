using UnityEngine;
using System.Collections;
using InputEventNS;
using System.Linq;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using System;

public class TapInCorrectOrderMoveObjectManager : MonoBehaviour {
	public Collider2D[] correctPieceOrder;
	private Collider2D[] allPieces;
	private int index;
	private Collider2D[] piecesTapped;
	private InputHandler touchOrMouseListener;
	public AudioClip errorSound;
	public AudioClip selectedSound;

	private bool isAnimating = false;

	public PlayMakerFSM managerFSM;
	public Vector3 movePieceAmount;

	private Collider2D lastPieceTapped;
	public bool resetOnError = false;

	private List<Collider2D> movedPieces = new List<Collider2D>();

	// Use this for initialization
	void Start () {
		allPieces = GetComponentsInChildren<Collider2D>();
		Debug.Log("All pieces in WaterCave: " + allPieces.Length);
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, null, null, 1.0f);
		piecesTapped = new Collider2D[correctPieceOrder.Length];
	}


	void TouchOrMouseStart(InputHandler handler) {
		if (isAnimating) return;
		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);

		foreach (Collider2D pieceCollider in allPieces) {
			if (pieceCollider == Physics2D.OverlapPoint(touchPos)) {
				lastPieceTapped = pieceCollider;
				if (selectedSound) {
					Helper.PlayAudioIfSoundOn(selectedSound);
				}
				HandlePieceTapped();
			}
		}
	}

	private void HandlePieceTapped() {
		// Enable or disable the collider based on isEnablePieceWhenTapped
		if (index < piecesTapped.Length)
		{
//			Debug.Log("Piece tapped: " + lastPieceTapped.name);
			piecesTapped[index] = lastPieceTapped;
			MovePiece(lastPieceTapped);
			movedPieces.Add(lastPieceTapped);
		}
	}
	
	private void MovePiece(Collider2D col) {
		isAnimating = true;
		col.enabled = false;
		GameObject pieceGameObject = col.gameObject;
		iTween.MoveBy(pieceGameObject, iTween.Hash("amount", movePieceAmount, "time", 0.5f, "oncompletetarget", gameObject, "oncomplete", "MoveComplete"));
	}

	private void MoveComplete() {
		isAnimating = false;
		Collider2D correctCollider = correctPieceOrder[index];
		if (resetOnError && (lastPieceTapped != correctCollider)) {
			HandleLoss();
		} else {
			Debug.Log("Tap in correct order index is: " + index);
			index++;
			if (index >= correctPieceOrder.Length)
			{
				CheckIfWin();
			}
		}
	}

	void CheckIfWin() {
		bool didWin = correctPieceOrder.SequenceEqual(piecesTapped);
		if (didWin) {
			if(managerFSM!= null) {
				managerFSM.SendEvent("won");
				InputEvent.RemoveListener(touchOrMouseListener);
			}
		} else {
			HandleLoss();	
		}
	}

	private void HandleLoss() {
		index = 0;
		Helper.PlayAudioIfSoundOn(errorSound);

		foreach (Collider2D pieceCollider in movedPieces) {
			GameObject pieceGameObject = pieceCollider.gameObject;
			pieceCollider.enabled = true;
			iTween.MoveBy(pieceGameObject, -movePieceAmount, 0.5f);
		}
		movedPieces.Clear();
	}

	void OnDisable() {
		InputEvent.RemoveListener(touchOrMouseListener);
	}
}