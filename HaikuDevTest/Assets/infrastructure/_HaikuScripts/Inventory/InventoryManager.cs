using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

	const string SAVE_ITEMS_KEY = "items";
    const string GRANTED_START_ITEMS_KEY = "gsi";

	private static InventoryManager s_instance;

	[SerializeField]
	InventoryItemData[] _itemDatas;

    [SerializeField, HideInInspector]
    bool _grantedStartItems;

	Dictionary<int,InventoryInstanceData> _itemInstanceDatas;



	public static InventoryManager instance{
		get{
			return s_instance;
		}
	}

	protected  void Awake(){
        foreach (InventoryItemData itemData in _itemDatas) {
            if (itemData.zoomObject != null) {
                itemData.zoomObject.gameObject.SetActive(false);
            }
        }


		s_instance = this;
	}

	public int GetItemCount(int pId){

		if (_itemInstanceDatas == null) {
			return 0;
		}

		InventoryInstanceData instanceData = null;
		if (_itemInstanceDatas.TryGetValue (pId, out instanceData)) {
			return instanceData.count;
		} else {
			return 0;
		}

	}

	public bool HasItem(int pId){
		return GetItemCount (pId) > 0;
	}

    public void UseItem(int pId){
        InventoryItemData itemData = GetItemData(pId);

        if (itemData == null) {
            Debug.LogError("No data found for item id " + pId);
            return;
        }

        if (_itemInstanceDatas == null) {
            return;
        } else {
            InventoryInstanceData instanceData = null;
            if (_itemInstanceDatas.TryGetValue(pId, out instanceData)) {
                if(instanceData.count <= 0){
                    return;
                }

                ++instanceData.useCount;
                if(instanceData.useCount >= itemData.removeAfterUses){
                    instanceData.useCount = 0;
                    RemoveItem(pId);
                }
            }
        }
    }

	public void AddItem(int pId, int pCount = 1){
		if (pCount < 1) {
			Debug.LogError ("Can't add less than 1 item");
			return;
		}

		InventoryItemData itemData = GetItemData (pId);

		if (itemData == null) {
			Debug.LogError ("No data found for item id " + pId);
			return;
		}

		ChapterUIManager chapterUIManager = ChapterUIManager.instance;

		if (_itemInstanceDatas == null) {
			_itemInstanceDatas = new Dictionary<int,InventoryInstanceData> ();
            _itemInstanceDatas.Add (pId, new InventoryInstanceData(pId,pCount,0));
			chapterUIManager.PlayAddInventoryItemAnimation (itemData, pCount);
		} else {
			InventoryInstanceData instanceData = null;
			if (_itemInstanceDatas.TryGetValue (pId, out instanceData)) {
				instanceData.count += pCount;
				chapterUIManager.UpdateInventoryItemCount (itemData, instanceData.count,true);
			} else {
				_itemInstanceDatas.Add (pId, new InventoryInstanceData(pId,pCount,0));
				chapterUIManager.PlayAddInventoryItemAnimation (itemData, pCount);
			}
		}


	}

	public void RemoveItem(int pId, int pCount = 1) {
		if (pCount < 1) {
			Debug.LogError ("Can't remove less than 1 item");
			return;
		}

		InventoryItemData itemData = GetItemData (pId);

		if (itemData == null) {
			Debug.LogError ("No data found for item id " + pId);
			return;
		}

		if (_itemInstanceDatas == null) {
			return;
		} else {
			InventoryInstanceData instanceData = null;
			if (_itemInstanceDatas.TryGetValue (pId, out instanceData)) {
				ChapterUIManager chapterUIManager = ChapterUIManager.instance;
				instanceData.count -= pCount;
				chapterUIManager.UpdateInventoryItemCount (itemData, instanceData.count);

                if(instanceData.count <= 0){
                    chapterUIManager.ClearSelectedItem();
                }


                //we dont delete the item if count is 0 because the presence of the item
                //indicates that the animation has already played
			}
		}

	}

	public InventoryItemData GetItemData(int pId){
		foreach (InventoryItemData data in _itemDatas) {
			if (data.itemId == pId) {
				return data;
			}
		}

		return null;
	}

	protected  void SaveScript(string fileName){

	}

    protected  void InitializeScript(string fileName) {

        GrantStartItemsAndUpdateUI();
    }

    void GrantStartItemsAndUpdateUI(){
        if(_itemInstanceDatas == null){
            _itemInstanceDatas = new Dictionary<int, InventoryInstanceData>();
        }

        InventoryInstanceData instanceData;
        ChapterUIManager chapterUIManager = ChapterUIManager.instance;

        foreach (InventoryItemData itemData in _itemDatas) {
            if (_itemInstanceDatas.TryGetValue(itemData.itemId, out instanceData)) {
                if(!_grantedStartItems && itemData.startCount > 0){
                    instanceData.count += itemData.startCount;
                }
                chapterUIManager.UpdateInventoryItemCount(itemData, instanceData.count);
            } else {
                if (!_grantedStartItems && itemData.startCount > 0) {
                    instanceData = new InventoryInstanceData(itemData.itemId, itemData.startCount, 0);
                    _itemInstanceDatas.Add(itemData.itemId, instanceData);
                    chapterUIManager.UpdateInventoryItemCount(itemData, instanceData.count);
                }

            }
        }

        _grantedStartItems = true;

    }

    protected  void LoadScript(string fileName){
       
	}
}
