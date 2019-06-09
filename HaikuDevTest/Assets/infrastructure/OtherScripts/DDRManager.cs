using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class DDRManager : MonoBehaviour {
	public GameObject lane0;
	public GameObject lane1;
	public GameObject lane2; // Possibly can be deleted
	public GameObject lane3; // Possibly can be deleted

	public GameObject redAction;
	public GameObject greenAction;
	public GameObject blueAction;
	public GameObject purpleAction;

	public GameObject incorrectXpost;
	public GameObject correctXpost;

	public GameObject correctFX;
	public GameObject incorrectFX;

	private float currentTimeToSpawn;
	private int piecesSpawned;
	private int incorrectPieces;

	private GameObject[] lanes;
	private GameObject[] actions;
	private float[] timeToSpawn;

	public AudioClip correctSound0;
	public AudioClip correctSound1;
	public AudioClip correctSound2;
	public AudioClip correctSound3;

	private List<List<GameObject>> allAction = new List<List<GameObject>>();
//	private LineWave wave;

	// Use this for initialization
	void Awake () {
		timeToSpawn = new float[] {3.5f, 3.5f, 3.0f, 3.0f, 3.0f, 2.5f, 2.5f, 2.5f, 1.5f, 1.0f, 
							       2.5f, 2.5f, 2.0f, 2.0f, 2.2f, 1.0f, 1.5f, 1.5f, 1.5f, 1.0f,
							       2.5f, 2.5f, 2.5f, 2.5f, 2.0f, 1.5f, 1.0f, 1.5f, 1.5f, 8.0f,
								   3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f, 3.0f }; // Last row not used

		currentTimeToSpawn = timeToSpawn[0];
		lanes = new [] {lane0, lane1, lane2, lane3};
		actions = new [] {redAction, greenAction, blueAction, purpleAction};
	}
	
	// Update is called once per frame
	void Update () {
		currentTimeToSpawn -= Time.deltaTime;
		if (currentTimeToSpawn < 0) {
			if (piecesSpawned > 5) {
				int rand = UnityEngine.Random.Range(0, 100);
//				if (rand > 90) {
//					SpawnPieces(4);
//				} else if (rand > 75) {
//					SpawnPieces(3);
//				} else 
				if (rand > 60) {
					SpawnPieces(2);
				} else {
					SpawnPieces(1);
				}
			} else {
				SpawnPieces(1);
			}
		}
	}

	private void SpawnPieces(int numPieces) {
		int[] colorIDs = new int[] {0, 1, 2, 3};
		colorIDs = ShuffleArray(colorIDs);
		List<GameObject> actionColumn = new List<GameObject>();
		for (int i = 0; i < numPieces; i++) {
//			Debug.Log("Spawning piece");
			GameObject lane = lanes[i];
			int colorID = colorIDs[i];
			GameObject action = actions[colorID];

			GameObject clonedAction = (GameObject)Instantiate(action);
			clonedAction.transform.parent = lane.transform;
			clonedAction.transform.localPosition = new Vector3(0, 0, 0);
			actionColumn.Add(clonedAction);

		}
		piecesSpawned++; // Consider 2 pieces to be 1
		Debug.Log("Pieces " + piecesSpawned);
		
		if (piecesSpawned >= 30) {
			PlayMakerFSM fsm = gameObject.GetComponent<PlayMakerFSM>();
			fsm.SendEvent("won");
			Debug.Log("Won");
		} else {

			allAction.Add(actionColumn);
	//		DebugList();
			currentTimeToSpawn = timeToSpawn[piecesSpawned]; 
		}
	}

	public void ColorTouched(string colorID) {
		if (allAction.Count > 0) {
			List<GameObject> actionColumn = allAction[0];

			for(int i = actionColumn.Count - 1; i >= 0; i--) {
				GameObject action = actionColumn[i];
				if (action.tag.Equals(colorID)) {
					// Now check position
					float xPosition = action.transform.position.x;
					if (xPosition > incorrectXpost.transform.position.x &&
					    xPosition < correctXpost.transform.position.x) {
						Debug.Log("Correct press");

						// Play Correct Sound
						if (colorID.Equals("1")) {
							Helper.PlayAudioIfSoundOn(correctSound0, 0.2f);
						} else if (colorID.Equals("2")) {
							Helper.PlayAudioIfSoundOn(correctSound1, 0.2f);
						} else if (colorID.Equals("3")) {
							Helper.PlayAudioIfSoundOn(correctSound2, 0.2f);
						} else {
							Helper.PlayAudioIfSoundOn(correctSound3, 0.2f);
						}

						// Set up the wave
//						wave.SetIdealAmpTo(1.0f);
						Invoke("RestoreLine", 1.0f);
						// Correct hit
						CreateFxAtPiece(correctFX, action);

					} else {
//						wave.SetIdealAmpTo(0.1f);
						Invoke("RestoreLine", 1.0f);

						Debug.Log("Pressed in wrong position");
						IncorrectAction(action);
					}
					// Remove the action
					actionColumn.RemoveAt(i);
					Destroy (action);
				} else {
					Debug.Log("Touched but wrong color");
				}
			}
			UpdateList();
		}
	}

	private void RestoreLine() {
//		wave.SetIdealAmpTo(0.25f);
	}

	private void CreateFxAtPiece(GameObject fx, GameObject piece) {
		//		Debug.Log("Creating fx " + fx.name);
		GameObject fxClone = (GameObject)Instantiate(fx, new Vector3(0f, 0f, 0f), Quaternion.identity);
		//		fxClone.transform.Rotate(-90, 0, 0);
		fxClone.transform.parent = this.transform;
		fxClone.transform.position = piece.transform.position;
	}
	
	public void DestroyPiece(GameObject action) {
		IncorrectAction(action);
		// Manually destroy since you aren't going through color touched

		List<GameObject> actionColumn = allAction[0];
		actionColumn.Remove(action);
		Destroy(action);
		UpdateList();
	}

	public void IncorrectAction(GameObject action) {
		CreateFxAtPiece(incorrectFX, action);
		IncrementIncorrect();
		// Updating list and destroying action happens afterwards in colortouched
	}

	private void UpdateList() {
		for (int i = allAction.Count - 1; i >= 0; i--) {
			List<GameObject> actionColumn = allAction[i];
//			Debug.Log("Name: " + actionColumn[0].name + " action column i " + i + " count " + actionColumn.Count);
//			if (actionColumn[0] == null) {
//				allAction.RemoveAt(i);
//			}
//			else 
			if (actionColumn.Count == 0) {
				allAction.RemoveAt(i);
			}
		}
//		DebugList();
	}

	private void DebugList() {
		string debugString = "";

		for (int i = 0; i < allAction.Count; i++) {
			List<GameObject> actionColumn = allAction[i];
			debugString = debugString + " i " + i;
			for (int j = 0; j < actionColumn.Count; j++) {
				GameObject action = actionColumn[j];
				if (action) {
					debugString = debugString + " action " + action.name;
				}
			}
		}
		Debug.Log("Debug list" + debugString);
	}
	
	public void IncrementIncorrect() {
		incorrectPieces++;
		// Check if loss

		float percentage = incorrectPieces / (float)piecesSpawned;
//		Debug.Log("Percent Wrong: " + percentage);
		if (percentage > 0.3f &&
		    piecesSpawned > 10) {
//			wave.SetIdealAmpTo(0.10f);

			PlayMakerFSM fsm = gameObject.GetComponent<PlayMakerFSM>();
			fsm.SendEvent("loss");
			Debug.Log("Loss");
		}
	}

	private int[] ShuffleArray(int[] a) {
		// Loops through array
		for (int i = a.Length-1; i > 0; i--) {
			// Randomize a number between 0 and i (so that the range decreases each time)
			int rnd = UnityEngine.Random.Range(0,i);
			
			// Save the value of the current i, otherwise it'll overright when we swap the values
			int temp = a[i];
			
			// Swap the new and old values
			a[i] = a[rnd];
			a[rnd] = temp;
		}
		return a;
	}
}
