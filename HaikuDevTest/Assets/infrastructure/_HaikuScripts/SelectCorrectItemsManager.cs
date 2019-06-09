using UnityEngine;
using System.Collections;
using InputEventNS;

public class SelectCorrectItemsManager : MonoBehaviour {
	private InputHandler touchOrMouseListener;
	public SelectCorrectItemPiece[] pieces;

	// Use this for initialization
	void Start () {
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, TouchOrMouseChange, TouchOrMouseEnd, 1.0f) ;

	}
	
	void TouchOrMouseStart(InputHandler handler)
	{
		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);
		
		foreach (SelectCorrectItemPiece piece in pieces) {
			if (piece.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
				piece.isSelected = !piece.isSelected;
				CheckIfWin();
			}
		}
	}
	void TouchOrMouseChange(InputHandler handler) {
	}

	void TouchOrMouseEnd(InputHandler handler) {
	}
		
		
	void CheckIfWin() {
		foreach (SelectCorrectItemPiece piece in pieces) {
			if (piece.isCorrect) {
				if (!piece.isSelected) {
					Debug.Log("Correct piece " + piece.name + " not selected");
					return;
				}
			} else {
				if (piece.isSelected) {
					Debug.Log("Not correct piece selected: " + piece.name);
					return;
				}
			}
		}
		GetComponent<PlayMakerFSM>().SendEvent("won");
		Destroy(this);
	}
	
	// Update is called once per frame
	void OnDisable () {
		InputEvent.RemoveListener(touchOrMouseListener);
	}
}
