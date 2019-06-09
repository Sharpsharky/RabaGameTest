using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class ToggleOffChildColliders : MonoBehaviour {
	private static List<Collider2D> collidersToToggle = new List<Collider2D>();

	//The list of colliders which were turned off, only these colliders which exist in the respective
	// room should be turned back on toggleOnColliders.
	private GameObject toBeToggled;

	//make it a class variable so that it might just needed to be initialized once. GetComponent is and expensive call.
	private ChapterSceneManager sceneManager = null;

	void Start () {
	
	}

	/*Get all the enabled colliders in the current room, add them to a list for later use and disable them.*/
	public void turnOffColliders() {
		//only initialize if the mainCameraScript is not initialized before. save extra calls to GetComponent.
		if (sceneManager == null)
			sceneManager = ChapterSceneManager.instance;
		
		//toBeToggled should be set each time this method is called, to ensure that we are indeed enabling all colliders
		//in the current room on MainCamera
		toBeToggled = sceneManager.currentRoom.gameObject;
		Debug.Log ("Turning off colliders in " + toBeToggled.name);
        Collider2D[] colliders = toBeToggled.GetComponentsInChildren<Collider2D> ();
        foreach (Collider2D colliderInScene in colliders) {
			if (colliderInScene.enabled) {
				colliderInScene.enabled = false;
				collidersToToggle.Add (colliderInScene);
			}
        }
		PlayMakerFSM[] fsms = this.GetComponents<PlayMakerFSM>() ;
		foreach (PlayMakerFSM fsm in fsms) {
			fsm.SendEvent ("collidersOff");
		}
    }

	// "OLD COMMENT": When turning on colliders with nested calls tot his, we get undefined since the second time you turn off the colliders
	// you no longer have a reference to the original tag.

	/*New Description of this method: "This methods turns on all the colliders in a room, the conditions for turning them on are:
		1)they should be in the toBeToggled list, this ensures that they were actually enabled before they were disabled
		2)they exist in the current room of Main Camera Script."
	*/
	public void turnOnColliders() {
		//only initialize if the mainCameraScript is not initialized before. save extra calls to GetComponent.
		if(sceneManager == null)
			sceneManager = ChapterSceneManager.instance;
		
		//toBeToggled should be set each time this method is called, to ensure that we are indeed enabling all colliders
		//in the current room on MainCamera
		GameObject roomGameObject = sceneManager.currentRoom.gameObject;
		if (toBeToggled == null) {
			turnOnColliders(roomGameObject);
		} else {
			if (toBeToggled.Equals(roomGameObject)) {
				turnOnColliders(roomGameObject);
			} else {
				turnOnColliders(toBeToggled); // You have to turn on the previous room that you have turned off colliders or else we will leave it permanently off
				turnOnColliders(roomGameObject);
			}
		}
		CollidersOff();
	}

	public void turnOnColliders(GameObject turnCollidersOnFor) {
		Debug.Log ("Turning on colliders in " + turnCollidersOnFor.name);
		if (turnCollidersOnFor != null) {
			Collider2D[] colliders = turnCollidersOnFor.GetComponentsInChildren<Collider2D> ();
			foreach (Collider2D colliderInScene in colliders) {
				if (collidersToToggle.Contains(colliderInScene)) {
					colliderInScene.enabled = true;
				}
			}


		}
	}

	private void CollidersOff() {
		// Clear dictionary, setting it to null releases the memory.
		collidersToToggle = null;
		Debug.Log ("CollidersToToggle cleared");
		collidersToToggle = new List<Collider2D>();

		PlayMakerFSM[] fsms = this.GetComponents<PlayMakerFSM>() ;
		foreach (PlayMakerFSM fsm in fsms) {
			fsm.SendEvent ("collidersOn");
		}
	}
}