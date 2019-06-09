using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Toggle the tap blocker, which blocks everythign below dialogue.")]
	public class ToggleTapBlocker : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Do you want the tap blocker on or off")]
		public bool blockerOn;

	

		public override void OnEnter()
		{
            ChapterUIManager.instance.EnableTapBlocker(blockerOn);
			Finish();
		}

	}
}
