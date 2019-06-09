using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{

    [ActionCategory("Inventory")]
	[Tooltip("Checks to see if a certain inventory item is selected")]
	public class CheckInventoryItemSelected : FsmStateAction{

        [RequiredField]
        public FsmInt itemId;

		public FsmEvent selectedEvent;

		public FsmEvent notSelectedEvent;

		public override void OnEnter(){
            bool selected = itemId.Value == ChapterUIManager.instance.GetSelectedItemId();
            if(selected){
                Fsm.Event(selectedEvent);
            }else{
                Fsm.Event(notSelectedEvent);
            }
			Finish();
		}


	}
}
