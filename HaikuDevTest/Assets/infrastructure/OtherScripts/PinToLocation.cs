using UnityEngine;
using System.Collections;

public class PinToLocation : MonoBehaviour {
	public enum PinTo {TopLeft, TopRight, BottomLeft, BottomRight, TopMiddleRight };
	public PinTo pinTo;
	
	// Use this for initialization
	void Start () {
		switch (pinTo) {
		case (PinTo.BottomLeft):
			transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0,0,1));
			break;
		case (PinTo.BottomRight):
			transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1,0,1));
			break;
		case (PinTo.TopLeft):
			transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0,1,1));
			break;
		case (PinTo.TopRight):
			transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1,1,1));
			break;
		case (PinTo.TopMiddleRight):
			transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.65f,0.75f,1));
			break;}
	}
}
