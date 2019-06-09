using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EmptyColliderGraphic : EmptyGraphic, ICanvasRaycastFilter {

	Collider2D _collider;

	protected override void Awake(){
        base.Awake();
		_collider = GetComponent<Collider2D>();
		_collider.enabled = false;
	}

	public bool IsRaycastLocationValid(Vector2 screenPosition, Camera raycastEventCamera){
		Vector3 worldPoint = raycastEventCamera.ScreenToWorldPoint (screenPosition);
		_collider.enabled = true;
		bool result = _collider.OverlapPoint (worldPoint);
		_collider.enabled = false;
		return result;
	}
}
