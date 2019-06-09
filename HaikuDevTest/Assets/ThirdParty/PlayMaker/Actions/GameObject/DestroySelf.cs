// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Destroys the Owner of the Fsm! Useful for spawned Prefabs that need to kill themselves, e.g., a projectile that explodes on impact.")]
	public class DestroySelf : FsmStateAction
	{
		[Tooltip("Detach children before destroying the Owner.")]
		public FsmBool detachChildren;
		[Tooltip("If you want to save the scene after marking the object as destroyed.")]
		public FsmBool saveSceneAfterMarkDestroyed;

		public override void Reset()
		{
			detachChildren = false;
			saveSceneAfterMarkDestroyed = true;
		}

		public override void OnEnter()
		{
			if (Owner != null)
			{
				

				if (detachChildren.Value)
				{
					Owner.transform.DetachChildren();
				}
				
				Object.Destroy(Owner);
            }
			
			Finish();
		}
	}
}