using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundDuringAnimation : MonoBehaviour {


	public void PlaySound(AudioClip pAudioClip){
		if (pAudioClip == null) {
			return;
		}

		Helper.PlayAudioIfSoundOn (pAudioClip);
	}

	//Animation System does not support multiple parameters or method overloading
	public void PlaySoundSetVolume(AnimationEvent pEvent){
		AudioClip audioClip = pEvent.objectReferenceParameter as AudioClip;

		if (audioClip == null) {
			return;
		}

		float volume = pEvent.floatParameter;

		Helper.PlayAudioIfSoundOn (audioClip, volume);
	}




}
