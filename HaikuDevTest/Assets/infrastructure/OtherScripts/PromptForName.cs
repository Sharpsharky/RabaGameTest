using UnityEngine;
using System.Collections;

public class PromptForName : MonoBehaviour {
	TouchScreenKeyboard keyboard;
	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown() {
		Debug.Log("opening touchscreen keyboard");
		keyboard = TouchScreenKeyboard.Open("name", TouchScreenKeyboardType.Default, false, false, false, false, "name");
	}
	
	// Update is called once per frame
	void Update () {
		if (keyboard != null) {
			if (keyboard.active) {
				// Trim text to 10 characters
				if (keyboard.text.Length > 10) {
					keyboard.text = keyboard.text.Substring(0, 10);
				}
			}
            if (keyboard.status == TouchScreenKeyboard.Status.Done) {
				Debug.Log("Name: " + keyboard.text);
				PlayerPrefs.SetString(Constants.kPlayerNameKey, keyboard.text);
//				Destroy (this.gameObject);

			}
		}
	}
}
