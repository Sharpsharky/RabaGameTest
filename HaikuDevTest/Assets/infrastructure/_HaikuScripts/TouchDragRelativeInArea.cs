using UnityEngine;
using System.Collections;
using InputEventNS;

public class TouchDragRelativeInArea : MonoBehaviour {

	private bool isTouched = false;
	private Vector3 offset = new Vector3(0, 0, 0);
	
	private InputHandler touchOrMouseListener;
	// Use this for initialization
	void Start () {
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, TouchOrMouseChange, TouchOrMouseEnd, 1.0f) ;
	}
	
	// Update is called once per frame
	void TouchOrMouseStart (InputHandler handler) {
		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);
		LayerMask mask = 1 << 13;
		mask = ~mask;
		if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos,mask)) {
			offset = gameObject.transform.position - wp;
			isTouched = true;
		}
	}
	
	void TouchOrMouseChange (InputHandler handler) {
		if (isTouched) {
			InputHandlerPointer pointer = (InputHandlerPointer)handler;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
			Vector3 newPos = worldPosition + offset;
			LayerMask mask = 1 << 13;
			
			Collider2D hit2 =Physics2D.OverlapPoint (new Vector2(newPos.x,newPos.y),mask);
			if(hit2!=null)
			{
				if(hit2.gameObject.name=="TrunkArea")
				{
					gameObject.transform.position = new Vector3(newPos.x, newPos.y, gameObject.transform.position.z);
				}
				else
				{
					isTouched = false;
				}
			}
			else
			{
				isTouched = false;	
			}
			
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
