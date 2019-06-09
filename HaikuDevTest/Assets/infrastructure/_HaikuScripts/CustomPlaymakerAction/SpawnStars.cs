using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker.Actions;


namespace HutongGames.PlayMaker.Actions {

    public class SpawnStars : FsmStateAction {
        public FsmOwnerDefault spawnLocationObject;
        public FsmString starCollectId;

        public override void Reset() {
            starCollectId = null;
        }

        public override void OnEnter() {
            GameObject go = Fsm.GetOwnerDefaultTarget(spawnLocationObject);
            ChapterUIManager.instance.starCountDisplay.SpawnStars(starCollectId.Value, go.transform.position);
            Finish();
        }
    }
}


