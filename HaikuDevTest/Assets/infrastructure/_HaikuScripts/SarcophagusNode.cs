using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(CircleCollider2D))]
public class SarcophagusNode : MonoBehaviour {

	//Static Action called by each node every time a piece is snapped, to check for the end condition.
	public static Action<SarcophagusNode> LockBoxNodeOccupiedAction;

	private CircleCollider2D circleCollider = null;

	//Array of adjacent nodes for legit movements.
	public List<SarcophagusNeighbour> neighbours = null;

	// Use this for initialization
	void Start () {
		circleCollider = this.GetComponent<CircleCollider2D> ();
		FindNeighbourNodes ();
	}

	void OnEnable() { }

	void OnDisable() { }

	#region Setup - finding neighbours

	/*There are edge colliders which have their one end near the center of this node and the other end in the neighbouring nodes.
	By finding the overlapping colliders on the other edge of these edge colliders, we can find neighbouring nodes.*/
	void FindNeighbourNodes()
	{
		neighbours = null;
		neighbours = new List<SarcophagusNeighbour> ();
		
		Collider2D[] allColliders = Physics2D.OverlapCircleAll (this.transform.position, 0.2f);

		//Get current edges our node is in contact with
		var edges = 
			from n in allColliders
				where (n is EdgeCollider2D)
			select n;	

		foreach (EdgeCollider2D edge in edges) {
			
			//get all 2d colliders on each point of this edge collider
			Collider2D[] colliders0 = Physics2D.OverlapCircleAll (edge.gameObject.transform.TransformPoint(edge.points[0]), 0.2f);
			Collider2D[] colliders1 = Physics2D.OverlapCircleAll (edge.gameObject.transform.TransformPoint(edge.points[1]), 0.2f);

			//filter out the circle colliders at points[0];
			for (int i = 0; i < colliders0.Length; i++) {
				if (colliders0 [i] is CircleCollider2D) {
					if (colliders0 [i] == this.circleCollider) {
						//we are looking at the wrong end of edge collider.
						break;
					} else {
						if(colliders0 [i].GetComponent<SarcophagusNode>() != null)
							neighbours.Add (new SarcophagusNeighbour(colliders0 [i].GetComponent<SarcophagusNode>(),edge));
					}
				}
			}

			//filter out the circle colliders and points[1];
			for (int i = 0; i < colliders1.Length; i++) {
				if (colliders1 [i] is CircleCollider2D) {
					if (colliders1 [i] == this.circleCollider) {
						//we are looking at the wrong end of edge collider.
						break;
					} else {
						if(colliders1 [i].GetComponent<SarcophagusNode>() != null)
							neighbours.Add (new SarcophagusNeighbour(colliders1 [i].GetComponent<SarcophagusNode>(),edge));
					}
				}
			}
		}
	}
	#endregion


	#region Node Update
	void SetKnobAtThis(SarcophagusJarV2 knob) {
		Vector3 pieceNewTransform = this.transform.position;
		pieceNewTransform = new Vector3 (pieceNewTransform.x, pieceNewTransform.y, pieceNewTransform.z - 1);
		knob.transform.position = pieceNewTransform;
		if (LockBoxNodeOccupiedAction != null) {
 			LockBoxNodeOccupiedAction(this);
		}
	}
	#endregion

	#region collision detection
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("LockBoxPuzzleKnobSprite")) {
			SarcophagusJarV2 knob = other.transform.parent.GetComponent<SarcophagusJarV2> ();
			knob.currentNode = this;
			SetKnobAtThis (knob);
		}
	}
	#endregion

}
