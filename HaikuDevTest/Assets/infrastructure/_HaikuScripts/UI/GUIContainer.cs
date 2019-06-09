using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class GUIContainer : MonoBehaviour {

	[SerializeField]
	InventoryIconContainer _inventoryContainer;
	public InventoryIconContainer inventoryContainer {
		get {
			return _inventoryContainer;
		}
	}

	[SerializeField]
	StarCountDisplay _starCountDisplay;
    public StarCountDisplay starCountDisplay {
		get {
            return _starCountDisplay;
		}
	}

    [SerializeField]
    Image _hintNotifier;
    public Image hintNotifier {
        get {
            return _hintNotifier;
        }
    }

    RectTransform _rectTransform;
	public RectTransform rectTransform {
		get {
			return _rectTransform;
		}
	}

	void Awake(){
		_rectTransform = GetComponent<RectTransform> ();
	}


}
