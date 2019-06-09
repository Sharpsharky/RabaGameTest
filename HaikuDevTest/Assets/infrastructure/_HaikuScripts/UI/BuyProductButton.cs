using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class BuyProductButton : MonoBehaviour, IPointerClickHandler{
    
    [SerializeField]
    IAPConstants.IAPProduct _product;

    [SerializeField]
    TextMeshProUGUI _priceText;

    public IAPConstants.IAPProduct product {
        get {
            return _product;
        }
    }

    public Action<BuyProductButton> OnClick;

	
	public void OnPointerClick(PointerEventData pEventData) {
        if(OnClick != null){
            OnClick(this);
        }
	}

    public void SetPrice(string pPrice){
        Debug.Log("Set Price " + pPrice);
        _priceText.text = pPrice;
    }
}
