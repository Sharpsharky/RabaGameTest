using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryZoomIcon : MonoBehaviour, IPointerClickHandler{
	

    GameObject _zoomedInObject;
	ChapterSceneManager _sceneManager;
    static List<InventoryZoomIcon> s_allInstances;

    void Awake(){
        if(s_allInstances == null){
            s_allInstances = new List<InventoryZoomIcon>();
        }

        s_allInstances.Add(this);
    }

    void OnDestroy(){
        if (s_allInstances != null) {
            s_allInstances.Remove(this);
        }
    }	

    void Start() {
        _sceneManager = ChapterSceneManager.instance;
	}


    public void SetZoomObject(GameObject pObject){
        _zoomedInObject = pObject;
        _zoomedInObject.gameObject.SetActive(false);
    }

    public void ShowZoom(bool pShow){

        if(_zoomedInObject == null){
            return;
        }

        if (!pShow) {
            _zoomedInObject.SetActive(false);
        } else {
            _zoomedInObject.SetActive(true);
            _zoomedInObject.transform.position = new Vector3(_sceneManager.currentRoom.transform.position.x,
                                                            _sceneManager.currentRoom.transform.position.y,
                                                            _zoomedInObject.transform.position.z);

            _zoomedInObject.transform.localScale = _sceneManager.currentRoom.transform.localScale;

            InventoryZoomIcon otherIcon;
            for (int i = 0; i < s_allInstances.Count; i++) {
                otherIcon = s_allInstances[i];
                if (otherIcon != this && otherIcon != null && otherIcon._zoomedInObject != null) {
                    otherIcon._zoomedInObject.SetActive(false);
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData pEventData) {
        if(_zoomedInObject == null){
            return;
        }

        ShowZoom(!_zoomedInObject.activeSelf);
	}

}
