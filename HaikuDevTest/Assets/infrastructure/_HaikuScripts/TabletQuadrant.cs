using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum QuadrantType {
	topLeft, topRight, bottomLeft, bottomRight
};

public class TabletQuadrant : MonoBehaviour {

	public QuadrantType type;
	public int correctLeftChipNumber, correctRightChipNumber;
	public TabletChipOrientation correctLeftChipOrientation, correctRightChipOrientation;

	public TabletPuzzleChip leftChip = null, rightChip = null;

	// Use this for initialization
	void Start () {}
	void Update () {}

	// Public methods

	// If this chip is on this quadrant, remove it
	public void removeChipIfNeeded(TabletPuzzleChip chip) {
		if (this.leftChip == chip) {
			this.leftChip = null;
		} else if (this.rightChip == chip) {
			this.rightChip = null;
		}
	}

	// Check the quadrant has the correct chips in the correct position
	public bool hasCorrectChips() {
		// One of the two chips is missing
		if (this.leftChip == null || this.rightChip == null) { return false; }

		// Check orientation and piece type match
		if (this.leftChip.chipNumber != this.correctLeftChipNumber || 
			this.leftChip.chipOrientation != this.correctLeftChipOrientation ||
			this.rightChip.chipNumber != this.correctRightChipNumber || 
			this.rightChip.chipOrientation != this.correctRightChipOrientation) {
				return false;
		}

		return true;
	}
}
