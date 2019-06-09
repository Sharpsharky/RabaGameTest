using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InputEventNS;

public enum TabletChipOrientation {
	up, down
};

public class TabletPuzzleChip : MonoBehaviour {

	private enum ChipState {
		normal, off, on
	};

	public int chipNumber;
	public TabletChipOrientation chipOrientation;
	public GameObject goManager;

	private bool isTouched = false;
	private bool isDragged = false;
	private Vector3 offset;
	private TabletPuzzleManager manager;
	private Vector3 centerOffset;

	// Use this for initialization
	void Start () {
		this.manager = goManager.GetComponent<TabletPuzzleManager>();
		InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, TouchOrMouseChange, TouchOrMouseEnd, 1.0f);

		Vector3 worldCenter = this.goManager.GetComponent<BoxCollider2D>().bounds.center;
		Vector3 chipCenter = this.GetComponent<BoxCollider2D>().bounds.center;
		this.centerOffset = worldCenter - chipCenter;
	}

	void Update () {}

	// Public Methods

	// Bring back to the original position and default sprite
	public void reset() {
		this.GetComponent<SpriteRenderer>().sprite = this.spriteForChipState(ChipState.normal);
		Vector3 worldCenter = this.goManager.GetComponent<BoxCollider2D>().bounds.center;
		this.transform.position = worldCenter - this.centerOffset;
	}

	public void turnOn() {
		this.GetComponent<SpriteRenderer>().sprite = this.spriteForChipState(ChipState.on);
	}

	public void turnOff() {
		this.GetComponent<SpriteRenderer>().sprite = this.spriteForChipState(ChipState.off);
	}

	// Private Methods

	void TouchOrMouseStart(InputHandler handler) {
		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);

		// Make sure you are touching this object
		if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
			this.GetComponent<SpriteRenderer>().sortingOrder = 10;
			this.isTouched = true;
			this.isDragged = false;
			this.offset = gameObject.transform.position - wp;
		}
	}

	void TouchOrMouseChange(InputHandler handler) {
        if (this.isTouched) {
			this.isDragged = true;

            InputHandlerPointer pointer = (InputHandlerPointer)handler;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
            Vector3 newPos = worldPosition + this.offset;
        
			gameObject.transform.position = new Vector3 (newPos.x, newPos.y, gameObject.transform.position.z);		
		}
	}

	void TouchOrMouseEnd (InputHandler handler) {
		if (this.isTouched) {
			this.isTouched = false;

			if (!this.isDragged) { // Tap
				this.rotateChip();
			} else { // Drag
				if (!this.manager.checkChipDidSnap(this)) {
					this.reset();
				} 
			}
			this.GetComponent<SpriteRenderer>().sortingOrder = 5;
		}
	}

	private void rotateChip() {
		Vector3 rotation = new Vector3(0,0,180);
		iTween.RotateAdd(gameObject, iTween.Hash("amount", rotation, "time", 0.3, "oncomplete", "OnMoveComplete", "oncompletetarget", gameObject));
	}

	public void OnMoveComplete() {
		this.chipOrientation = (this.chipOrientation == TabletChipOrientation.up) ? TabletChipOrientation.down : TabletChipOrientation.up;
	}

	Sprite spriteForChipState(ChipState state) {
		switch (state) {
			case ChipState.normal: return Resources.Load<Sprite>("TabletPuzzle/TabletChip" + this.chipNumber);
			case ChipState.off: return Resources.Load<Sprite>("TabletPuzzle/TabletChip" + this.chipNumber + "_off");
			case ChipState.on: return Resources.Load<Sprite>("TabletPuzzle/TabletChip" + this.chipNumber + "_on");
			default: return null;
		}
	}

}
