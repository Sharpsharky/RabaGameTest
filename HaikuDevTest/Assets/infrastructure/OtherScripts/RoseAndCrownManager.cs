using UnityEngine;
using System.Collections;

public class RoseAndCrownManager : MonoBehaviour {
	private ArrayList slots = new ArrayList();
	private RoseCrownSlot selectedSlot = null;
	private Vector3 scaledUpScale = new Vector3(1.5f, 1.5f, 1.5f);
	private Vector3 scaledDownScale = new Vector3(.6667f, .6667f, .6667f);
	public AudioClip moveSound;

	// Use this for initialization
	void Start () {
		slots.Add (this.transform.Find ("0_slot").GetComponent<RoseCrownSlot> ());
		slots.Add (this.transform.Find ("1_slot").GetComponent<RoseCrownSlot> ());
		slots.Add (this.transform.Find ("2_slot").GetComponent<RoseCrownSlot> ());
		slots.Add (this.transform.Find ("3_slot").GetComponent<RoseCrownSlot> ());
		slots.Add (this.transform.Find ("4_slot").GetComponent<RoseCrownSlot> ());
		slots.Add (this.transform.Find ("5_slot").GetComponent<RoseCrownSlot> ());
		slots.Add (this.transform.Find ("6_slot").GetComponent<RoseCrownSlot> ());

	}
	
	public void SlotTapped(RoseCrownSlot roseCrown) {
		Debug.Log ("Int " + roseCrown.slot);
		int slot = roseCrown.slot;
		GameObject slotTappedPiece = roseCrown.piece;
		if ((selectedSlot == null) && (slotTappedPiece != null)) {
			// Selec the slot if there's a piece and no slot has been selected
			selectedSlot = (RoseCrownSlot)slots [slot];
			Debug.Log ("Selected Slot = " + slot);
			slotTappedPiece.transform.localScale = Vector3.Scale(slotTappedPiece.transform.localScale, scaledUpScale);
			// TODO: Do something to visually represent selection
		} else if (selectedSlot != null) {
			// Select or Unselect
			if (slot == selectedSlot.slot) {
				DeselectSlot();
			} else {
				// Try to move
				string pieceTagStr = selectedSlot.piece.tag;
				int nearSlotToCheck = selectedSlot.slot;
				int farSlotToCheck = selectedSlot.slot;
				if (pieceTagStr == "1") {
					nearSlotToCheck--;
					farSlotToCheck = farSlotToCheck - 2;
				} else {
					nearSlotToCheck++;
					farSlotToCheck = farSlotToCheck + 2;
				}
//				Debug.Log("Near Slot " + nearSlotToCheck + " Far Slot " + farSlotToCheck);
				if (slot == nearSlotToCheck) {
					TryMoveToNearSlot(nearSlotToCheck);
				} else {
					TryMoveToFarSlot(nearSlotToCheck, farSlotToCheck, pieceTagStr);
				}
			}
		}
	}

	private void TryMoveToNearSlot(int slot) {
		RoseCrownSlot nearSlot = (RoseCrownSlot)slots [slot];
		if (nearSlot.piece == null) {
			MoveSelectedPieceToSlot(nearSlot);
		}
		Debug.Log ("Trying to nearmove to " + nearSlot);
	}

	private void CheckIfWin() {
		int correctCount = 0;
		for (int i = 0; i <= 2; i++) {
			RoseCrownSlot slot = (RoseCrownSlot)slots [i];
			if (slot.piece != null) {
				if (slot.piece.tag == "1")
					correctCount ++;
			}
		}
		for (int i = 4; i <= 6; i++) {
			RoseCrownSlot slot = (RoseCrownSlot)slots [i];
			if (slot.piece != null) {
				if (slot.piece.tag == "2")
				correctCount ++;
			}
		}
		if (correctCount >= 6) {
			PlayMakerFSM fsm = this.GetComponent<PlayMakerFSM> ();
			fsm.SendEvent("won");
		}
	}

	private void TryMoveToFarSlot(int near, int far, string pieceTag) {
		RoseCrownSlot farSlot = (RoseCrownSlot)slots [far];
		RoseCrownSlot nearSlot = (RoseCrownSlot)slots [near];
		if (farSlot.piece == null && (nearSlot.piece != null)) {
			// You can only jump over different colored squares
			if (nearSlot.piece.tag != pieceTag) {
				MoveSelectedPieceToSlot(farSlot);
			}
		}
		Debug.Log ("Trying to farmove to " + farSlot);
	}

	private void MoveSelectedPieceToSlot(RoseCrownSlot slot) {
		slot.piece = selectedSlot.piece;
		slot.piece.transform.position = slot.transform.position;
		slot.piece.transform.localScale = Vector3.Scale (slot.piece.transform.localScale, scaledDownScale);
		selectedSlot.piece = null;
		selectedSlot = null;
		AudioSource.PlayClipAtPoint(moveSound, gameObject.transform.localPosition);
		CheckIfWin();
	}

	private void DeselectSlot() {
		selectedSlot.piece.transform.localScale = Vector3.Scale (selectedSlot.piece.transform.localScale, scaledDownScale);
		Debug.Log ("Deselecting");
		selectedSlot = null;
		// Disable selection
	}

	// Update is called once per frame
	void Update () {
	
	}
}
