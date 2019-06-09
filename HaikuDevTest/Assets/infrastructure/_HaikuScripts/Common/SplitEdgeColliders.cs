using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(EdgeCollider2D))]
[ExecuteInEditMode]
public class SplitEdgeColliders : MonoBehaviour {

	public int _noOfPointsInEachCollider = 2;

	#region Editor Helper Methods
	public void AddEdgeCollider () {
		EdgeCollider2D newCollider = gameObject.AddComponent<EdgeCollider2D> ();

		Vector2[] p = new Vector2[2];
		p [0] = new Vector2 (1f, 1f);
		p [1] = new Vector2 (2f, 2f);

		newCollider.points = p;
	}

	public void SplitColliders () {
		EdgeCollider2D[] colliders = GetComponents<EdgeCollider2D> ();

//		List<EdgeCollider2D> collidersToDestroy = new List<EdgeCollider2D> ();

		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].points.Length > _noOfPointsInEachCollider) {
				DivideIntoMultipeColliders (colliders [i]);

				Vector2[] originalPoints = new Vector2[_noOfPointsInEachCollider];
				for (int j = 0; j < originalPoints.Length; j++) {
					originalPoints [j] = colliders [i].points [j];
				}
				colliders [i].points = originalPoints;
			}
		}
	}

	private void DivideIntoMultipeColliders (EdgeCollider2D collider) {
		EdgeCollider2D newCollider = gameObject.AddComponent<EdgeCollider2D> ();
		Vector2 [] points = new Vector2[_noOfPointsInEachCollider];

		int i = _noOfPointsInEachCollider-1;
		int counter = 0;
		while (i < collider.points.Length) {
			points [counter] = new Vector2(collider.points [i].x, collider.points [i].y);

			counter++;
			if (counter >= _noOfPointsInEachCollider) {
				newCollider.points = points;

				newCollider = gameObject.AddComponent<EdgeCollider2D> ();
				newCollider.points = new Vector2[_noOfPointsInEachCollider];
				counter = 0;
			} else {
				i++;
			}
		}

//		newCollider.points = points;
	}
	#endregion
}
