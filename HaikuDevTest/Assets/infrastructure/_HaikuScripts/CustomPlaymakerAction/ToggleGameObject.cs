
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.GameObject)]
    [Tooltip("Activates/deactivates a Game Object.")]
    public class ToggleGameObject : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The GameObject to activate/deactivate.")]
        public FsmOwnerDefault gameObject;

        public override void Reset()
        {
            gameObject = null;

        }

        public override void OnEnter()
        {
            DoActivateGameObject();
            Finish();
        }



        void DoActivateGameObject()
        {
            // If null return
            if (gameObject == null)
            {
                return;
            }

            // Get game object from FSM variable
            var go = Fsm.GetOwnerDefaultTarget(gameObject);

            // If null return
            if (go == null)
            {
                return;
            }

            // Toggle
            go.SetActive(!go.activeSelf);
        }


    }
}