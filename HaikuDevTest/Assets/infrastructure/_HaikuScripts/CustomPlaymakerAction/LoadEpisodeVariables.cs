using UnityEngine;

namespace HutongGames.PlayMaker.Actions {
    [ActionCategory("_Common")]
  
    public class LoadEpisodeVariables : FsmStateAction {


        [RequiredField, CompoundArray("Variables to Load", "Key", "Store Result"),Tooltip("Note: Array, GameObject, and Object not supported")]
        public FsmString[] keys;

        [RequiredField,UIHint(UIHint.Variable)]
        public FsmVar[] datas;

        public override void Reset() {
            keys = new FsmString[1];
            datas = new FsmVar[1];
        }

        public override void OnEnter() {

          


            Finish();
        }
    }
}