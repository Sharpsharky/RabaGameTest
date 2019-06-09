using UnityEngine;
using System.Collections;
using InputEventNS;
using System.Linq;
using System.Collections.Generic;

public class SafeWheelManager : MonoBehaviour {
	private Vector3 originalPosition;
	private Vector3 centerPoint; // possibly account for center not being in center
	private float cachedAngle = 0f; // Account for the angle that your initial touch was
	private float originalAngle; // Account for the initial rotation of the object
	private float newAngle; // The absolute rotation of the ring piece
	
	private bool isTouched;
	private float roundToIncrement = 30;
	
	private List<int> correctNums = new List<int>(5);
	private List<int> currentNums = new List<int>(5);
	private int maxCount = 5;
	private int lastAngle = 0;
	
	public AudioClip lockInSlot;
	
	private InputHandler touchOrMouseListener;
	
	void Start() {
		correctNums.Add(60);
		correctNums.Add(-30);
		correctNums.Add(30);
		correctNums.Add(90);
		correctNums.Add(-60);
		originalAngle = transform.eulerAngles.z; // Store your initial rotation
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, TouchOrMouseChange, TouchOrMouseEnd, 1.0f) ;
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
			isTouched = false;

			newAngle = (float)RoundAngle(newAngle); // Round off
			transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
			
			originalAngle = newAngle; // Store this for the next touch
			Debug.Log(" original angle: " + originalAngle);

			int relativeAngle = (int)originalAngle - lastAngle;
			lastAngle = (int)originalAngle;

			UpdatePuzzleAngle(relativeAngle);
		}
	}
	
	private void UpdatePuzzleAngle(int relativeAngle) {
		Helper.PlayAudioIfSoundOn(lockInSlot);
		Debug.Log("Current nums capacity: " + currentNums.Capacity + " count: " + currentNums.Count);
		if (currentNums.Count + 1 > maxCount) {
			currentNums.RemoveAt(0);
		}
		currentNums.Add (relativeAngle);
		bool isCorrect = Enumerable.SequenceEqual(correctNums, currentNums);

		var result = string.Join(";", currentNums.Select(x => x.ToString()).ToArray()); // (.NET 3.5)
		Debug.Log("RelativeAngle: " + relativeAngle + " isCorrect: " + isCorrect + " allAngles " + result);

		if (isCorrect) {
			gameObject.GetComponent<Collider2D>().enabled = false; // Prevent more touches while we are resetting the puzzle
			Debug.Log("Won Nav Puzzle!");
			GetComponent<PlayMakerFSM>().SendEvent("won");
		}
	}
	
	private int RoundAngle(float number) {
		int i = Mathf.RoundToInt(number);
		return ((int)Mathf.Round(i / roundToIncrement)) * (int)roundToIncrement;
	}
	
	void OnDisable() {
		InputEvent.RemoveListener(touchOrMouseListener);
	}
}