using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOutPanel : MonoBehaviour {

	public CanvasGroup canGroup = null;

	float requiredAlpha = 1.0f;
	float currentAlpha = 0.0f;
	float tweenDuration = 0.0f;

	private PlayMakerFSM _fsm;

	void Awake(){
		if (canGroup == null) {
			canGroup = this.GetComponent<CanvasGroup>();
		}

		_fsm = GetComponent<PlayMakerFSM>();
	}
		
	public void FadeOut(float duration)
	{
		SetAlphaOverDuration (0.0f, duration);
	}

	public void FadeIn(float duration)
	{
		SetAlphaOverDuration (1.0f, duration);
	}

	void SetAlphaOverDuration(float alpha, float duration)
	{
        StopAllCoroutines();
        
		if(canGroup == null)
			canGroup = this.GetComponent<CanvasGroup>();
		
		requiredAlpha = alpha;
		currentAlpha = canGroup.alpha;
		tweenDuration = duration;

		if (Mathf.Abs (requiredAlpha - currentAlpha) < 0.1f) {
			canGroup.alpha = requiredAlpha;
           
			SendComplete ();
		} else if (tweenDuration < 0.1f) {
			canGroup.alpha = requiredAlpha;
			SendComplete ();
		}
		else {
			if(requiredAlpha > currentAlpha)
				StartCoroutine ("fadeInCoroutine");
			else
				StartCoroutine ("fadeOutCoroutine");
			
		}
	}

	IEnumerator fadeInCoroutine()
	{
        //Debug.Log("start FadeIn " + canGroup.alpha);
		float dt = (tweenDuration / Time.deltaTime);
		int iterations = (int)dt;
		float delta = (requiredAlpha - currentAlpha) / iterations;
		for(int i=0;i<iterations;i++)
		{
			canGroup.alpha = canGroup.alpha + delta;

			yield return null;
		}

        //Debug.Log("finish FadeIn");
		SendComplete ();
	}

	private void SendComplete(){
		if (_fsm != null) {
			_fsm.SendEvent ("itween complete");
		}
	}

	IEnumerator fadeOutCoroutine()
	{
        //Debug.Log("start FadeOut " + canGroup.alpha);
		float dt = (tweenDuration / Time.deltaTime);
		int iterations = (int)dt;
		float delta = (requiredAlpha + currentAlpha) / iterations;
		for(int i=0;i<iterations;i++)
		{
			canGroup.alpha = canGroup.alpha - delta;
			yield return null;
		}
        //Debug.Log("finish FadeOut");
		SendComplete ();
	}
}
