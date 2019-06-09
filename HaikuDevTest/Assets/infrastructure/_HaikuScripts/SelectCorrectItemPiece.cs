using UnityEngine;
using System.Collections;

public class SelectCorrectItemPiece : MonoBehaviour {
	public bool isCorrect;

	private bool _isSelected;

	private Vector3 originalScale;
	private Vector3 selectedScale;

	public bool isSelected {
		get { return _isSelected; }
		set {
			_isSelected = value;
			if (_isSelected) {
				transform.localScale = selectedScale;
			} else {
				transform.localScale = originalScale;
			}
		}
	}

	// Use this for initialization
	void Start () {
		originalScale = transform.localScale;
		selectedScale = new Vector3(originalScale.x * 1.2f, originalScale.y * 1.2f, 1.0f);
	}
}
