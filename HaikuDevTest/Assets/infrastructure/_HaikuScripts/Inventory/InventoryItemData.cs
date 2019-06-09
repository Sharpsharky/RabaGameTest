using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData{

	[SerializeField]
	int _itemId;
	public int itemId {
		get {
			return _itemId;
		}
	}

	[SerializeField]
	Sprite _sprite;
	public Sprite sprite {
		get {
			return _sprite;
		}
	}

	[SerializeField]
	string _localizationKey;
	public string localizationKey {
		get {
			return _localizationKey;
		}
	}

	[SerializeField]
	int _slotNumber;
	public int slotNumber {
		get {
			return _slotNumber;
		}
	}

	[SerializeField]
	int _removeAfterUses;
	public int removeAfterUses {
		get {
			return _removeAfterUses;
		}
	}

	[SerializeField]
	int _startCount;
	public int startCount {
		get {
			return _startCount;
		}
	}

    [SerializeField]
    GameObject _zoomObject;
    public GameObject zoomObject {
        get {
            return _zoomObject;
        }
    }


    /*[SerializeField]
	string _localizationSheet = "Sheet1";
	public string localizationSheet {
		get {
			return _localizationSheet;
		}
	}*/
}
