using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using InputEventNS;

public abstract class Draggable : MonoBehaviour {

	private enum AxisDirection {
		none, vertical, horizontal
	};

	private Vector3 previousPosition;
	private Vector3 offset;

	private bool isTouched = false;
	private AxisDirection direction = AxisDirection.none;
	private Vector3 startingPosition;
 
	protected bool onlyMoveAlongAxis = false;

	// Use this for initialization
	protected virtual void Start() {
		 InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, TouchOrMouseChange, TouchOrMouseEnd, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {  }

	// Abstract methods

	public abstract void DragDidStart();

	public abstract void DragDidContinue(Vector3 position);

	public abstract void DragDidEnd();

	// Private methods

	void TouchOrMouseStart (InputHandler handler) {
		// Reset states
		this.isTouched = false;
		this.direction = AxisDirection.none;

		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPosition = new Vector2(worldPoint.x, worldPoint.y);
		this.offset = gameObject.transform.position - worldPoint;

		if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition)) {
			this.isTouched = true;
			this.previousPosition = gameObject.transform.position;
			this.DragDidStart();
		}
	}
       
	void TouchOrMouseChange (InputHandler handler) {
		if (!this.isTouched) { return; }

        InputHandlerPointer pointer = (InputHandlerPointer)handler;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector3 newPosition = new Vector3(worldPosition.x + offset.x, worldPosition.y + offset.y, gameObject.transform.position.z);
		Vector3 deltaPosition = this.previousPosition - newPosition;

		// This will lock the movement along one of the two axis
		if (this.onlyMoveAlongAxis) {
			this.direction = Math.Abs(deltaPosition.x) > Math.Abs (deltaPosition.y) ? AxisDirection.horizontal : AxisDirection.vertical;

			float xPos = (this.direction == AxisDirection.horizontal) ? (worldPosition.x + offset.x) : transform.position.x;
			float yPos = (this.direction == AxisDirection.vertical) ? (worldPosition.y + offset.y) : transform.position.y;
			newPosition = new Vector3(xPos, yPos, gameObject.transform.position.z);
		}
				
		this.previousPosition = newPosition;
		this.DragDidContinue(newPosition);
	}

	void TouchOrMouseEnd (InputHandler handler) {
		if (!this.isTouched) { return; }

		this.DragDidEnd();
	}

}
