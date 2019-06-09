using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enables and disables and AudioSource when sound settings are turned on and off
[RequireComponent(typeof(AudioSource))]
public class AudioSourceHelper : MonoBehaviour {

	private static List<AudioSourceHelper> s_instances;

	public static void NotifySoundSettingChange(bool pSoundIsOn){
		if (s_instances == null) {
			return;
		}

		foreach (AudioSourceHelper helper in s_instances) {
			helper.EnableSound (pSoundIsOn);
		}
	}

	private AudioSource[] _sources;

	private void Awake(){
		if (s_instances == null) {
			s_instances = new List<AudioSourceHelper>();
		}

		s_instances.Add (this);
		_sources = GetComponents<AudioSource> ();
	}
		
	private void Start(){
		bool disableSound = PlayerPrefs.HasKey(Constants.kSoundPref) && PlayerPrefs.GetInt(Constants.kSoundPref) == 0;
		EnableSound(!disableSound);
	}

	private void EnableSound(bool pIsSoundOn){
		foreach (AudioSource source in _sources) {
			source.enabled = pIsSoundOn;
		}
	}

	private void OnDestroy(){
		s_instances.Remove (this);
	}


}
