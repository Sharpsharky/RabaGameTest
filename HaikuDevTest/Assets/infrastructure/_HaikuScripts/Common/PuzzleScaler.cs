using UnityEngine;
using System.Collections;

public class PuzzleScaler : MonoBehaviour {
	
	public GameObject puzzlePrefab;
	public float scaleToApply = 1.0f;
	public PlayMakerFSM winConditionObject;
	public bool consistentScale = false;
	
	void Start() {
		
		GameObject puzzle = (GameObject)Instantiate (puzzlePrefab);
		puzzle.transform.parent = gameObject.transform;
		puzzle.transform.localPosition = Vector3.zero;

		if (consistentScale) {
			puzzle.transform.localScale = new Vector3(scaleToApply, scaleToApply, scaleToApply);
		} else {
			this.Scale (puzzle);
		}
	}

	private void Scale(GameObject go) {

		// we need to convert the scale x of the gameobject to scaleToApply while maintaining aspect ratio
		Vector3 goLocalScale = go.transform.localScale;

		float newScaleY = (goLocalScale.y * scaleToApply) / goLocalScale.x;

		goLocalScale.x = scaleToApply;
		goLocalScale.y = newScaleY;

		go.transform.localScale = goLocalScale;
	}

	public void PuzzleComplete() {

		winConditionObject.SendEvent("won");
	}
}
