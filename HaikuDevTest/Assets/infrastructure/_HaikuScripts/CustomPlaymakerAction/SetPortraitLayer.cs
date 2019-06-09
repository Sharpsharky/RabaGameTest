using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions {
    
    [ActionCategory("_Common")]
    [Tooltip("Sets a layer in a character portrait")]
    public class SetPortraitLayer : FsmStateAction {
        [RequiredField]
        public FsmString layerName;

        public FsmBool layerIsOn = true;


        public override void Reset() {
            layerName = null;
            layerIsOn = true;
        }

        public override void OnEnter() {
  
            Finish();
        }


    }
}
