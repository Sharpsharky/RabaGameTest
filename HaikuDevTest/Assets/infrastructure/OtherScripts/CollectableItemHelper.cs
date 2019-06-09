using UnityEngine;
using System.Collections;

public class CollectableItemHelper : MonoBehaviour {

	public GameObject InventoryToUnlock = null;
	public int majorHintId = 0;
	public int minorHintId = 0;

	// Use this for initialization
	void Start () {
		PlayMakerFSM fsm = this.GetComponent<PlayMakerFSM> ();
		fsm.FsmVariables.GetFsmGameObject ("InventoryItemToUnlock").Value = InventoryToUnlock;
		fsm.FsmVariables.GetFsmInt ("majorHintId").Value = majorHintId;
		fsm.FsmVariables.GetFsmInt ("minorHintId").Value = minorHintId;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
