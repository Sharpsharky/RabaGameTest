using UnityEngine;
using System.Collections;
using InputEventNS;

public class StatueSortingOrderException : MonoBehaviour {
	public SpriteRenderer leftLeg;
	public SpriteRenderer rightLeg;

	public Sprite leftLegException;
	public Sprite rightLegException;

	private InputHandler touchOrMouseListener;

	// Use this for initialization
	void Start () {
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, null, null, 1.0f) ;
	}
	
	// Update is called once per frame
	void TouchOrMouseStart (InputHandler handler) {
		if (leftLeg.sprite.Equals(leftLegException) &&
			rightLeg.sprite.Equals(rightLegException)) {
			leftLeg.sortingOrder = 0;
			rightLeg.sortingOrder = -1;
		} else {
			leftLeg.sortingOrder = -1;
			rightLeg.sortingOrder = 0;
		}
	}

	void OnDisable() {
		InputEvent.RemoveListener(touchOrMouseListener);
	}
}
