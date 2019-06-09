using UnityEngine;
using System.Collections;

public class DoesNotRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {
		// Fix it so that the original orientation is kept (even during rotation)
		transform.eulerAngles = new Vector3(0, 0, 0);
	}
}
