using UnityEngine;
using System.Collections;
using InputEventNS;
using System.Linq;

public class NavigationWheel : MonoBehaviour {
	private Vector3 originalPosition;
	private Vector3 centerPoint; // possibly account for center not being in center
	private float cachedAngle = 0f; // Account for the angle that your initial touch was
	private float originalAngle; // Account for the initial rotation of the object
	private float newAngle; // The absolute rotation of the ring piece

	private bool isTouched;
	private float roundToIncrement = 45;

	private int[] correctNums = new int[]{225, 270, 315, 45};
	private int[] currentNums = new int[4];
	private int currentSlot = 0;

	public SpriteRenderer[] enteredAngles;
	private int maxSlots;

	public AudioClip lockInSlot;

	private InputHandler touchOrMouseListener;

	void Start() {
		originalAngle = transform.eulerAngles.z; // Store your initial rotation
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, TouchOrMouseChange, TouchOrMouseEnd, 1.0f) ;
		maxSlots = enteredAngles.Length;
	}

	void TouchOrMouseStart(InputHandler handler) {
		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		originalPosition = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		// Get the position of the initial touch so you know how much to rotate relative to this point
		cachedAngle = Mathf.Atan2 (-originalPosition.x, originalPosition.y) * Mathf.Rad2Deg;

		// Make sure you are touching this object
		Vector2 touchPos = new Vector2(originalPosition.x, originalPosition.y);
		if (GetComponent<Collider2D>().OverlapPoint(touchPos)) {
			GetComponent<AudioSource>().enabled = false;
			isTouched = true;
		} 
	}

	public void TouchOrMouseChange(InputHandler handler) {
		if (isTouched) {
			GetComponent<AudioSource>().enabled = true;
			InputHandlerPointer pointer = (InputHandlerPointer)handler;
			Vector3 dir = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
			float newTouchPos = Mathf.Atan2(-dir.x,dir.y) * Mathf.Rad2Deg;
			newAngle = newTouchPos + originalAngle - cachedAngle; // Account for (new touch position - original touch position) and the original rotation angle of the ring
//			Debug.Log("name: " + name + " NTP: " + newTouchPos + " origAngle: " + originalAngle + " cached: " + cachedAngle + " newA: " + newAngle);
			transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
		}
	}

	public void TouchOrMouseEnd(InputHandler handler) {
		if (isTouched) {
			GetComponent<AudioSource>().enabled = false;
			newAngle = (float)RoundAngle(newAngle); // Round off
			transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);

			originalAngle = newAngle; // Store this for the next touch
			Debug.Log(" original angle: " + originalAngle);
			int rotation = RoundAngle(gameObject.transform.rotation.eulerAngles.z);
			isTouched = false;
			UpdatePuzzleAngle(rotation);
		}
	}

	private void UpdatePuzzleAngle(int angle) {
		Helper.PlayAudioIfSoundOn(lockInSlot);
		currentNums[currentSlot] = angle;
		bool isCorrect = Enumerable.SequenceEqual(correctNums, currentNums);
		Debug.Log("Slot " + currentSlot + " angle: " + angle + " isCorrect: " + isCorrect);

		SpriteRenderer enteredAngle = enteredAngles[currentSlot];
		enteredAngle.enabled = true;
		enteredAngle.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		currentSlot++;

		if (currentSlot >= maxSlots) {
			gameObject.GetComponent<Collider2D>().enabled = false; // Prevent more touches while we are resetting the puzzle
			if (!isCorrect) {
				iTween.RotateTo (gameObject, iTween.Hash("z", 0, "onComplete", "ResetPuzzle", "onCompleteTarget", gameObject, "islocal", true, "time", 1.0, "delay", 0.5));
				originalAngle = 0; // Reset the angle (which you previously were caching for another touch)
			} else {
				Debug.Log("Won Nav Puzzle!");
				GetComponent<PlayMakerFSM>().SendEvent("won");
			}
		}
	}

	private void ResetPuzzle() {
		iTween.RotateTo (gameObject, iTween.Hash("z", 0, "onComplete", "WeightCompare", "onCompleteTarget", gameObject, "islocal", true, "time", 0.2));

		foreach (SpriteRenderer enteredAngle in enteredAngles) {
			enteredAngle.enabled = false;
		}
		currentSlot = 0;
		gameObject.GetComponent<Collider2D>().enabled = true; // Prevent more touches while we are resetting the puzzle
	}

	private int RoundAngle(float number) {
		int i = Mathf.RoundToInt(number);
		return ((int)Mathf.Round(i / roundToIncrement)) * (int)roundToIncrement;
	}

	void OnDisable() {
		InputEvent.RemoveListener(touchOrMouseListener);
	}
}
