using UnityEngine;

namespace HutongGames.PlayMaker.Actions {
    
    [ActionCategory("_Common")]
    public class SaveEpisodeVariables : FsmStateAction {

       
        [RequiredField, CompoundArray("Variables to Save", "Key", "Value"),Tooltip("Note: Array, GameObject, and Object not supported")]
        public FsmString[] keys;

        [RequiredField]
        public FsmVar[] datas;


        public override void Reset() {
            keys = new FsmString[1];
            datas = new FsmVar[1]; 
        }

        T GetValue<T>(FsmVar pVar){
            object value = PlayMakerUtils.GetValueFromFsmVar(Fsm, pVar);
            return (T)value;
        }

        public override void OnEnter() {

   


            Finish();
        }
    }
}
