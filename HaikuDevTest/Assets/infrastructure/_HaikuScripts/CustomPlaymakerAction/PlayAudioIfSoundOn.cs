using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{
	
	[ActionCategory("_Common")]
	[Tooltip("Plays an audio clip if sound is enabled")]
	public class PlayAudioIfSoundOn : FsmStateAction{

		[RequiredField]
		[Title("Audio Clip")]
		[ObjectType(typeof(AudioClip))]
		public FsmObject clip;

		[HasFloatSlider(0, 1)]
		public FsmFloat volume = 1f;

		public FsmFloat delay = 0f;

		[Title("Optional Audio Source"), Tooltip("Set this if you need the sound to be played " +
			"from a particular AudioSource or if you need to stop the sound later. Note that the sound will " +
			"loop if audioSource.loop is set to true")]
		[ObjectType(typeof(AudioSource))]
		public FsmObject audioSource;


		private float _elapsedTime;

		public override void Reset(){
			clip = null;
			volume = 1;
			delay = 0f;
		}

		public override void OnEnter(){
			_elapsedTime = 0;
			if (delay.Value <= 0f) {
				PlaySound ();
			}
		}


		public override void OnUpdate(){
			_elapsedTime += Time.deltaTime;
			if (_elapsedTime >= delay.Value) {
				PlaySound ();
			}
		}

		private void PlaySound(){
			if (PlayerPrefs.HasKey(Constants.kSoundPref)) {
				if (PlayerPrefs.GetInt(Constants.kSoundPref) == 0) {
					//Debug.Log ("not playing sound");
					Finish ();
					return;
				}
			}

			AudioClip audioClip = clip.Value as AudioClip;

			if (audioClip != null) {
				AudioSource source = audioSource.Value as AudioSource;

				if (source != null) {
					if (source.loop) {
						source.clip = audioClip;
						source.volume = volume.Value;
						source.Play ();
					} else {
						source.PlayOneShot (audioClip, volume.Value);
					}

				} else {
					AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume.Value);
				}
			}
			Finish();
		}


	}
}
