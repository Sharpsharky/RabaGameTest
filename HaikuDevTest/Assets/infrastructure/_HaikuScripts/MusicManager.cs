using UnityEngine;
using System.Collections;
using System.Linq;

public class MusicManager : MonoBehaviour {
	private int[] correctSequence;
	private int currentNote;
	public GameObject musicSlotsParent;
	private bool hasWon = false;

	// Use this for initialization
	void Start () {
		// Hard code correct sequence.
		correctSequence = new int[] {1, 3, 5, 5, 3, 1, 0, 2, 4, 4, 2, 0, 6, 4, 2, 6, 4, 2, 6, 1, 3, 5, 0, 2, 4, 4};
	}

	public void NotePressed(MusicPiece piece) {
		if (hasWon) return;
		int correctNote = correctSequence[currentNote];
		if (piece.noteID == correctNote) {
			Debug.Log("Correct note");
			CorrectNoteAtMusicSlot();
			currentNote++;
			if (currentNote == (correctSequence.Length )) {
				PlayMakerFSM fsm = this.GetComponent<PlayMakerFSM>();
				fsm.SendEvent("won");
				hasWon = true;
				Debug.Log("Win");
			}
		} else {
			Debug.Log("Reset back to 0");
			ResetAllMusicSlots();
			currentNote = 0;
		}
	}

	private void CorrectNoteAtMusicSlot() {
		string musicSlotName = "MusicSlot (" + currentNote + ")"; // pretty hacky
		GameObject musicSlot = GameObject.Find(musicSlotName);
		musicSlot.GetComponent<SpriteRenderer>().enabled = true;
	}

	private void ResetAllMusicSlots() {
		SpriteRenderer[] renderers = musicSlotsParent.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in renderers) {
			spriteRenderer.enabled = false;
		}
	}
}
