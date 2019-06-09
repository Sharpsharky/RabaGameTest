using UnityEngine;
using System.Collections;
using InputEventNS;

public class SelectCorrectColliderManager : MonoBehaviour {
	private Collider2D[] colliders;
	private InputHandler touchOrMouseListener;
	private Collider2D selectedCollider = null;
	public Collider2D correctCollider;
	public GameObject magnifier;

	public AudioClip selectedSound;
	public AudioClip checkColliderSound;

	// Use this for initialization
	void Start () {
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, null, null, 1.0f) ;
		colliders = GetComponentsInChildren<Collider2D>();
	}
	
	void TouchOrMouseStart (InputHandler handler) {

		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);

		// Make sure you are touching this object
		foreach (Collider2D targetCollider in colliders) {
			if (targetCollider == Physics2D.OverlapPoint(touchPos)) {
				Debug.Log("Collider pressed: " + targetCollider.gameObject.name);
				if (selectedCollider == null) {
					selectedCollider = targetCollider;
					Helper.PlayAudioIfSoundOn(selectedSound);
					selectedCollider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				} else if (selectedCollider == targetCollider) {
					Investigate();
				} else {
					Helper.PlayAudioIfSoundOn(selectedSound);
					selectedCollider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
					selectedCollider = targetCollider;
					selectedCollider.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				}
			}
		}
	}

	private void Investigate() {
		Helper.PlayAudioIfSoundOn(checkColliderSound);
		magnifier.GetComponent<SpriteRenderer>().enabled = true;
		magnifier.transform.position = selectedCollider.transform.position;
		selectedCollider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		magnifier.GetComponent<PlayMakerFSM>().SendEvent("activate");
	}

	public void CheckIfWin() {
		if (selectedCollider == correctCollider) {
			GetComponent<PlayMakerFSM>().SendEvent("won");
		} else {
			magnifier.GetComponent<SpriteRenderer>().enabled = false;
			selectedCollider = null;
		}
	}

	void OnDisable() {
		InputEvent.RemoveListener(touchOrMouseListener);
	}
}
