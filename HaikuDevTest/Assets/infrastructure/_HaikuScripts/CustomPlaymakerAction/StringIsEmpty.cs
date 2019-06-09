using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HutongGames.PlayMaker.Actions{
	
	[ActionCategory("_Common")]
	[Tooltip("Checks to see if a string is empty")]
	public class StringIsEmpty : FsmStateAction{

		[RequiredField]
		public FsmString stringToCheck;

		public FsmEvent emptyEvent;

		public FsmEvent notEmptyEvent;

		public override void OnEnter(){
			if (string.IsNullOrEmpty (stringToCheck.Value)) {
				Fsm.Event (emptyEvent);
			} else {
				Fsm.Event (notEmptyEvent);
			}
			Finish();
		}
	}
}
