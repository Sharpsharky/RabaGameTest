using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{

	[ActionCategory("Inventory")]
	[Tooltip("Checks to see if a certain item is in the player's inventory")]
	public class CheckHasItem : FsmStateAction{

	
		public FsmEvent hasItemEvent;

		public FsmEvent noItemEvent;

		public FsmInt itemId;

		public override void OnEnter(){
			if (InventoryManager.instance.HasItem (itemId.Value)) {
				Fsm.Event (hasItemEvent);
			} else {
				Fsm.Event (noItemEvent);
			}
			Finish();
		}


	}
}
