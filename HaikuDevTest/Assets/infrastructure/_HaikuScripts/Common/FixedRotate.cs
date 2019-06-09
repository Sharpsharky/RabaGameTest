using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// Fixed rotate.
/// 
/// Note: The gameobject must be placed at localPosition (0, 0) for the rotation to work correctly.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class FixedRotate : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IDragHandler{

	public bool controlEnabled = true;

	public float deadRaduis;

	//Making a reference to ball here to avoid the bug where ball loses the line.
	//If the ball is on Moving Circle and its position is dirty, the circle shouldn't rotate
	//to keep the ball position in line.
	//ToDo: remove this reference and respective usages when using this script somewhere else.
//	public TrackBall ball = null;

	[Range(1, 180)]
	public float deltaAngle;

	bool touchRotate = false;

	Quaternion restingRotation = Quaternion.identity;

	float touchDownLookZAngle;

	bool dirtyRotation = false;

	public static Action RotationEnded;

	// Use this for initialization
	void Start () {
		restingRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (controlEnabled) {
			if (!touchRotate && dirtyRotation) {

				transform.rotation = Quaternion.RotateTowards (transform.rotation, 
					restingRotation, Time.deltaTime * 100);

				if (transform.rotation == restingRotation) {
					dirtyRotation = false;

					//-- Call Rotation Tween Complete Handler here
					if (RotationEnded != null) {
						RotationEnded ();
					}
				}
			}
		}
	}

    public void OnPointerDown(PointerEventData pointerEventData) {
		if (controlEnabled) {
			//Calculate drag position in world space
			var touchPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
			touchPos = Camera.main.ScreenToWorldPoint (touchPos);
			touchPos.z = 0.0f;

			if (Vector3.Distance (transform.position, touchPos) > deadRaduis) {
				touchRotate = true;

				Vector3 diff = touchPos - transform.parent.position;
				diff.Normalize ();

				touchDownLookZAngle = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;

			}
		}
	}

    public void OnDrag(PointerEventData pointerEventData) {
		if (controlEnabled) {
			if (touchRotate) {
				//Calculate drag position in world space

				var touchPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
				touchPos = Camera.main.ScreenToWorldPoint (touchPos);
				touchPos.z = 0.0f;

				Vector3 diff = touchPos - transform.parent.position;
				diff.Normalize ();

				float rot_z = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;
				rot_z = rot_z - touchDownLookZAngle;

				float updatedZRotation = transform.eulerAngles.z + rot_z;

				transform.rotation = Quaternion.Euler (0f, 0f, updatedZRotation);

				touchDownLookZAngle = Mathf.Atan2 (diff.y, diff.x) * Mathf.Rad2Deg;

				dirtyRotation = true;
			}
		}
	}

    public void OnPointerUp(PointerEventData pointerEventData) {
		if (controlEnabled) {
			touchRotate = false;

			float angle = transform.rotation.eulerAngles.z;

			float rotationAngle = angle % deltaAngle;

			float previousAngle = angle - rotationAngle;
			float nextAngle = previousAngle + deltaAngle;

//		//Check which resting angle is the nearest
			float restingAngle = Mathf.Abs (nextAngle - angle) < Mathf.Abs (angle - previousAngle) ? nextAngle : previousAngle;


			restingRotation = Quaternion.Euler (0f, 0f, restingAngle);
		}
	}
		
}
