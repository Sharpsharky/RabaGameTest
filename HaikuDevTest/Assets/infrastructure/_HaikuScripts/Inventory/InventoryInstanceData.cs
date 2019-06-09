using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryInstanceData{
	public int itemId;
	public int count;
	public int useCount;
  
	public InventoryInstanceData(){}

	public InventoryInstanceData(int pItemId, int pCount, int pUseCount){
		itemId = pItemId;
		count = pCount;
		useCount = pUseCount;
	}
}
