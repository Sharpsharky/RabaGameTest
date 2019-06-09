using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InputEventNS;

public class DragItemToTargets : MonoBehaviour {
	private Vector3 originalLocalPosition;
	private InputHandler touchOrMouseListener;
	private bool isTouched;
	public bool destroyAfterSuccess = true;
	public Collider2D[] targets;
	public string[] eventsToSendTargets;
	public string playmakerEventIfSuccess; // If left empty, no event. Sends if you do not have "destroyAfterSuccess"
	public string localizationKeyIfWrongTarget; // If left empty, it will not trigger the top bar

	public bool moveToDefaultSortingLayerIfDragging = true;
	private string cachedSortingLayer = "";

	private Vector3 offset = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
		cachedSortingLayer = GetComponent<SpriteRenderer>().sortingLayerName;
		originalLocalPosition = gameObject.transform.localPosition;
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, TouchOrMouseChange, TouchOrMouseEnd, 1.0f) ;
	}

	void TouchOrMouseStart (InputHandler handler) {
		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);

		// Make sure you are touching this object
		if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
			gameObject.transform.rotation = Quaternion.identity; // Reset the rotation
			offset = gameObject.transform.position - wp;

			isTouched = true;
			if (moveToDefaultSortingLayerIfDragging) {
				SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
				foreach(SpriteRenderer childRenderer in spriteRenderers) {
					childRenderer.sortingLayerName = "Default";
				}
			}
		}
	}

	void TouchOrMouseChange(InputHandler handler) {
		if (isTouched) {
			InputHandlerPointer pointer = (InputHandlerPointer)handler;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
			gameObject.transform.position = new Vector3(worldPosition.x + offset.x, 
														worldPosition.y + offset.y, 
														gameObject.transform.position.z);
		}
	}

	void TouchOrMouseEnd(InputHandler handler) {
		//		Debug.Log("Touch finished");
		if (isTouched) {
			if (moveToDefaultSortingLayerIfDragging) {
				SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
				foreach(SpriteRenderer childRenderer in spriteRenderers) {
					childRenderer.sortingLayerName = cachedSortingLayer;
				}	
			}

			isTouched = false;
			Vector3 wp = transform.position;
			Vector2 piecePos = new Vector2(transform.position.x, transform.position.y);

			bool didHitTarget = false;
			for (int i = 0; i < targets.Length; i++) {
				Collider2D targetCollider = targets[i];
				if (targetCollider.OverlapPoint(piecePos)) {
					string eventToSend = eventsToSendTargets[i];
					if (!string.IsNullOrEmpty(eventToSend)) {
						targetCollider.gameObject.GetComponent<PlayMakerFSM>().SendEvent(eventToSend);
					}
					if (destroyAfterSuccess) {
						Destroy(gameObject);
					} else {
						gameObject.transform.localPosition = originalLocalPosition;
						if (!string.IsNullOrEmpty(playmakerEventIfSuccess)) {
							GetComponent<PlayMakerFSM>().SendEvent(playmakerEventIfSuccess);
						}
					}
					didHitTarget = true;
				}
			}

			// Return to origianl position if you didn't hit a target
			if (!didHitTarget) {
				Helper.LocalizeKeyToTopBar(localizationKeyIfWrongTarget);
				gameObject.transform.localPosition = originalLocalPosition;
			}
		}
	}

	private void OnDisable() {
		InputEvent.RemoveListener(touchOrMouseListener);
	}

}
