using UnityEngine;
using System.Collections;
using System.Linq;

public class BookManager : PuzzleController {

	private BookPuzzlePiece[] allPieces;
    private Vector3[] allPiecesLocalPositions;
    private int[] allPiecesStartingSlotPositions;
	private BookPuzzlePiece selectedPiece = null;
//	private Vector3 originalPieceScale;
//	private Vector3 scaledUpPieceScale;
	public GameObject selectedHalo;
    public AudioClip tapSound;
	private GameObject selectedHaloClone;
	public bool rotateSwap = false;
	
	private BookPuzzlePiece swappingPiece1;
	private BookPuzzlePiece swappingPiece2;
	private Transform cachedParent;

	private bool isSwapping;

	public bool changeSpriteAndZOrder;
	public int setupWithRandomSwaps;

    public bool isFilingPuzzle = false;
	// Use this for initialization
	void Start () {
        
		allPieces = GetComponentsInChildren<BookPuzzlePiece>();
        allPiecesLocalPositions = new Vector3[allPieces.Length];
        for (int i = 0; i < allPieces.Length; i++)
        {
            allPiecesLocalPositions[i] = allPieces[i].transform.localPosition;
        }
        allPiecesStartingSlotPositions = new int[allPieces.Length];
        for (int i = 0; i < allPieces.Length; i++)
        {
            allPiecesStartingSlotPositions[i] = allPieces[i].slotPosition;
        }

        selectedHaloClone = (GameObject)Instantiate(selectedHalo, new Vector3(0f, 0f, 0f), Quaternion.identity);
            selectedHaloClone.transform.parent = this.transform;
            selectedHaloClone.GetComponent<SpriteRenderer>().enabled = false;
            selectedHaloClone.transform.localScale = allPieces[0].transform.localScale;
        if (!isFilingPuzzle)
        {
            selectedHaloClone.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.65F);
        }
		if (setupWithRandomSwaps > 0) {
			SetupWithRandomSwaps(setupWithRandomSwaps);
		}
	}

	private void SetupWithRandomSwaps(int swaps) {
//		Helper.MuteSound(true);
		for (int i = 0; i < swaps; i++) {
			int randomIndex = Random.Range(0, allPieces.Length);
			int randomIndex2 = Random.Range(0, allPieces.Length);
			BookPuzzlePiece piece1 = allPieces[randomIndex];
			BookPuzzlePiece piece2 = allPieces[randomIndex2];
			Swap(piece1, piece2);
		}
//		Helper.MuteSound(false);
	}
	
	private void CheckIfWin() {
		int totalPieces = allPieces.Count();
		int correctPieces = 0;
		foreach (BookPuzzlePiece piece in allPieces) {
			if (piece.isCorrectPosition()) {
				correctPieces++;
			}
		}
		
		if (correctPieces == totalPieces) {
			Win();
		} else {
			Debug.Log(" Correct : " + correctPieces + " total: " + totalPieces);
		}
	}

	protected override void Win()
    {
		SolvePuzzle();
		selectedHalo.GetComponent<Renderer>().enabled = false;
		foreach (BookPuzzlePiece piece in allPieces)
		{
			piece.gameObject.GetComponent<Collider2D>().enabled = false;
		}

        base.Win();
	}

    public override void Skip()
    {
        Win();
    }

    public override void ResetPuzzle()
    {
        for (int i = 0; i < allPieces.Length; i++)
        {
            allPieces[i].slotPosition = allPiecesStartingSlotPositions[i];
        }
        for (int i = 0; i < allPieces.Length; i++)
        {
            allPieces[i].transform.localPosition = allPiecesLocalPositions[i];
        }
        base.ResetPuzzle();
    }

    public void SolvePuzzle() {
		rotateSwap = false; // Do not animate when we are solving puzzle
		// We do this with two loops..  Is there a more efficient way? Probably.
		allPieces = null;
		// The disabled pieces didn't come as children earlier. Fetch them again after all the pieces are enabled.
		allPieces = GetComponentsInChildren<BookPuzzlePiece>();

		for (int i = 0; i < allPieces.Length; i++) {
			BookPuzzlePiece piece1 = allPieces[i];
			if (!piece1.isCorrectPosition()) {
				for (int j = 0; j < allPieces.Length; j++) {
					BookPuzzlePiece piece2 = allPieces[j];
					if (piece2.slotPosition == piece1.correctSlotPosition) {
						Swap(piece1, piece2);
					}
				}
			}
		}
	}
	
	private void Swap(BookPuzzlePiece piece1, BookPuzzlePiece piece2) {
		Debug.Log("Swapping: " + piece1.name + " and: " + piece2.name);


		// Determine if you will use the swap animation
		if (!rotateSwap) {
			// Switch positions immediately since we are not using the swap animation
			Vector3 cachedPosition = piece1.transform.position;
			int cachedSlotPosition = piece1.slotPosition;
			piece1.transform.position = piece2.transform.position;
			piece1.slotPosition = piece2.slotPosition;
			piece2.transform.position = cachedPosition;
			piece2.slotPosition = cachedSlotPosition;
		} else {

			/*For the Carnival Gem Box puzzle, check if both the pieces are on a same circle collider.*/
			Collider2D[] overlappingPiece1 = Physics2D.OverlapCircleAll (piece1.transform.position, 0.35f);
			Collider2D[] overlappingPiece2 = Physics2D.OverlapCircleAll (piece2.transform.position, 0.35f);

			Collider2D commonCollider = null;
			for(int i=0;i<overlappingPiece1.Length;i++)
			{
				Collider2D piece1Overlap = overlappingPiece1 [i];
				for (int j = 0; j < overlappingPiece2.Length; j++) {
					if(overlappingPiece2[j].Equals(piece1Overlap))
					{
						commonCollider = piece1Overlap;
						break;
					}
					if (commonCollider != null)
						break;
				}
			}
			if (commonCollider != null) {
				isSwapping = true;
				swappingPiece1 = piece1;
				swappingPiece2 = piece2;
				cachedParent = piece1.transform.parent;

				// Create a new GameObject between the two pieces and set it as the swapping pieces' parent.
				GameObject parentGo = new GameObject ();
				Vector3 midPoint = Vector3.Lerp (piece1.transform.position, piece2.transform.position, 0.5f);
				parentGo.transform.position = midPoint;

				piece1.transform.parent = parentGo.transform;
				piece2.transform.parent = parentGo.transform;
				// Cache variables for the callback

				// Rotate around the midpoint parent (swapping the two locations). FontanaPiece fixes the individual pieces rotation so they should still point up.
				iTween.RotateTo (parentGo, iTween.Hash ("z", 180, "time", 0.5, "onComplete", "PiecesRotated", "onCompleteTarget", gameObject));

                // Swap the slot positions
                int cachedSlot = piece1.slotPosition;
                piece1.slotPosition = piece2.slotPosition;
                piece2.slotPosition = cachedSlot;

            }
			else {
				DeselectPiece (piece1);
				DeselectPiece (piece2);
			}

//			Helper.PlayAudioIfSoundOn(swapSound);
		}

		if (changeSpriteAndZOrder) {
			piece1.GetComponent<SpriteRenderer>().sortingOrder = piece1.slotPosition;
			piece1.transform.position = new Vector3(piece1.transform.position.x, piece1.transform.position.y, -piece1.slotPosition / 2.0f);
			piece2.GetComponent<SpriteRenderer>().sortingOrder = piece2.slotPosition;
			piece2.transform.position = new Vector3(piece2.transform.position.x, piece2.transform.position.y, -piece2.slotPosition / 2.0f );
		}
	}
		
	private void PiecesRotated() {
		Transform oldParent = swappingPiece1.transform.parent;
		swappingPiece1.transform.parent = cachedParent;
		swappingPiece2.transform.parent = cachedParent;
		Destroy (oldParent.gameObject);
		CheckIfWin();
		isSwapping = false;
	}
	
	public void BookPieceTapped(BookPuzzlePiece piece) {
		if (isSwapping) return;
		Helper.PlayAudioIfSoundOn(tapSound);
		if (selectedPiece) {
			if (!selectedPiece.Equals(piece)) {
				Swap(selectedPiece, piece);
				DeselectSelectedPiece();
				// Check the win in PiecesRotated if you are using the swap animation
				if (!rotateSwap) {
					CheckIfWin();
				}
			} else {
				// deselect piece
				DeselectSelectedPiece();
			}
		} else {
			// select this piece
			SelectPiece(piece);
		}
	}
	
	private void SelectPiece(BookPuzzlePiece piece) {
		selectedPiece = piece;
//		piece.gameObject.transform.localScale = scaledUpPieceScale;
//		selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = 2;
		if (selectedPiece.selectionHalo == null) {
			selectedHaloClone.GetComponent<SpriteRenderer>().enabled = true;
			selectedHaloClone.transform.position = piece.gameObject.transform.position;
            if(isFilingPuzzle)
                 selectedHaloClone.GetComponent<SpriteRenderer>().sortingOrder = piece.GetComponent<SpriteRenderer>().sortingOrder;
        } else {
			selectedPiece.selectionHalo.sortingOrder = piece.GetComponent<SpriteRenderer>().sortingOrder;
			selectedPiece.selectionHalo.enabled = true;
		}
	}
	
	private void DeselectSelectedPiece() {
//		selectedPiece.gameObject.transform.localScale = originalPieceScale;
//		selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = 0;
		if (selectedPiece != null) {
			if (selectedPiece.selectionHalo != null) {
				selectedPiece.selectionHalo.enabled = false;
			}
		}

		selectedHaloClone.GetComponent<SpriteRenderer>().enabled = false;
		selectedPiece = null;
	}

	private void DeselectPiece(BookPuzzlePiece piece)
	{
		if (piece != null) {
			if (piece.selectionHalo != null) {
				piece.selectionHalo.enabled = false;
			}
		}

		selectedHaloClone.GetComponent<SpriteRenderer>().enabled = false;
	}
}
