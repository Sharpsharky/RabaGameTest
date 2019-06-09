using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DDRPieceSpawner : MonoBehaviour {
	public GameObject spawnedAction;
	public GameObject correctXpost;
	public GameObject incorrectXpost;
	public DDRManager manager;
	public GameObject correctFx;
	public GameObject incorrectFx;
	private List<GameObject> allAction = new List<GameObject>();

	public void SpawnPiece() {
		GameObject clonedAction = (GameObject)Instantiate(spawnedAction);
		clonedAction.transform.parent = this.transform;
		clonedAction.transform.localPosition = new Vector3(0, 0, 0);
		allAction.Add(clonedAction);
	}

	public void ColorTouched() {
		if (allAction.Count > 0) {
			GameObject firstPiece = (GameObject)allAction[0];
			float xPosition = firstPiece.transform.position.x;
			if (xPosition > incorrectXpost.transform.position.x &&
			    xPosition < correctXpost.transform.position.x) {

				// Correct hit
				CreateFxAtPiece(correctFx, firstPiece);
				allAction.Remove(firstPiece);
				Destroy(firstPiece);
				Debug.Log("Correct press");
			} else {
				Debug.Log("Failed press");
				IncorrectPiece(firstPiece);
			}
		}
	}

	private void CreateFxAtPiece(GameObject fx, GameObject piece) {
//		Debug.Log("Creating fx " + fx.name);
		GameObject fxClone = (GameObject)Instantiate(fx, new Vector3(0f, 0f, 0f), Quaternion.identity);
//		fxClone.transform.Rotate(-90, 0, 0);
		fxClone.transform.parent = this.transform;
		fxClone.transform.position = piece.transform.position;
	}

	public void PieceHitDestroyer(GameObject piece) {
		Debug.Log("Destroying piece " + piece.name);
		IncorrectPiece(piece);
	}

	private void IncorrectPiece(GameObject piece) {
		CreateFxAtPiece(incorrectFx, piece);
		allAction.Remove(piece);
		manager.IncrementIncorrect();
		Destroy (piece);
	}
}
