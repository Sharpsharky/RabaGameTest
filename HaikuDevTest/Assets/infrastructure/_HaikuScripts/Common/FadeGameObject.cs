using UnityEngine;
using System.Collections;
using System;

public class FadeGameObject : MonoBehaviour {

	[Tooltip("The time in which the object will fade in.")]
	public float _fadeInTime = 1.5f;

	[Tooltip("The time in which the object will fade out.")]
	public float _fadeOutTime = 1.5f;

	SpriteRenderer[] _sprites;

	public Action FadeCompleteEvent;

    void Awake() {
        if (_sprites == null) {
            _sprites = GetComponentsInChildren<SpriteRenderer>();
        }
    }

	public void FadeOut() {
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", _fadeOutTime, "to", 0.0f,
			"time", 1f, "easetype", "linear","onupdate", "SetAlpha", "oncomplete", "FireFadeCompleteEvent", "oncompletetarget", this.gameObject));
	}

	public void FadeIn() {
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 0f, "to", 1f,
			"time", _fadeInTime, "easetype", "linear",
			"onupdate", "SetAlpha", "oncomplete", "FireFadeCompleteEvent", "oncompletetarget", this.gameObject));
	}

	public void SetAlpha(float newAlpha) {
        Color color;
        foreach(SpriteRenderer sprite in _sprites){
            color = sprite.color;
            color.a = newAlpha;
            sprite.color = color;
        }
	}

	void FireFadeCompleteEvent () {
		if (FadeCompleteEvent != null) {
			FadeCompleteEvent ();
		}
	}
}
