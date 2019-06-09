using UnityEngine;
using System.Collections;

public class GizmoForEmptyGO : MonoBehaviour {

	public Color color;
	public float radius;
	void OnDrawGizmosSelected() {
		Gizmos.color = color;
		if (radius == 0)
			radius = 0.25f;

		if (color.a == 0)
			color.a = 255;
		
		Gizmos.DrawSphere(transform.position, radius);
	}
}
