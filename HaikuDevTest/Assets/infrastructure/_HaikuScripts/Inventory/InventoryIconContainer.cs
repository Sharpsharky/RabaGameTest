using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryIconContainer : MonoBehaviour {

	[SerializeField]
	InventoryIconSlot[] _slots;

	int? _forceSelectedItemId;

	public int selectedItemId{
		get{
			if (_selectedSlot != null) {
				return _selectedSlot.itemId;
			} else if (_forceSelectedItemId != null) {
				return (int)_forceSelectedItemId;
			}else {
				return 0;
			}
		}
	}

	InventoryIconSlot _selectedSlot;

	void OnEnable(){
		foreach (InventoryIconSlot slot in _slots) {
			slot.OnSelectInventoryItem += OnSelectSlot;
		}
	}

	void OnDisable(){
		foreach (InventoryIconSlot slot in _slots) {
			slot.OnSelectInventoryItem -= OnSelectSlot;
		}
	}

	public void UpdateItemCount(InventoryItemData pItemData, int pNewCount){
		int index = pItemData.slotNumber - 1;
		if (index < 0 || index >= _slots.Length) {
			Debug.LogError ("invalid slot " + pItemData.slotNumber);
			return;
		}

		_slots [index].UpdateItemCount (pItemData, pNewCount);
	}

	public void UpdateItemCount(int pItemId, int pNewCount){
		InventoryItemData data = InventoryManager.instance.GetItemData (pItemId);
		if (data != null) {
			UpdateItemCount (data, pItemId);
		}
	}

	public RectTransform GetItemImageRectTransform(int pSlotNumber){
		int index = pSlotNumber - 1;
		if (index < 0 || index >= _slots.Length) {
			Debug.LogError ("invalid slot " + pSlotNumber);
			return null;
		}

		return _slots [index].GetImageRectTransform ();
	}

	public void ForceSelectItemId(int pItemId){
		foreach (InventoryIconSlot slot in _slots) {
			if (slot.itemId == pItemId) {
				OnSelectSlot (slot, true);
				return;
			}
		}

        Debug.Log("Force selected " + pItemId);
		_forceSelectedItemId = pItemId;
	}

    public void OpenZoom(int pItemId){

        InventoryItemData data = InventoryManager.instance.GetItemData(pItemId);
        if(data == null || data.zoomObject == null){
            Debug.LogError("can't find zoom object for item " + pItemId);
            return;
        }

        foreach (InventoryIconSlot slot in _slots) {
            slot.ShowZoom(false);
        }

        RoomHelper currentRoom = ChapterSceneManager.instance.currentRoom;
        data.zoomObject.SetActive(true);
        data.zoomObject.transform.position = new Vector3(currentRoom.transform.position.x,
                                                         currentRoom.transform.position.y,
                                                         data.zoomObject.transform.position.z);

        data.zoomObject.transform.localScale = currentRoom.transform.localScale;
    }

    public void ClearSelected(){
        _forceSelectedItemId = null;
        _selectedSlot = null;

        foreach (InventoryIconSlot slot in _slots) {
               slot.SetSelected(false);
        }
    }

	void OnSelectSlot(InventoryIconSlot pSlot, bool pSelected){
		_forceSelectedItemId = null;

		if (pSelected) {
			_selectedSlot = pSlot;
			foreach (InventoryIconSlot slot in _slots) {
				if (slot != pSlot) {
					slot.SetSelected (false);
				}
			}
		} else {
			_selectedSlot = null;
		}

	}
}
