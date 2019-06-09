using UnityEngine;
using System.Collections;

public class MusicPiece : MonoBehaviour {
	public int noteID;
	private MusicManager manager;
	private bool isPressedDown = false;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
	}

	void OnMouseDown() {
		if (isPressedDown) return;
		AudioSource audioSource = this.GetComponent<AudioSource>();
		audioSource.Play();
		manager.NotePressed(this);
		GetComponent<Renderer>().enabled = false;
		isPressedDown = true;
		Invoke ("RestoreRenderer", 0.5f);
	}

	private void RestoreRenderer() {
		isPressedDown = false;
		GetComponent<Renderer>().enabled = true;
	}
}
