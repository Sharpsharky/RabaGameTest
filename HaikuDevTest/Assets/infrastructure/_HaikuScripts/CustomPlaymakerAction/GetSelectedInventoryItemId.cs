using UnityEngine;

namespace HutongGames.PlayMaker.Actions {

    [ActionCategory("Inventory")]
    [Tooltip("Gets the selected inventory item id.")]
    public class GetSelectedInventoryItemId : FsmStateAction {

        [RequiredField]
        [UIHint(UIHint.Variable)]
        public FsmInt storeInt;

        public override void Reset() {
            storeInt = 0;
        }

        public override void OnEnter() {
            storeInt.Value = ChapterUIManager.instance.GetSelectedItemId();
            Finish();
        }
    }
}
