using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Sends loops or unloops a particle system.")]
	public class LoopParticleSystem : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault fsmGameObject;
		public bool loop;
		public bool recursive = true;

		public override void Reset()
		{
			fsmGameObject = null;
		}

		public override void OnEnter()
		{
			LoopGameObject ();
			Finish();
		}

		void LoopGameObject()
		{
            ParticleSystem.MainModule main;
			GameObject gameObject = fsmGameObject.GameObject.Value;
			if (!recursive) {
				ParticleSystem system = gameObject.GetComponent<ParticleSystem>();
                main = system.main;
                main.loop = loop;
			} else {
				ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem system in particleSystems) {
                    main = system.main;
                    main.loop = loop;
				}
			}
		}
	}
}