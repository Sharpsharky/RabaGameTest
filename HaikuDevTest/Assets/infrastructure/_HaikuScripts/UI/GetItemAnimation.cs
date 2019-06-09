using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayMakerFSM))]
public class GetItemAnimation : MonoBehaviour {

	PlayMakerFSM _fsm;
	InventoryItemData _itemData;
	int _newCount;

	void Awake(){
		_fsm = GetComponent<PlayMakerFSM> ();
	}


    public void Activate(InventoryItemData pItemData, int pNewCount,RectTransform pDestinationRect,AudioClip pSoundEffect){
		_newCount = pNewCount;
		_itemData = pItemData;

		Helper.LocalizeKeyToTopBar (pItemData.localizationKey);

		HutongGames.PlayMaker.Actions.SetEventProperties.properties = new Dictionary<string, object>{
			{"sprite", _itemData.sprite},
            {"sound",pSoundEffect},
			{"destinationIcon",pDestinationRect.gameObject},
		};

		_fsm.SendEvent ("activate");
	}

	public void UpdateUIIcon(){
		ChapterUIManager.instance.UpdateInventoryItemCount (_itemData, _newCount);
	}
}
