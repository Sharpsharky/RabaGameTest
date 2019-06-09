using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("_Common")]
	[Tooltip("Toggles whether or not colliders in the current room of the main camera is active")]
	public class ToggleCurrentRoomColliders : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Clip")]
		[ObjectType(typeof(AudioClip))]
		public FsmBool turnCollidersOn = false;
		public FsmOwnerDefault sendColliderEventTo;
		private GameObject sendColliderEvent;

		private static List<Collider2D> collidersToToggle = new List<Collider2D>();
		private static GameObject toBeToggled;

		public override void Reset()
		{
			sendColliderEvent = null;
			turnCollidersOn = false;
		}

		public override void OnEnter()
		{
			sendColliderEvent = Fsm.GetOwnerDefaultTarget(sendColliderEventTo);
			if (sendColliderEvent == null)
			{
				return;
			}

			if (turnCollidersOn.Value) {
				TurnOnColliders();
			} else {
				TurnOffColliders();
			}
			Finish();
		}

		private void TurnOffColliders() {

			//toBeToggled should be set each time this method is called, to ensure that we are indeed enabling all colliders
			//in the current room on MainCamera
			toBeToggled = ChapterSceneManager.instance.currentRoom.gameObject;
			Debug.Log ("Turning off colliders in " + toBeToggled.name);
			Collider2D[] colliders = toBeToggled.GetComponentsInChildren<Collider2D> ();
			foreach (Collider2D colliderInScene in colliders) {
				if (colliderInScene.enabled) {
					colliderInScene.enabled = false;
					collidersToToggle.Add (colliderInScene);
				}
			}
			PlayMakerFSM[] fsms = sendColliderEvent.GetComponents<PlayMakerFSM>() ;
			foreach (PlayMakerFSM fsm in fsms) {
				fsm.SendEvent ("collidersOff");
			}
		}

		private void TurnOnColliders() {
			ChapterSceneManager sceneManager = ChapterSceneManager.instance;
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

			PlayMakerFSM[] fsms = sendColliderEvent.GetComponents<PlayMakerFSM>() ;
			foreach (PlayMakerFSM fsm in fsms) {
				fsm.SendEvent ("collidersOn");
			}
		}
	}
}
