using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class setFsmNeighbors : MonoBehaviour {
	public AudioClip clip;
	// Use this for initialization
	void Start () {
		string name = gameObject.name;

		PlayMakerFSM fsm = GetComponent<PlayMakerFSM>();
		char columnChar = name[0];
		int col = (int)char.GetNumericValue (columnChar);

		char rowChar = name [1];
		int row = (int)char.GetNumericValue (rowChar);

		if (row < 6) {
			int northRow = row + 1;
			string northString = string.Concat (col.ToString (), northRow.ToString ());
			FsmGameObject north = fsm.FsmVariables.GetFsmGameObject ("northObject");
			north.Value = GameObject.Find (northString);
		}
		if (row > 1) {
			int southRow = row - 1;
			string southString = string.Concat (col.ToString (), southRow.ToString ());
			FsmGameObject south = fsm.FsmVariables.GetFsmGameObject ("southObject");
			south.Value = GameObject.Find (southString);
				}
		if (col < 6) {
			int eastCol = col + 1;
			string eastString = string.Concat (eastCol.ToString (), row.ToString ());
			FsmGameObject east = fsm.FsmVariables.GetFsmGameObject ("eastObject");
			east.Value = GameObject.Find (eastString);
				}

		if (col > 1) {
			int westCol = col - 1;
			string westString = string.Concat (westCol.ToString (), row.ToString ());
			FsmGameObject west = fsm.FsmVariables.GetFsmGameObject ("westObject");
//			Debug.Log("westString " + westString);
//			Debug.Log("This is the west name" + west.Name);
			west.Value = GameObject.Find (westString);
		}
	}

	void TurnOnSprite() {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.enabled = true;
		AudioSource.PlayClipAtPoint (clip, gameObject.transform.position);
		}

	void TurnOffSprite() {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.enabled = false	;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
