using UnityEngine;
using System.Collections;
using InputEventNS;

public class TouchDragRelative : MonoBehaviour {
	private bool isTouched = false;
	private Vector3 offset = new Vector3(0, 0, 0);

	private InputHandler touchOrMouseListener;

	public Collider2D colliderBounds;

	public bool clampX = false;
	public bool clampY = false;

	private float maxX;
	private float maxY;
	private float minX;
	private float minY;

	// Use this for initialization
	void Start () {
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, TouchOrMouseChange, TouchOrMouseEnd, 1.0f) ;


	}
	
	// Update is called once per frame
	void TouchOrMouseStart (InputHandler handler) {
		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);

		if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
			offset = gameObject.transform.position - wp;
			isTouched = true;
		}
	}

	void TouchOrMouseChange (InputHandler handler) {
		if (isTouched) {
			InputHandlerPointer pointer = (InputHandlerPointer)handler;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
			Vector3 newPos = worldPosition + offset;

			float newX = newPos.x;
			float newY = newPos.y;

			if (colliderBounds != null) {
				Bounds bounds = colliderBounds.bounds;
				maxX = bounds.max.x;
				minX = bounds.min.x;
				maxY = bounds.max.y;
				minY = bounds.min.y;

				newX = Mathf.Clamp(newX, minX, maxX);
				newY = Mathf.Clamp(newY, minY, maxY);
//				Debug.Log("MaxX: " + maxX + " MinX: " + minX + " maxY: " + maxY + " minY: " + minY + " newX: " + newX + " newY: " + newY);
			}

			if (clampX) newX = gameObject.transform.position.x;
			if (clampY) newY = gameObject.transform.position.y;

			gameObject.transform.position = new Vector3(newX, newY, gameObject.transform.position.z);
		}
	}

	void TouchOrMouseEnd (InputHandler handler) {
		if (isTouched) {
			isTouched = false;
		}
	}

	void OnDisable() {
		InputEvent.RemoveListener(touchOrMouseListener);
	}
}
