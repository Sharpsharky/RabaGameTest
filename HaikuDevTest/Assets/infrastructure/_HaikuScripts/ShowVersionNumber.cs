using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class ShowVersionNumber : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Version: " + Application.version);
		GetComponent<TextMeshPro>().text = "Version: " + Application.version;
	}
}
