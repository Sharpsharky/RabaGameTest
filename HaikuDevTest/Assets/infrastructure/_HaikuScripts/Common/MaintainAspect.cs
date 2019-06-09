using UnityEngine;
using System.Collections;

public class MaintainAspect : MonoBehaviour {

//	[Range(0.0f, 1.0f)]
//	public float widthOrHeight;

	public Vector2 aspect;


	void Start() {
		Debug.Log ("Lossy Scale is " + transform.lossyScale);


		float aspectRatio = aspect.x / aspect.y;
		float currentAspectRatio = transform.lossyScale.x / transform.lossyScale.y;

		float xMult = aspectRatio / currentAspectRatio;


		Vector3 localScale = transform.localScale;
		localScale.x *= xMult;

		transform.localScale = localScale;

	}
}
