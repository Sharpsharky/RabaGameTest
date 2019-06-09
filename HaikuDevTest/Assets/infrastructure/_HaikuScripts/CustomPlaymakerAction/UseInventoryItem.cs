using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions {

    [ActionCategory("Inventory")]
    [Tooltip("Increments inventory item use count, and removes it if it has reached the max number of uses")]
    public class UseInventoryItem : FsmStateAction {
        
        public FsmInt itemId;

        public override void OnEnter() {
            InventoryManager.instance.UseItem(itemId.Value);
            Finish();
        }
    }
}

