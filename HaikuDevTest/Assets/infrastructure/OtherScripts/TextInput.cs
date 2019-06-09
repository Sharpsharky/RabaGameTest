using UnityEngine;
using System.Collections;

public class TextInput : MonoBehaviour {
	private string stringToEdit = "Player";
	public Font guiFont;
	private bool nameConfirmed = false;

	void OnGUI () {
		float width = 300;
		float height = 50;
//		GUI.skin.label.fontSize = 50;
		GUIStyle guiStyle = GUI.skin.GetStyle ("Label");
		guiStyle.fontSize = 25;
		guiStyle.font = guiFont;
		stringToEdit = GUI.TextField (new Rect ((Screen.width - width) / 2, (Screen.height - height) / 2, width, height), stringToEdit, 25, guiStyle);
		if (nameConfirmed) {
			PlayerPrefs.SetString(Constants.kPlayerNameKey, stringToEdit);
			Destroy(gameObject);
			// TODO: dialoguemanager
//			Debug.Log("Destroying self");
		}
	}

	public void NameConfirmed() {
		nameConfirmed = true;
	}
}
