using UnityEngine;
using System.Collections;
using InputEventNS;

public class ScalePiece : MonoBehaviour {
	public ScalePuzzleManager manager;
	private bool isTouched;
	private GameObject hitTargetOn = null; // the plate you are on
	private Vector3 originalLocalPosition;
	private Quaternion originalLocalRotation;
	private Transform originalParent;
	public AudioClip onOrOffSound;
	
	public Sprite spriteWhenDragging = null;
	private Sprite originalSprite;
	public int weight;

	private InputHandler touchOrMouseListener;

	// Use this for initialization
	void Start () {
		originalLocalPosition = gameObject.transform.localPosition;
		originalLocalRotation = gameObject.transform.rotation;
		originalParent = gameObject.transform.parent;
		originalSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, TouchOrMouseChange, TouchOrMouseEnd, 1.0f) ;
	}
	
	void TouchOrMouseStart (InputHandler handler) {
		// TouchOrMouse event is of (sub-)type InputHandlerPointer
		//		Debug.Log("Touch or mouse start");
		
		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);
		
		// Make sure you are touching this object
		if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
			gameObject.transform.rotation = Quaternion.identity; // Reset the rotation
			if (spriteWhenDragging != null) {
				gameObject.GetComponent<SpriteRenderer>().sprite = spriteWhenDragging;
			}
			
			// If you are already on a plate, move back to your original
			if (hitTargetOn != null) {
				ReturnToOriginalPosition();
			} else {
				isTouched = true;
			}
		}
	}
	
	void TouchOrMouseChange(InputHandler handler) {
		//		Debug.Log("Touch or mouse change");
		if (isTouched) {
			//			Debug.Log("Actually is this object");
			InputHandlerPointer pointer = (InputHandlerPointer)handler;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
			gameObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, gameObject.transform.position.z);
		}
	}
	
	void TouchOrMouseEnd(InputHandler handler) {
		//		Debug.Log("Touch finished");
		if (isTouched) {
			isTouched = false;
			GameObject hitTarget = manager.CheckIfMove(this);
			if (hitTarget == null) {
				Debug.Log("hit target is null");
				ReturnToOriginalPosition();
			} else {
				SnapToTarget(hitTarget);
			}
		}
	}

	// TODO: we need to clarify what the pieces do vs what the manager does.
	private void SnapToTarget(GameObject scale) {
		Helper.PlayAudioIfSoundOn(onOrOffSound);
		gameObject.transform.position = new Vector3(scale.transform.position.x, scale.transform.position.y, originalLocalPosition.z);
		gameObject.transform.parent = scale.transform;
		gameObject.transform.localRotation = Quaternion.identity; // reset to local rotation

		hitTargetOn = scale;
		manager.WeightCompare();
	}
	
	public void ReturnToOriginalPosition() {
		Helper.PlayAudioIfSoundOn(onOrOffSound);
		gameObject.transform.parent = originalParent;
		gameObject.transform.localPosition = originalLocalPosition;
		gameObject.transform.rotation = originalLocalRotation;
		gameObject.GetComponent<SpriteRenderer>().sprite = originalSprite;
		if (hitTargetOn != null) {
			manager.PieceMovedOffScale(this);

			hitTargetOn = null;
		}
	}

	private void DisableScalePiece() {
		InputEvent.RemoveListener(touchOrMouseListener);
		enabled = false;
	}
}