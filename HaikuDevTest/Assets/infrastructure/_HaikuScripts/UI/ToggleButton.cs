using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class ToggleButton : MonoBehaviour, IPointerClickHandler{

	[Header("Toggle Buttons can swap between 2 images, swap between 2 sprites on a single image, or both")] 

	[SerializeField]
	Image _onImage;

	[SerializeField]
	Image _offImage;

	[SerializeField]
	Image _spriteSwapImage;

	[SerializeField]
	Sprite _onSprite;

	[SerializeField]
	Sprite _offSprite;

	[SerializeField]
	TextMeshProUGUI _text;

	[SerializeField]
	string _onTextLocKey;

	[SerializeField]
	string _offTextLocKey;

	[SerializeField]
	AudioClip _tapSound;

    [SerializeField]
    bool _startsOn = false;

	bool _isOn;

    public bool isOn {
        get {
            return _isOn;
        }
    }

    public System.Action<ToggleButton, bool> OnToggle;

	void Awake(){
        SetOn (_startsOn);
	}

	void OnEnable(){
	
	}

	void OnDisable(){
	
	}

	void Refresh(){
		SetOn (_isOn);
	}

	public void SetOn(bool pIsOn){
		_isOn = pIsOn;
		if (_onImage != null) {
			_onImage.gameObject.SetActive (_isOn);
		}

		if (_offImage != null) {
			_offImage.gameObject.SetActive (!_isOn);
		}

		if (_spriteSwapImage != null) {
            Sprite sprite = _isOn ? _onSprite : _offSprite;
            Color color = _spriteSwapImage.color;
            if(sprite != null){
                color.a = 1f;
                _spriteSwapImage.color = color;
                _spriteSwapImage.sprite = sprite;
            }else{
                color.a = 0f;
                _spriteSwapImage.color = color;
            }
			
		}

		if (_text != null && _onTextLocKey != null && _offTextLocKey != null) {
			_text.text = Helper.LocalizeText (_isOn ? _onTextLocKey : _offTextLocKey);
		}
	}

	public void OnPointerClick(PointerEventData pEventData){
		SetOn (!_isOn);

		if (OnToggle != null) {
			OnToggle (this, _isOn);
		}

		if (_tapSound != null) {
			Helper.PlayAudioIfSoundOn (_tapSound);
		}
	}
}
