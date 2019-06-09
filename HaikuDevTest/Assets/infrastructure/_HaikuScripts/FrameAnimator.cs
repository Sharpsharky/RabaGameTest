using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrameAnimator : MonoBehaviour {

	public List<GameObject> frames;
	public bool playOnAwake = true;
	public bool wheelFrames;
	public float delayBetweenFrames = 0.5f;
	private int index;
	private bool countUp = true;

	public int loopsBeforeEnd;
	private int numberOfLoops;
	public string eventWhenComplete;
	public PlayMakerFSM sendCompleteEvent;

	// Use this for initialization
	void Start () {
		if (playOnAwake) {
			StartAnimation();
		}
	}

	void StartAnimation() {
		InvokeRepeating("ShowNextFrame", 0, delayBetweenFrames);
	}

	void ChangeDelayBetweenFrames(float newDelay) {
		delayBetweenFrames = newDelay;
		CancelInvoke("ShowNextFrame");
		InvokeRepeating("ShowNextFrame", 0, delayBetweenFrames);
	}

	void ShowNextFrame() {
		foreach (GameObject frame in frames) {
			frame.SetActive(false);
		}
		frames[index].SetActive(true);
		UpdateIndex();
	}

	void UpdateIndex() {
		if (wheelFrames) {
			if (index == (frames.Count - 1)) {
				index--;
				countUp = false;
			} else if (index == 0) {
				index++;
				countUp = true;
				CheckIfAnimationComplete();
			} else {
				if (countUp) {
					index++;
				} else {
					index--;
				}
			}
		} else { // Count up and reset to 0 when you hit the max
			if (index == (frames.Count - 1)) {
				index = 0;
				CheckIfAnimationComplete();
			} else {
				index++;
			}
		}
	}

	void CheckIfAnimationComplete() {
		numberOfLoops++;
		if (numberOfLoops >= loopsBeforeEnd) {
			StopAnimation();
		}
	}

	void StopAnimation() {
		CancelInvoke("ShowNextFrame");
		if (sendCompleteEvent != null) {
			if (!string.IsNullOrEmpty(eventWhenComplete)) {
				sendCompleteEvent.SendEvent(eventWhenComplete);
			} else {
				Debug.Log("Set a complete event string in inspector for animation in: " + gameObject.name);
			}
		} else {
			Debug.Log("No send complete event set in inspector for animation in: " + gameObject.name);
		}
	}
}
