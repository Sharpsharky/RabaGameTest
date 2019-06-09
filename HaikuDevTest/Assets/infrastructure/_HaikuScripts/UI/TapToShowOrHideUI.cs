using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapToShowOrHideUI : MonoBehaviour, IPointerClickHandler{

	[SerializeField]
	GameObject _panelToShowOrHide;

	[SerializeField]
	bool _show;

	[SerializeField]
	AudioClip _sound;

	public void OnPointerClick(PointerEventData pEventData){

		if (_sound != null) {
			Helper.PlayAudioIfSoundOn (_sound);
		}

		_panelToShowOrHide.SetActive (_show);
	}
}
