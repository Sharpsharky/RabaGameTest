using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{
	
	[ActionCategory("_Common")]
	[Tooltip("Fades the scene in or out. Requires a SceneFade component in the scene hierarchy")]
	public class FadeScene : FsmStateAction{
		
		[Tooltip("true = fade scene in, false = fade scene out")]
		public FsmBool fadeIn;

		public FsmEvent finishedEvent;

		public override void Reset(){
			fadeIn = false;
		}

		public override void OnEnter(){
			if (fadeIn.Value) {
				SceneFade.FadeInStatic (OnFadeComplete);
			} else {
				SceneFade.FadeOutStatic (OnFadeComplete);
			}
		}

		private void OnFadeComplete(){
			if (finishedEvent != null) {
				Fsm.Event (finishedEvent);
			}
		}
	}
}
