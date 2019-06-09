using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{

	[ActionCategory("_Common")]
	[Tooltip("Sends an event to any PlayMakerFSM with a corresponding SceneEventListener component")]
	public class SendSceneEvent : FsmStateAction{

		public FsmString eventName;

		public override void OnEnter(){
			SceneEventDispatcher.instance.SendSceneEvent (eventName.Value);
			Finish ();
		}
	}
}
