using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Tintash")]
	[Tooltip("Show Object Routine")]
	public class TintashShowObject : FsmStateAction
	{
		public FsmOwnerDefault fsmObject;
		
		public override void Reset()
		{
		}
		
		public override void OnEnter()
		{
			if (Fsm.EventData.StringData == Owner.gameObject.name)
			{
				if (fsmObject.OwnerOption == OwnerDefaultOption.UseOwner)
				{
					Fsm.Event("isCorrect");
				}
			}
		}
	}
}
