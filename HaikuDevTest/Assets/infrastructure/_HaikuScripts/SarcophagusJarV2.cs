using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class SarcophagusJarV2 : MonoBehaviour {

	public SarcophagusNode currentNode = null;
	public SarcophagusNode targetNode = null;
	public bool forceMoveBack = false;

	private Vector3 movePosition = Vector3.zero;
	private bool positionDirty = false;
	private SarcophagusNode startingNode;

	public float updateSpeed = 15f;
	public AudioClip slidingSound;

	Vector3 offset;


	// Use this for initialization
	void Start () {
		startingNode = currentNode;
	}

	// Update is called once per frame
	void Update () {
		//Lazy move
		if (positionDirty) {
			transform.position = Vector3.MoveTowards (transform.position, movePosition, Time.deltaTime * updateSpeed);

			if (transform.position == movePosition) {
				positionDirty = false;
			}
		}
	}

	#region Touch/Mouse Input
	void OnMouseDown() {
		var touchPosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
		touchPosition = Camera.main.ScreenToWorldPoint (touchPosition);
		touchPosition.z = 0.0f;
		offset = gameObject.transform.position - touchPosition;

		Helper.PlayAudioIfSoundOn(this.slidingSound);
	}

	void OnMouseDrag() {
		if (!forceMoveBack) {

			//Figure out if which edge colliders we are in contact with

			var center = new Vector2 (transform.position.x, transform.position.y);
			var colliders = Physics2D.OverlapCircleAll (center, 0.1f);

			//Get current edges our ball is in contact with
			var edges = 
				from n in colliders
				where (n is EdgeCollider2D)
				select n;

			//Calculate drag position in world space
			var dragPosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
			dragPosition = Camera.main.ScreenToWorldPoint (dragPosition);
			dragPosition = dragPosition + offset;
			dragPosition.z = 0.0f;


			//If we are in contact with multiple edge colliders, we need to store the position for the edge that 
			//results in the maximum distance from the drag position
			Vector3 finalPosition = transform.position;
			//Transform container = transform.parent;


			foreach (EdgeCollider2D edge in edges) {

				//Get all points of the edge collider, but we are only interested in the starting and ending point 
				//(edge colliders can have more than two points) since our implementation deals in straight lines
				var points = edge.points;

				//Points show up in local space
				for (int i = 0; i < points.Count (); i++) {
					points [i] = edge.gameObject.transform.TransformPoint (points [i]);
				}

			
				//Calculate the edge vector (first point to last point)
				//and the touch vector (first point to touch point)
				Vector3 line = new Vector3 (points.Last ().x - points.First ().x, points.Last ().y - points.First ().y, 0);
				Vector3 drag = new Vector3 (dragPosition.x - points.First ().x, dragPosition.y - points.First ().y, 0);

				Vector3 lineProjection = Vector3.Project (drag, line);

				//We need to check if we overproject, or project in the opposite direction
				lineProjection = Vector3.Dot (lineProjection, line) < 0f ? Vector3.zero : lineProjection;
				lineProjection = lineProjection.magnitude > line.magnitude ? line : lineProjection;

				Vector3 position = new Vector3 (points.First ().x, points.First ().y, 0) + lineProjection;

				if (Vector3.Distance (transform.position, position) >
				   Vector3.Distance (transform.position, finalPosition)) {
					finalPosition = position;
					positionDirty = true;
				}

			}

			movePosition = finalPosition;
		}
	}

	void OnMouseUp() {

		Vector3 finalPosition = currentNode.transform.position;
		float minDistance = Vector3.Distance (transform.position, finalPosition);

		var center = new Vector2 (transform.position.x, transform.position.y);
		var colliders = Physics2D.OverlapCircleAll (center, 0.1f);

		//Get current edges our ball is in contact with
		var edges = 
			from n in colliders
				where (n is EdgeCollider2D)
			select n;
		
		foreach (EdgeCollider2D edge in edges) {
			foreach(SarcophagusNeighbour neigh in this.currentNode.neighbours)
			{
				if(neigh.connectingLine.Equals(edge))
				{
					//This is one of the edges we are on
					if(Vector3.Distance(transform.position,neigh.neighbourNode.transform.position) < minDistance)
					{
						minDistance = Vector3.Distance (transform.position, neigh.neighbourNode.transform.position);
						finalPosition = neigh.neighbourNode.transform.position;
					}
				}
			}
		}

		movePosition = finalPosition;
		positionDirty = true;
		forceMoveBack = false;
	}

	#endregion

	#region collision detection
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Jar")) {
			SarcophagusJarV2 knob = other.transform.GetComponent<SarcophagusJarV2> ();
			Debug.Log("Touching other jar -> " + knob);

			knob.ForceMoveBack();
		}
	}
	#endregion

	#region Public methods
	public void ForceMoveBack() {
		forceMoveBack = true;
		movePosition = currentNode.transform.position;
		positionDirty = true;
	}

	public void Reset() {
		forceMoveBack = true;
		transform.position = this.startingNode.transform.position;
		positionDirty = false;
	}

	#endregion

}
