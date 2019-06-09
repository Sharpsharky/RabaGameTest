using UnityEngine;
using System.Collections;
using InputEventNS;
using System.Linq;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using System;

public class TapInCorrectOrder : MonoBehaviour {
	public Collider2D[] correctPieceOrder;
	private Collider2D[] allPieces;
	public SpriteRenderer[] disableWhenCorrectPiece; // Slowly disable these renderers while the player hits the correct pieces (for example, lasers). Must have the same as triesBeforeReset
	public bool isEnablePieceWhenTapped;

	public int triesBeforeReset;
	private int index;
	private Collider2D[] piecesTapped;
	private InputHandler touchOrMouseListener;
	public AudioClip errorSound;
	public AudioClip selectedSound;

	public bool resetOnError; // Resets the puzzle immediately if the player makes an error.  Otherwise, the puzzle resets after triesBeforeReset

	public GameObject stretchedSprite;
	private Vector3 lastTappedPosition;
	private List<GameObject> stretchedSprites = new List<GameObject>();

	public int requiredSelectedItem; // Leave as 0 if nothing is required
	public string localizedKeyForRequiredItemError;

	public GameObject moveToTapLocation;
	private Vector3 originalMoveToTapPosition;
	private bool isAnimating = false;
	private Collider2D lastPieceTapped;

    public static Action Win;
    public static Action resetPuzzle;
    public bool isSteppingStoneRoom;

