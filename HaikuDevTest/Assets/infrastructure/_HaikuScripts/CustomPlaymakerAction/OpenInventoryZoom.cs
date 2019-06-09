using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions {

    [ActionCategory("Inventory")]
    public class OpenInventoryZoom : FsmStateAction {

      

        public FsmInt itemId;

        public override void Reset() {
            itemId = 0;
        }

        public override void OnEnter() {

            ChapterUIManager chapterUIManager = ChapterUIManager.instance;
            if(chapterUIManager != null){
                chapterUIManager.inventoryIconContainer.OpenZoom(itemId.Value);
            }else{
                Debug.LogError("No ChapterUIManager");
            }
            Finish();
        }

    }
}