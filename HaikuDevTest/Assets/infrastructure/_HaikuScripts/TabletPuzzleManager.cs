using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class TabletPuzzleManager : MonoBehaviour {

    public AudioClip snapAudioClip;
	public List<TabletQuadrant> quadrants;

    AudioSource audioSource;

	// Use this for initialization
	void Start () {
		this.audioSource = GetComponent<AudioSource>();
	}

	void Update () {}

	public bool checkChipDidSnap(TabletPuzzleChip chip) {
		// Remove the current chip from any quadrant
		foreach (TabletQuadrant q in this.quadrants) {
			q.removeChipIfNeeded(chip);
		}

		// Check if the chip is snapping to any quadrant
		bool didSnap = false;
		foreach (TabletQuadrant q in this.quadrants) {
			Vector3 chipPosition = chip.transform.position;
			Bounds qBounds = q.GetComponent<BoxCollider2D>().bounds;
			float chipOffset = chip.GetComponent<BoxCollider2D>().bounds.size.x / 2;

			if (chipPosition.x >= qBounds.min.x && chipPosition.x <= qBounds.center.x &&
				chipPosition.y >= qBounds.min.y && chipPosition.y <= qBounds.max.y) { // Left
				// If there is a Chip there, bring it back to the board
				if (q.leftChip != null && q.leftChip != chip) {
					q.leftChip.reset();
				}
				q.leftChip = chip;
				// Set new position
				chip.transform.position = new Vector3(qBounds.center.x - chipOffset, qBounds.center.y, chip.transform.position.z);
				chip.turnOff();
				didSnap = true;
			}
			else if (chipPosition.x > qBounds.center.x && chipPosition.x <= qBounds.max.x &&
				chipPosition.y >= qBounds.min.y && chipPosition.y <= qBounds.max.y) { // Right
				// If there is a Chip there, bring it back to the board
				if (q.rightChip != null && q.rightChip != chip) {
					q.rightChip.reset();
				}
				q.rightChip = chip;
				// Set new position
				chip.transform.position = new Vector3(qBounds.center.x + chipOffset, qBounds.center.y, chip.transform.position.z);
				chip.turnOff();
				didSnap = true;				
			}
		}

		if (didSnap) {
			this.audioSource.PlayOneShot(this.snapAudioClip, 1);
		}

		this.checkDidWin();
		return didSnap;
	}

	private bool checkDidWin() {
		foreach (TabletQuadrant q in this.quadrants) {
			// Quadrant is missing some chips
			if (!q.hasCorrectChips()) {
				return false;
			}
		}

		// Make all Chip selected when winning the game
		foreach (TabletQuadrant q in this.quadrants) {
			q.leftChip.turnOn();
			q.rightChip.turnOn();
		}

		gameObject.GetComponent<PlayMakerFSM>().SendEvent("won");
		return true;
	}

}
