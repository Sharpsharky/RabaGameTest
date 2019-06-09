using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZ : MonoBehaviour {

	public float degreesPerSecond = 0.1f;
	public bool clockwise = true;

	void Update () {
		transform.Rotate (Vector3.forward, (clockwise ? -1 : 1) * degreesPerSecond * Time.deltaTime);
	}
}