    public PlayMakerFSM managerFSM;
	// Use this for initialization
	void Start () {
		allPieces = GetComponentsInChildren<Collider2D>();
		Debug.Log("All pieces: " + allPieces.Length);
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, null, null, 1.0f);
		piecesTapped = new Collider2D[triesBeforeReset];
		if (moveToTapLocation != null) {
			originalMoveToTapPosition = moveToTapLocation.transform.localPosition;
		}
		if (triesBeforeReset < correctPieceOrder.Length) {
			Debug.Log("We reset the puzzle before the player can enter the correct length!");
		}
	}

	void TouchOrMouseStart(InputHandler handler) {
		if (isAnimating) return;
		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);
		
		foreach (Collider2D pieceCollider in allPieces) {
			if (pieceCollider == Physics2D.OverlapPoint(touchPos)) {
				// You can select the piece if its renderer is the opposite of isEnablePieceWhenTapped.
				if (isSteppingStoneRoom || pieceCollider.GetComponent<Renderer>().enabled == !isEnablePieceWhenTapped) { 
					lastPieceTapped = pieceCollider; // Cache because HandlePieceTapped can trigger later

					// If you need to have an item to do tap in correct order, check it now.
					if (!HasCorrectItem()) return;
					
					if (selectedSound) {
						Helper.PlayAudioIfSoundOn(selectedSound);
					}

					if (!MoveToTapLocation(lastPieceTapped.transform.position)) {
						HandlePieceTapped();
						// Otherwise handle piece tapped at the end of MoveToTapLocation
					}

				}
			}
		}
	}

	private void HandlePieceTapped() {
        // Enable or disable the collider based on isEnablePieceWhenTapped
        if (index < piecesTapped.Length)
        {
            Debug.Log("Piece tapped: " + lastPieceTapped.name);
            piecesTapped[index] = lastPieceTapped;
            foreach (Renderer rend in lastPieceTapped.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = isEnablePieceWhenTapped;
            }

            // Check if you pressed the right piece, if you did not start a HandleLoss coRoutine
            if (resetOnError)
            {
                // Stretch a sprite between the two points
                if (stretchedSprite != null && !lastTappedPosition.Equals(Vector3.zero))
                {
                    Strech(lastPieceTapped.transform.position, true);
                }

                if (lastPieceTapped == correctPieceOrder[index])
                {
                    // Correct piece is pressed. Disable renderers if necessary.
                    if (disableWhenCorrectPiece.Length > index)
                    {
                        SpriteRenderer[] renderers = disableWhenCorrectPiece[index].GetComponentsInChildren<SpriteRenderer>();
                        foreach (SpriteRenderer rend in renderers)
                        {
                            rend.enabled = false;
                        }
                    }
                }
                else
                {
                    // Debug.Log("Wrong piece");
                    StartCoroutine(HandleLoss());
                }
            }

            index++;
            lastTappedPosition = lastPieceTapped.transform.position;
            Debug.Log("Tap in correct order index is: " + index);
            if (index >= triesBeforeReset)
            {
                CheckIfWin();
            }
        }
	}
	private bool MoveToTapLocation(Vector3 touchPos) {
		Debug.Log("Moving to tap : " + touchPos.x + " Y: " + touchPos.y);
		if (moveToTapLocation != null) {
			isAnimating = true;
			if (!moveToTapLocation.GetComponent<Renderer>().enabled) {
				moveToTapLocation.GetComponent<Renderer>().enabled = true;
			}
			iTween.MoveTo(moveToTapLocation, iTween.Hash("position", new Vector3(touchPos.x, touchPos.y, moveToTapLocation.transform.position.z), 
			                                             "isLocal", true,
			                                             "time", 0.5f, 
			                                             "oncomplete", "RestoreOriginalPosition", 
			                                             "oncompletetarget", gameObject));
			return true;
		} else {
			return false;
		}
	}

	private void RestoreOriginalPosition() {
		Debug.Log("Restoring original position");
		iTween.MoveTo(moveToTapLocation, iTween.Hash("position", originalMoveToTapPosition, "isLocal", true, "time", 1.0f));
		HandlePieceTapped();
		isAnimating = false;
//		iTween.MoveTo(moveToTapLocation, iTween.Hash("position", originalMoveToTapPosition, "isLocal", true, "time", 1.0f, "oncomplete", "AnimationComplete", "oncompletetarget", gameObject));
	}

	private bool HasCorrectItem() {
		if (requiredSelectedItem != 0) {
			int selectedItemID = FsmVariables.GlobalVariables.GetFsmInt("selectedItem").Value;
			if (selectedItemID != requiredSelectedItem) {
				Helper.LocalizeKeyToTopBar(localizedKeyForRequiredItemError);
				return false;
			}
		}
		return true;
	}

	private void Strech(Vector3 finalPosition, bool mirrorZ) {
		GameObject newSprite = (GameObject)Instantiate(stretchedSprite);
		newSprite.transform.parent = this.transform;
		Vector3 centerPos = (lastTappedPosition + finalPosition) / 2f;
		newSprite.transform.position = centerPos;
		Vector3 direction = finalPosition - lastTappedPosition;
		direction = Vector3.Normalize(direction); // Normalize gives us a 1 unit vector in the same direction
		newSprite.transform.right = direction;
		if (mirrorZ) newSprite.transform.right *= -1f;
		Vector3 scale = new Vector3(1,1,1);
		scale.x = Vector3.Distance(lastTappedPosition, finalPosition) / 4; // Unclear why we have to divide by 2 here
		newSprite.transform.localScale = scale;
		stretchedSprites.Add(newSprite);
	}

	void CheckIfWin() {
		bool didWin = correctPieceOrder.SequenceEqual(piecesTapped);
		if (didWin) {
            if(managerFSM!= null)
                managerFSM.SendEvent("won");
            if (isSteppingStoneRoom)
            {
                if (Win != null)
                    Win();
            }
		} else {
			StartCoroutine(HandleLoss());
		}
	}

   
	IEnumerator HandleLoss() {
        if (!isSteppingStoneRoom)
            yield return new WaitForSeconds(1.0f);
        else
        {
            if(resetPuzzle!=null)
                resetPuzzle();
            yield return new WaitForSeconds(0.2f);
        }
        index = 0;
		lastTappedPosition = Vector3.zero;

		foreach (GameObject stretchedSprite in stretchedSprites) {
			Destroy(stretchedSprite);
		}
		stretchedSprites = new List<GameObject>();

		foreach (Collider2D pieceCollider in allPieces) {
			foreach (Renderer rend in pieceCollider.GetComponentsInChildren<Renderer>()) {
				rend.enabled = !isEnablePieceWhenTapped;
			}
			if (errorSound) {
				Helper.PlayAudioIfSoundOn(errorSound, 0.5f);
			}
		}
		RestoreDisabledSprites();
//		Debug.Log("lost!");
		piecesTapped = new Collider2D[triesBeforeReset];
	}

	void RestoreDisabledSprites() {
		for (int i = 0; i < disableWhenCorrectPiece.Length; i ++) {
			SpriteRenderer[] renderers = disableWhenCorrectPiece[i].GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer rend in renderers) {
				rend.enabled = true;
			}
		}
	}

	void OnDisable() {
		InputEvent.RemoveListener(touchOrMouseListener);
	}
}
