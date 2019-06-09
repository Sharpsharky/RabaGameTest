using UnityEngine;
using System.Collections;

public class ScalePuzzleManager : MonoBehaviour {
	public GameObject leftScale;
	public GameObject rightScale;

	public static string kPieceOnTarget = "2"; // Hacky since we are reusing the global tags we have
	public static string kPieceOffTarget = "1";

	private ScalePiece leftPiece = null;
	private ScalePiece rightPiece = null;

	private void SendWin() {
		PlayMakerFSM fsm = this.GetComponent<PlayMakerFSM>();
		fsm.SendEvent("won");
	}

	public void WeightCompare() {
		Util.Log ("Weight compare");
		int difference = 0;
		if (rightPiece != null &
		    leftPiece != null) {
			// If left is heavier, difference is positive.
			difference = leftPiece.weight - rightPiece.weight;
		} else if (rightPiece != null) {
			difference = -200;
		} else if (leftPiece != null) {
			difference = 200;
		} else if (leftPiece == null &&
		           rightPiece == null) {
			Debug.Log("Trying to weigh two empty scales!");
		}
		float timeToRotate = 6 / Mathf.Max(Mathf.Pow(Mathf.Abs (difference), 0.25f), 1.0f);
		Debug.Log("Time to rotate: " + timeToRotate + " difference: " + difference);
		
		if (difference > 0) {
			iTween.RotateTo (gameObject, iTween.Hash ("z", 20, "time", timeToRotate));
		} else if (difference < 0) {
			iTween.RotateTo (gameObject, iTween.Hash ("z", -20, "time", timeToRotate));
		} else {
			iTween.RotateTo (gameObject, iTween.Hash ("z", 0, "time", timeToRotate));
		}
	}

	public void MoveAllOffScale() {
		if (leftPiece != null) {
			leftPiece.ReturnToOriginalPosition();
		}
		if (rightPiece != null) {
			rightPiece.ReturnToOriginalPosition();
		}
	}

	public void PieceMovedOffScale(ScalePiece piece) {
		if (piece == null) return;
		Debug.Log("Piece moved off scale");
		if (piece.Equals(leftPiece)) {
			Debug.Log("Clearing left piece");
			leftPiece = null;
		} else if (piece.Equals(rightPiece)) {
			Debug.Log("Clearing right piece");
			rightPiece = null;
		}
		WeightCompare();
	}
	
	public GameObject CheckIfMove(ScalePiece piece) {
		GameObject scale = null;
		scale = CheckScale(leftScale, piece, ref leftPiece);
		if (scale == null) {
			scale = CheckScale(rightScale, piece, ref rightPiece);
		}
		return scale;
	}

	private GameObject CheckScale(GameObject scale, ScalePiece piece, ref ScalePiece pieceOnScale) {
		Debug.Log("Checking target " + scale.name);
		Vector3 wp = piece.transform.position;
		Vector2 piecePos = new Vector2(wp.x, wp.y);
		
		if (scale.GetComponent<Collider2D>().OverlapPoint(piecePos)) {
			Debug.Log("Trying to move piece to piece " + scale.name);
			if (!scale.tag.Equals(kPieceOnTarget)) {
				pieceOnScale = piece;
				// If it's empty, then do stuff
				return scale;
			} else {
				// If there's something on the piece, snap it back
				return null;
			}
		}
		return null;
	}
}