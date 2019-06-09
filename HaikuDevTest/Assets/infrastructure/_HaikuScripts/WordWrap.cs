using UnityEngine;
using System.Collections;

public class WordWrap : MonoBehaviour {
	private TextSize ts;

	// Use this for initialization
	void Start () {
		ts = new TextSize(gameObject.GetComponent<TextMesh>());
	}

	public void wrapTextTo(float wrapTo) {
//		print (ts.width);
		ts.FitToWidth (wrapTo);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
