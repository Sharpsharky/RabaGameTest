using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{

	[ActionCategory("_Common")]
	[Tooltip("Gradually changes the volume of an AudioSource over time")]
	public class AudioVolumeFade : FsmStateAction{

		[HasFloatSlider(0, 1)]
		public FsmFloat goalVolume = 1f;

		public FsmFloat duration = 1f;

		[RequiredField]
		[ObjectType(typeof(AudioSource))]
		public FsmObject audioSource;

		public FsmEvent finishedEvent;

		private AudioSource _audioSource;
		private float _initialVolume;

		private float _elapsedTime;


		public override void Reset(){
			audioSource = null;
			goalVolume = 1f;
			duration = 1f;
		}

		public override void OnEnter(){
			_elapsedTime = 0;
			_audioSource = audioSource.Value as AudioSource;
			_initialVolume = _audioSource.volume;
		}

		public override void OnUpdate(){
			_elapsedTime += Time.deltaTime;

			if (_elapsedTime >= duration.Value) {
				_audioSource.volume = goalVolume.Value;

				if (finishedEvent != null) {
					Fsm.Event (finishedEvent);
				}
				Finish ();
				return;
			}

			float t = Mathf.InverseLerp (0, duration.Value, _elapsedTime);
			_audioSource.volume = Mathf.Lerp (_initialVolume, goalVolume.Value, t);
		}


	}
}
