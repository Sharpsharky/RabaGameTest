using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
[ExecuteInEditMode]

public class GameObjectNameToTMPro : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		GetComponent<TextMeshPro>().text = gameObject.name;
	}
}
