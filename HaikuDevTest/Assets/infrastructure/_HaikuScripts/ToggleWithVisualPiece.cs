using UnityEngine;
using System.Collections;

public class ToggleWithVisualPiece : MonoBehaviour {
	public bool shouldBePressed;
	public bool isPressed;
	public ToggleWithVisualManager manager;

	public bool isCorrect() {
		return (shouldBePressed == isPressed);
	}

	void OnMouseDown() {
		isPressed = !isPressed;
		GetComponent<Renderer>().enabled = isPressed;
		manager.PieceToggled(this);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
