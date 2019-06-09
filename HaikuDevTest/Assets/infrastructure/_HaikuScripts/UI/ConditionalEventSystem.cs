using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalEventSystem : MonoBehaviour {

    [SerializeField]
    GameObject _fallbackEventSystemGameObject;

    void Awake() {
        if(UnityEngine.EventSystems.EventSystem.current == null){
            if(_fallbackEventSystemGameObject != null){
                _fallbackEventSystemGameObject.SetActive(true);
            }

        }
    }
}
