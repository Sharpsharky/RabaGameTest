using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{

	[ActionCategory("_Common")]
	public class SendMessageByName : FsmStateAction{

		public FsmOwnerDefault messageReceiver;
		public FsmString methodName;

		public override void OnEnter(){
			GameObject go = Fsm.GetOwnerDefaultTarget (messageReceiver);
			go.SendMessage (methodName.Value);
			Finish ();
		}
	}

}