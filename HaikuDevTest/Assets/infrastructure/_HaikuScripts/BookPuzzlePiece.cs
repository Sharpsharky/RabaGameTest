using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent (typeof (Collider2D))]
public class BookPuzzlePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	public int correctSlotPosition;
	public int slotPosition;
	public BookManager manager;

	public bool isAlwaysCorrect;
	public SpriteRenderer selectionHalo;

   
	// TEMP CHANGE TEMP CHANGE
	
	public bool isCorrectPosition() {
		if (isAlwaysCorrect) {
			return true;
		} else {
			Debug.Log(" name: " + this.gameObject.name + " correct: " + correctSlotPosition + " slot: " + slotPosition);
			return (correctSlotPosition == slotPosition);
		}
	}

	void Update() {
		// Fix it so that the original orientation is kept (even during rotation)
		transform.eulerAngles = new Vector3(0, 0, 0);
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        manager.BookPieceTapped(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // don't delete OnPointerUp - it is needed for OnPointerDown to work
    }
}
