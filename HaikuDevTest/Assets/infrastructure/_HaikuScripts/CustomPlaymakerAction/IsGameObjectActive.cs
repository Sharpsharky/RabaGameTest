

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Logic)]
    [Tooltip("Tests if a GameObject Variable has a null value. E.g., If the FindGameObject action failed to find an object.")]
    public class IsGameObjectActive : FsmStateAction
    {
        [RequiredField]
        [UIHint(UIHint.Variable)]
        [Tooltip("The GameObject variable to test.")]
        public FsmOwnerDefault gameObject;

        [Tooltip("Event to send if the GamObject is active.")]
        public FsmEvent isActive;

        [Tooltip("Event to send if the GamObject is NOT active.")]
        public FsmEvent isNotActive;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the result in a bool variable.")]
        public FsmBool storeResult;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;

        GameObject go;
        private bool sendEvent = false;

        public override void Reset()
        {
            go = null;
            gameObject = null;
            isActive = null;
            isNotActive = null;
            storeResult = null;
            everyFrame = false;
            sendEvent = false;
        }

        public override void OnEnter()
        {
            DoIsGameObjectNull();

            if (!everyFrame)
            {
                if (sendEvent == false)
                {
                    Fsm.Event(isNotActive);
                }
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoIsGameObjectNull();
        }

        void DoIsGameObjectNull()
        {
            go = Fsm.GetOwnerDefaultTarget(gameObject);

            if (go == null) return;

            var goIsActive = go.activeSelf;

            if (storeResult != null)
            {
                storeResult.Value = goIsActive;
            }
            
            Fsm.Event(goIsActive == true ? isActive : isNotActive);
            sendEvent = true;
        }
    }
}