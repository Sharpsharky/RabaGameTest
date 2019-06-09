using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapToSendMessage : MonoBehaviour, IPointerClickHandler{

    [SerializeField]
    GameObject _messageReceiver;

    [SerializeField]
    string _message;

    public void OnPointerClick(PointerEventData pPointerEventData){
        if(_messageReceiver != null && _message != null){
            _messageReceiver.SendMessage(_message);
        }
    }
}
