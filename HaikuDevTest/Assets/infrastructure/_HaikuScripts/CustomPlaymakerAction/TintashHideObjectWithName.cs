using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Tintash")]
	[Tooltip("Hide Object Routine With Name")]
	public class TintashHideObject : FsmStateAction
	{
		public FsmOwnerDefault fsmObject;

		public FsmString[] name;
		
		public override void Reset()
		{
		}
		
		public override void OnEnter()
		{
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i].Value == Owner.gameObject.name)
				{
					if (fsmObject.OwnerOption == OwnerDefaultOption.UseOwner)
					{
						Fsm.Event("isCorrect");
						break;
					}
				}
			}
		}
	}
}
