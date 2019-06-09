using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("_Common")]
    [Tooltip("Fades gameobject with sprite component")]
    public class ParticleStopEmit : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;

        public FsmBool includeChildren = false;

        public override void Reset()
        {
            gameObject = null;
            includeChildren = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // Get user choice
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            // Check
            if (go != null)
            {
                if (includeChildren.Value)
                {
                    var particleSystems = go.GetComponentsInChildren<ParticleSystem>();
                    foreach (var ps in particleSystems)
                    {
                        ps.Stop();
                    }
                }
                else
                {
                    var particleSys = go.GetComponent<ParticleSystem>();
                    if (particleSys)
                    {
                        particleSys.Stop();
                    }
                }
            }
            Finish();
        }
    }
}
