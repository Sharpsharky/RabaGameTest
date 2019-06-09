using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class InventoryIconSlot : MonoBehaviour,  IPointerClickHandler{

	[SerializeField]
	Image _iconImage;

	[SerializeField]
	Image _selectedBackground;

	[SerializeField]
	TextMeshProUGUI _countText;

    [SerializeField]
    InventoryZoomIcon _zoomIcon;

    List<Image> _clonedImages;

    List<ItemCountData> _itemDatas;

    class ItemCountData{
        public InventoryItemData itemData;
        public int count;
        public ItemCountData(InventoryItemData pItemData, int pCount){
            itemData = pItemData;
            count = pCount;
        }
    }
	public int itemId{
		get{
            return (_itemDatas == null || _itemDatas.Count == 0) ? 0 : _itemDatas[_itemDatas.Count-1].itemData.itemId;
		}
	}

	public System.Action<InventoryIconSlot, bool> OnSelectInventoryItem;

	void Awake(){
		_iconImage.gameObject.SetActive (false);
		_selectedBackground.gameObject.SetActive (false);
		_countText.gameObject.SetActive (false);
        _itemDatas = new List<ItemCountData>();
        if(_zoomIcon != null){
            _zoomIcon.gameObject.SetActive(false);
        }
	}

	public void UpdateItemCount(InventoryItemData pItemData, int pNewCount){

        ItemCountData countData;
        int totalCount = 0;
        int foundIndex = -1;

        for (int i = 0; i < _itemDatas.Count; ++i){
            countData = _itemDatas[i];
            if(countData.itemData.itemId == pItemData.itemId){
                countData.count = pNewCount;
                foundIndex = i;
            }
            totalCount += countData.count;
        }

        if(foundIndex >= 0){
            if(pNewCount <= 0){
                _itemDatas.RemoveAt(foundIndex);
            }
        }else{
            if (pNewCount > 0) {
                _itemDatas.Add(new ItemCountData(pItemData, pNewCount));
                totalCount += pNewCount;
            }
        }

        if(totalCount <= 0){
            _selectedBackground.gameObject.SetActive(false);
            _countText.gameObject.SetActive(false);
        }else{
            _selectedBackground.gameObject.SetActive(false);
            _countText.gameObject.SetActive(totalCount > 1);
            if (totalCount > 1) {
                _countText.text = "x" + totalCount;
            }
            _iconImage.gameObject.SetActive(true);
        }
        RefreshImages();

        if(_zoomIcon != null){
            if(_itemDatas.Count == 0){
                _zoomIcon.gameObject.SetActive(false);
            }else{
                GameObject zoomObject = _itemDatas[_itemDatas.Count - 1].itemData.zoomObject;
                if (zoomObject != null) {
                    _zoomIcon.gameObject.SetActive(true);
                    _zoomIcon.SetZoomObject(zoomObject);
                } else {
                    _zoomIcon.gameObject.SetActive(false);
                }
            }
           
        }
	}

    public void ShowZoom(bool pShow){
        if(_zoomIcon != null){
            _zoomIcon.ShowZoom(pShow);
        }
    }

    void RefreshImages(){

        _iconImage.gameObject.SetActive(_itemDatas.Count > 0);

        int numRequiredImages = _itemDatas.Count;

        int numExtraRequiredImages = numRequiredImages - 1;

        if(numExtraRequiredImages > 0){
            if(_clonedImages == null){
                _clonedImages = new List<Image>();
            }

            if(_clonedImages.Count < numExtraRequiredImages){
                while (_clonedImages.Count < numExtraRequiredImages) {
                    GameObject imageObj = GameObject.Instantiate(_iconImage.gameObject, _iconImage.transform.parent);
                    _clonedImages.Add(imageObj.GetComponent<Image>());
                }
            }else if(_clonedImages.Count > numExtraRequiredImages){
                while (_clonedImages.Count > numExtraRequiredImages) {
                    GameObject.Destroy(_clonedImages[0].gameObject);
                    _clonedImages.RemoveAt(0);
                }
            }
        }else if(_clonedImages != null && _clonedImages.Count > 0){
            while (_clonedImages.Count > 0) {
                GameObject.Destroy(_clonedImages[0].gameObject);
                _clonedImages.RemoveAt(0);
            }
        }

      
        for (int i = 0; i < _itemDatas.Count; ++i){
            if(i == 0){
                _iconImage.sprite =_itemDatas[i].itemData.sprite;
            }else{
                _clonedImages[i - 1].sprite = _itemDatas[i].itemData.sprite;
            }
        }
    }

	public RectTransform GetImageRectTransform(){
        return _iconImage.GetComponent<RectTransform>();
	}

	public void SetSelected(bool pSelected){
		_selectedBackground.gameObject.SetActive (pSelected);

	}

    InventoryItemData GetTopMostItemData(){
        if(_itemDatas.Count > 0){
            return _itemDatas[_itemDatas.Count - 1].itemData;
        }
      

        return null;
    }

	public void OnPointerClick(PointerEventData pEventData){
		bool selected = !_selectedBackground.gameObject.activeSelf;
		SetSelected (selected);
        InventoryItemData itemData = GetTopMostItemData();

		if (selected && itemData != null) {
			Helper.LocalizeKeyToTopBar (itemData.localizationKey,"Sheet1",false,true);
		}

		if (OnSelectInventoryItem != null) {
			OnSelectInventoryItem (this,selected);
		}
	}
}
