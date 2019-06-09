using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeImageLoop : MonoBehaviour {

    [SerializeField]
    float _startAlpha = 0f;

    [SerializeField]
    float _loopTime = 1f;

    Image _image;

    private void OnEnable() {
        if(_image == null){
            _image = gameObject.GetComponent<Image>();
        }
       
        StartCoroutine(DoAnimation());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    IEnumerator DoAnimation(){
        _image.CrossFadeAlpha(_startAlpha, 0f, false);

        float initialFadeInTime = _loopTime - (Mathf.InverseLerp(0f, 1f, _startAlpha) * _loopTime);

        if(_startAlpha < 1f){
            _image.CrossFadeAlpha(1f, initialFadeInTime, false);
            yield return new WaitForSeconds(initialFadeInTime);
        }

        WaitForSeconds waitTime = new WaitForSeconds(_loopTime);
        while(true){
            _image.CrossFadeAlpha(0f, _loopTime, false);
            yield return waitTime;

            _image.CrossFadeAlpha(1f, _loopTime, false);
            yield return waitTime;
        }
       
    }
	
}
