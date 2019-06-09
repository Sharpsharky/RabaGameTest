using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{

	//To fade a spriterenderer, one could use the existing iTween.FadeTo action. However this is problematic
	//because iTween.FadeTo sets the material property , not the sprite color which means you can't set the
	//starting alpha in the inspector. If you set the sprite color to alpha 0, an iTween fade in will not show up,
	//if you set it to 1, the sprite will be visible the whole time. So in order to fade in a sprite in you need one
	//action to iTween.Fade to 0, and another to iTween.Fade to 1. This class simplifies sprite fading by
	//using the sprite color so that one can easily set the starting alpha either in inspector or
	//in an fsm

	[ActionCategory("_Common")]
	[Tooltip("Fades to spriteRenderer to a given alpha")]
	public class SpriteRendererFade : FsmStateAction{

		public FsmFloat fadeTime = 1f;

		public FsmFloat goalAlpha = 1f;

		[CheckForComponent(typeof(SpriteRenderer))]
		[Tooltip("The GameObject with a SpriteRenderer attached.")]
		public FsmOwnerDefault spriteRendererGameObject;

		public FsmEvent finishedEvent;


		private float _elapsedTime;

		private float _startAlpha;

		private SpriteRenderer _spriteRenderer;


		public override void Reset(){
			fadeTime = 1f;
			goalAlpha = 1f;
			spriteRendererGameObject = null;
		}

		public override void OnEnter(){
			_elapsedTime = 0;
			_spriteRenderer = spriteRendererGameObject.GameObject.Value.GetComponent<SpriteRenderer> ();
			_startAlpha = _spriteRenderer.color.a;
		}

		public override void OnUpdate (){
			_elapsedTime += Time.deltaTime;
			float t = Mathf.InverseLerp (0, fadeTime.Value, _elapsedTime);
		
			Color color = _spriteRenderer.color;
			color.a = Mathf.Lerp (_startAlpha, goalAlpha.Value, t);

			_spriteRenderer.color = color;

			if (_elapsedTime >= fadeTime.Value) {
				Fsm.Event (finishedEvent);
				Finish ();
			}
		}
	}
}
