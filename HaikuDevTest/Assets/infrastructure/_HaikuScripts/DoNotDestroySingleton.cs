using UnityEngine;
using System.Collections;

public class DoNotDestroySingleton : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GameObject[] existingObjects = GameObject.FindGameObjectsWithTag (gameObject.tag);
        if (existingObjects.Length > 1) {
            Destroy (gameObject);
        } else {
            DontDestroyOnLoad (gameObject);
        }
    }
}
