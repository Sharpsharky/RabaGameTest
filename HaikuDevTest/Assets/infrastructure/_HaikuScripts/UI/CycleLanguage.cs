using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CycleLanguage : MonoBehaviour, IPointerClickHandler{

	string _findName = "";

	[SerializeField]
	Sprite _usaFlag;

	[SerializeField]
	Sprite _japaneseFlag;

	[SerializeField]
	Sprite _germanFlag;

	[SerializeField]
	Sprite _spanishFlag;

	[SerializeField]
	Sprite _frenchFlag;

	[SerializeField]
	Sprite _italianFlag;

	[SerializeField]
	Sprite _portugueseFlag;

	[SerializeField]
	Sprite _koreanFlag;

	[SerializeField]
	Image _languageIcon;


	void Start () {
		RefreshFlag();
	}

	public void OnPointerClick(PointerEventData pEventData) {

	}

	private void RefreshFlag() {

	}

	private bool isName(string name) {
		
		return (name==_findName);
	}
}
