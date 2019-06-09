using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Sends a task complete to the hint panel.")]
	public class HintComplete : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Hint Key")]
		public FsmString hintKey;	

		public override void Reset() {
			hintKey = null;
		}
		
		public override void OnEnter() {
			SendHintComplete ();
			Finish();
		}
		
		void SendHintComplete() {
			Helper.SendHintComplete(hintKey.Value);
		}
	}
}