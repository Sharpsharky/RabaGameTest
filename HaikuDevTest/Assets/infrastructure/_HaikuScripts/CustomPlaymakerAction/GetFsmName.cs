using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("_Common")]
    [Tooltip("Get FSM name")]
    public class GetFsmName : FsmStateAction
    {
        public FsmString storeValue;

        public override void OnEnter()
        {
            storeValue.Value = Fsm.Name;
            Finish();
        }
    }
}
