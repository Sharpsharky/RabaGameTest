using UnityEngine;
using System.Collections;

public class EventWhenTapped : MonoBehaviour {
	public delegate void TappedEvent(GameObject objectTapped);
	public event TappedEvent OnTap;
	public AudioClip soundWhenTapped;

	void OnMouseDown() {
		Helper.PlayAudioIfSoundOn(soundWhenTapped);
		if (OnTap != null) {
			OnTap(this.gameObject);
		}
	}
}
