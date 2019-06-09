using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ToggleButton))]
public class PlayerPreferenceToggleButton : MonoBehaviour {

	public enum PlayerPreferenceType{
		Music,
		Sound
	}

	[SerializeField]
	PlayerPreferenceType _type;

	ToggleButton _toggleButton;


	void OnEnable(){
		if (_toggleButton == null) {
			_toggleButton = GetComponent<ToggleButton> ();
		}

		_toggleButton.OnToggle += OnToggle;

		string key = GetPrefString ();
		bool isOn;

		if (PlayerPrefs.HasKey (key)) {
			isOn = PlayerPrefs.GetInt (key) == 1;
		} else {
			isOn = true;
		}

		_toggleButton.SetOn (isOn);

	}

	void OnDisable(){
		_toggleButton.OnToggle -= OnToggle;
	}

	string GetPrefString(){
		if (_type == PlayerPreferenceType.Music) {
			return Constants.kMusicPrefs;
		} else {
			return Constants.kSoundPref;
		}
	}

	void OnToggle(ToggleButton pButton, bool isOn){
		string key = GetPrefString ();
        PlayerPrefs.SetInt(key, isOn ? 1 : 0);

		if (_type == PlayerPreferenceType.Sound) {
          
			AudioSourceHelper.NotifySoundSettingChange (isOn);
        }else{
            MusicPlayer musicPlayer = MusicPlayer.instance;
            if(musicPlayer != null){
                musicPlayer.SetEnabled(isOn);
            }else{
                Debug.LogError("no music player");
            }
        }
	}
}
