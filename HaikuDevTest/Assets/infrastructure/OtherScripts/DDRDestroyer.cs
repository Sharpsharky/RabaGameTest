using UnityEngine;
using System.Collections;

public class DDRDestroyer : MonoBehaviour {
	public DDRManager manager;
	
	void OnTriggerEnter2D(Collider2D collider) {
		GameObject collided = collider.gameObject;
//		string tag = collided.tag;
//		Debug.Log("Trigger entered " + tag);

		manager.DestroyPiece(collided);
	}
}
