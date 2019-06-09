using UnityEngine;
using System.Collections;

public class StopDeviceRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AllowRotation(bool allowRotation) {
		Debug.Log ("Setting rotation to " + allowRotation.ToString());
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeLeft = allowRotation;
		Screen.autorotateToLandscapeRight = allowRotation;
	}
}
