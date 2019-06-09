using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorDeletePlayerPrefs : MonoBehaviour {

	[MenuItem("Tools/Delete All PlayerPrefs")]
	static public void DeleteAllPlayerPrefs() {
		PlayerPrefs.DeleteAll();
	}
}
