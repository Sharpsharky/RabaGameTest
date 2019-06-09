using UnityEngine;

namespace HutongGames.PlayMaker.Actions {
	[ActionCategory("_Common")]
	[Tooltip("Sends an event when all the  particles in a given particle system are dead")]
	public class WaitForParticlesDone : FsmStateAction {

        [CheckForComponent(typeof(ParticleSystem))]
		[Tooltip("The GameObject with a ParticleSystem attached.")]
		public FsmOwnerDefault particleSystemGameObject;

        [Tooltip("Event to send when all the particles are dead")]
        public FsmEvent finishedEvent;

        float _timeSinceLastCheck;

        const float TIME_BETWEEN_CHECKS = 0.5f;

        ParticleSystem _particleSystem;

		public override void Reset() {
            finishedEvent = null;
            particleSystemGameObject = null;
		}

		public override void OnEnter() {
            
            if(particleSystemGameObject == null){
                Finish();
                return;
            }
            _timeSinceLastCheck = TIME_BETWEEN_CHECKS;
            _particleSystem = particleSystemGameObject.GameObject.Value.GetComponent<ParticleSystem>();
		}

		public override void OnUpdate() {
            _timeSinceLastCheck += Time.deltaTime;
            if(_timeSinceLastCheck >= TIME_BETWEEN_CHECKS){
                _timeSinceLastCheck = 0;
                if(!_particleSystem.IsAlive(true)){
                    Fsm.Event(finishedEvent);
                    Finish();
                }
            }
		}


	}
}

