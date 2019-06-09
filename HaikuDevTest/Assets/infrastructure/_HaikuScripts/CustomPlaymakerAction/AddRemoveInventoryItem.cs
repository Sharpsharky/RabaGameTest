using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{

    [ActionCategory("Inventory")]
	[Tooltip("Increments or decrements an inventory item by item id.")]
	public class AddRemoveInventoryItem : FsmStateAction{

		public enum InventoryActionType{
			Add,
			Remove
		}

		[ObjectType(typeof(InventoryActionType))]
		public FsmEnum actionType;

		public FsmInt itemId;

		public override void Reset(){
			itemId = 0;
		}

		public override void OnEnter(){

			InventoryManager inventoryManager = InventoryManager.instance;

			if (inventoryManager != null) {
				InventoryActionType inventoryActionType = (InventoryActionType)actionType.Value;

				if (inventoryActionType == InventoryActionType.Add) {
					inventoryManager.AddItem (itemId.Value);
				} else {
					inventoryManager.RemoveItem(itemId.Value);
				}
			}

			Finish ();
		}

	}
}