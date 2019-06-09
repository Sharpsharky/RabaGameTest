using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour {

	[Tooltip("The object to rotate towards. If this is null then will be rotated towards mouse.")]
	public Transform _rotateTowardsObject;

	[Tooltip("The object that will be rotated. If this is null then the object that this " +
		"script is attached to will be rotated.")]
	public GameObject _objectToRotate;

	[Tooltip("If this is true then the object will only rotate during mousedown and mousedrag." +
		"Else the object will keep on rotating towards the mouse position.")]
	public bool _rotateOnMouseDown = true;

	private bool _isMouseDown = false;

	void Awake () {
		if (_objectToRotate == null) {
			_objectToRotate = gameObject;
		}
	}

	void Update () {
		if (!_rotateOnMouseDown) {
			RotateGameobjectTowardsMouse (_objectToRotate);
		}
	}

	void OnMouseDown () {
		_isMouseDown = true;
		if (_rotateOnMouseDown) {
			RotateGameobjectTowardsMouse (_objectToRotate);
		}
	}

	void OnMouseDrag () {
		if (_isMouseDown && _rotateOnMouseDown) {
			RotateGameobjectTowardsMouse (_objectToRotate);
		}
	}

	void OnMouseUp () {
		_isMouseDown = false;
	}

	public void RotateGameobjectTowardsMouse (GameObject go) {
		var mouse = Input.mousePosition;
		if (_rotateTowardsObject != null) {
			mouse = Camera.main.WorldToScreenPoint(_rotateTowardsObject.position);
		}
		var screenPoint = Camera.main.WorldToScreenPoint(go.transform.position);
		var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
		var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
		go.transform.localRotation = Quaternion.Euler(0, 0, angle);
	}
}
