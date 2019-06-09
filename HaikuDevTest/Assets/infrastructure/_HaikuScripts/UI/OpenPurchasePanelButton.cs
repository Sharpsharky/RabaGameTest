using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenPurchasePanelButton : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData pData){
        ChapterUIManager.instance.ShowPurchasePanel();
    }
}
