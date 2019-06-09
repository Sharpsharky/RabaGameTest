using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectToggle : MonoBehaviour {

	[SerializeField]
	private GameObject[] _gameObjects;


	[SerializeField]
	private int _startIndex = 0;

	[SerializeField]
	private bool _setIndexOnStart = true;

	//public int gameObjectCount{ get { return _gameObjects.Length; } }

	private void Start(){

		if (!_setIndexOnStart) {
			return;
		}

		if (_startIndex < 0 || _startIndex >= _gameObjects.Length) {
			if (_gameObjects.Length > 0) {
				_startIndex = 0;
			}
		} 

		SetIndex (_startIndex);
	}

	public void SetIndex(int pIndex){
		if(pIndex < 0 || pIndex >= _gameObjects.Length){
			Debug.LogError ("GameObjectToggle: Supplied index (" + pIndex + ") out of range."); 
			return;
		}


		for (int i = 0; i < _gameObjects.Length; ++i) {
			_gameObjects [i].SetActive (i == pIndex);
		}
	}
}
