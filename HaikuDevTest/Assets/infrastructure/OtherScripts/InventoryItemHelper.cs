using UnityEngine;
using System.Collections;


public class InventoryItemHelper : MonoBehaviour {
	private GameObject itemSlot;
	public int itemSlotNum;
	public int itemID;
	public string sheet = "Sheet1";
	public string localizationKey;
	public int removeAfterUses = 0; // 0 Means never remove after use
	private int uses;
	public bool startWithItem = false;

	// Use this for initialization
	void Start () {
		PlayMakerFSM fsm = this.GetComponent<PlayMakerFSM> ();
		SetItemId();
		fsm.FsmVariables.GetFsmInt ("selfItemID").Value = itemID;
		FindItemSlot();
		fsm.FsmVariables.GetFsmGameObject ("inventorySlot").Value = itemSlot;

		LocalizeItemName();


		if (startWithItem) {
			StartCoroutine(LateStartWithItem(1));
		}

		//InventoryManager.instance.RegisterInventoryIcon (itemID, fsm);
	}

	/*void OnDestroy(){
		InventoryManager.instance.UnRegisterInventoryIcon (itemID);
	}*/

	void SetItemId() {
		string firstTwoChars = name.Substring(0, 2);

		if(!int.TryParse(firstTwoChars, out itemID))
		{
			// Set item ID to a two digit number. 
			// If it isn't two digits, just grab the first number.
			itemID = int.Parse(name[0].ToString());
		}
	}

	// We have a delay because we want this to occur after all Start functions. 
	// In Awake, our UI is laid out according to PlatformSpecifics. 
	// In Start(), objects are moved according to UIPositionMarker. 
	// The "start with item" event sets an object's position to the item slot. So we want to make sure the item slot has been
	// laid out in Awake() and Start(). 
	IEnumerator LateStartWithItem(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		GetComponent<PlayMakerFSM>().SendEvent("start with item");
	}


	private void FindItemSlot() {
		string itemSlotTag = string.Concat("itemSlot_", itemSlotNum.ToString());
		itemSlot = GameObject.FindGameObjectWithTag(itemSlotTag);
		if (itemSlot == null) {
			Debug.Log("Need to get itemslot for object: " + gameObject.name);
		}
	}

	void LocalizeItemName() {

	}

	void OnDisable() {

	}

	public void IncrementUses() {
		uses++;
		if (uses >= removeAfterUses) {
			GetComponent<PlayMakerFSM>().SendEvent("remove");
		}
		
	}
}
