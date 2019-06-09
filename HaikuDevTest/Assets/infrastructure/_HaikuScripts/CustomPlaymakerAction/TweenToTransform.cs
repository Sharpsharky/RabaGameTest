using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions{
	
	[ActionCategory("_Common")]
	[Tooltip("Tweens a gameObject's scale, rotation, and position to match another gameObject")]
	public class TweenToTransform : FsmStateAction{

		public FsmOwnerDefault transformObject;
		public FsmGameObject destinationObject;

		public FsmFloat animationTime = 1f;

		public FsmEvent tweenCompleteEvent;

		public FsmBool isLocal = false;

		[ObjectType(typeof(iTween.EaseType))]
		public FsmEnum easeType;

	
		private float _elapsedTime;

		RectTransform _rectTransform;
		Vector2 _oldRectSize;
		Vector2 _newRectSize;

		public override void Reset(){

		}

		public override void OnEnter(){
			DoTweens ();
		}

		public override void OnUpdate(){
			_elapsedTime += Time.deltaTime;

			if (_rectTransform != null) {
				_rectTransform.sizeDelta = Vector2.Lerp (_oldRectSize, _newRectSize, _elapsedTime / animationTime.Value);
			}
			if (_elapsedTime >= animationTime.Value) {
				OnTweenComplete ();
			}
		}

		private void DoTweens(){
			_elapsedTime = 0;
          
            GameObject go = Fsm.GetOwnerDefaultTarget(transformObject);

			Vector3 position = isLocal.Value ? destinationObject.Value.transform.localPosition : destinationObject.Value.transform.position;
			Vector3 rotation = isLocal.Value ? destinationObject.Value.transform.localEulerAngles : destinationObject.Value.transform.eulerAngles;
			Vector3 scale = destinationObject.Value.transform.localScale;
			iTween.EaseType rawEaseType = (iTween.EaseType)easeType.Value;

            iTween.MoveTo(go,iTween.Hash("position",position,"time",animationTime.Value,"easetype",rawEaseType,"islocal",isLocal.Value));
            iTween.RotateTo(go,iTween.Hash("rotation",rotation,"time",animationTime.Value,"easetype",rawEaseType,"islocal",isLocal.Value));
            iTween.ScaleTo(go,iTween.Hash("scale",scale,"time",animationTime.Value,"easetype",rawEaseType));

			//TODO: RectTransform tweening wont work for all cases, this should be made more flexible to take into account anchors, offsets, etc
            _rectTransform = go.transform as RectTransform;
			if (_rectTransform != null) {
				RectTransform destTransform = destinationObject.Value.transform as RectTransform;
				if (destTransform != null) {
                    _oldRectSize = _rectTransform.rect.size;
                    _newRectSize = destTransform.rect.size;
				} else {
					_rectTransform = null;
				}
			}
		}



		private void OnTweenComplete(){
			Fsm.Event (tweenCompleteEvent);
			Finish ();
		}
	}

}
