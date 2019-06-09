using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))]
public class SceneFade : MonoBehaviour {

	[SerializeField]
	private float _fadeTime = 1f;

	private Collider2D _collider;

	private SpriteRenderer _spriteRenderer;

	private static SceneFade s_instance;

	private const string NO_INSTANCE_ERROR = "No Instance of SceneFade in Scene";

	private Action _callback;

	private bool _isFadingOut;

	private void Awake(){
		_collider = GetComponent<Collider2D> ();
		_spriteRenderer = GetComponent<SpriteRenderer> ();
		_spriteRenderer.enabled = true;
		s_instance = this;



		OnUpdateAlpha (0f);
		_collider.enabled = false;
	}
		
	public static void FadeOutStatic(Action pCallback = null){
		if (s_instance == null) {
			Debug.LogError (NO_INSTANCE_ERROR);
		} else {
			s_instance.FadeOut (pCallback);
		}
	}

	public static void FadeInStatic(Action pCallback = null){
		if (s_instance == null) {
			Debug.LogError (NO_INSTANCE_ERROR);
		} else {
			s_instance.FadeIn(pCallback);
		}
	}
		
	//can't do FadeIn(Action pCallback = null) because then the method can't be called from playmaker
	public void FadeIn(){
		FadeIn (null);
	}

	public void FadeIn(Action pCallback){
		_isFadingOut = false;
		_collider.enabled = true;
		_callback = pCallback;

		iTween.ValueTo(gameObject,iTween.Hash("from",1f, "to", 0f, "time",_fadeTime,
			"onupdate", "OnUpdateAlpha","oncomplete", "OnFadeComplete"));
	}

	//can't do FadeOut(Action pCallback = null) because then the method can't be called from playmaker
	public void FadeOut(){
		FadeOut (null);
	}

	public void FadeOut(Action pCallback){
		_isFadingOut = true;
		_collider.enabled = true;
		_callback = pCallback;

		iTween.ValueTo(gameObject,iTween.Hash("from",0f, "to", 1f, "time",_fadeTime,
			"onupdate", "OnUpdateAlpha","oncomplete", "OnFadeComplete"));
	}

	private void OnUpdateAlpha(float pAlpha){
		Color color = _spriteRenderer.color;
		color.a = pAlpha;
		_spriteRenderer.color = color;
	}

	private void OnFadeComplete(){
		if (!_isFadingOut) {
			_collider.enabled = false;
		}
			
		if (_callback != null) {
			_callback ();
		}
	}
}
