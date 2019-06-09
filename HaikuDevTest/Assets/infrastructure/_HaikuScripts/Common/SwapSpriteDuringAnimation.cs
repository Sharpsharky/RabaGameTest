using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * For some reason changing sprites on the timeline is not working 
 * This class is a workaround for changing sprites during an animation
 */

[RequireComponent(typeof(Animation)), RequireComponent(typeof(SpriteRenderer))]
public class SwapSpriteDuringAnimation : MonoBehaviour {

	SpriteRenderer _spriteRenderer;

	void Awake(){
		_spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void SetSprite(Sprite pSprite){
		_spriteRenderer.sprite = pSprite;
	}
}
