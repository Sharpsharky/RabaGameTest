using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Sends an event when a room is focused")]
	public class WaitForFocusRoom : FsmStateAction{

		[Tooltip("The room id to wait for focus")]
		public FsmInt roomId;

		[Tooltip("Event to send when the room is focused")]
		public FsmEvent isFocused;

		private ChapterSceneManager _sceneManager;

		public override void Reset(){
			isFocused = null;
			roomId = 0;
		}

		public override void OnEnter(){

			_sceneManager = ChapterSceneManager.instance;

			if (_sceneManager.currentRoomId == roomId.Value) {
				Fsm.Event (isFocused);
				Finish ();
			}
		}

		public override void OnUpdate(){
			if (_sceneManager.currentRoomId == roomId.Value) {
				Fsm.Event (isFocused);
				Finish ();
			}
		}


	}
}
