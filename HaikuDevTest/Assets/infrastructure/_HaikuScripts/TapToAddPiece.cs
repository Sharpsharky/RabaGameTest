using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class TapToAddPiece : MonoBehaviour {
	[SerializeField]
	private int[] selectedItems;

	[SerializeField]
	private int correctItemID;

	[SerializeField]
	private GameObject[] itemGameobjects;

	[SerializeField]
	private Sprite[] spritesToShow;

	private bool hasItem;
	private int index;

	[SerializeField]
	private TapToAddPieceManager manager;

	[SerializeField]
	private bool resetColliderOnSpriteSwitch;

	public AudioClip pieceAddedSound;

	// Cache the default box collider
	private Vector2 cachedColliderSize;
	private Vector2 cachedColliderOffset;

	void Start() {
		cachedColliderSize = GetComponent<BoxCollider2D>().size;
		cachedColliderOffset = GetComponent<BoxCollider2D>().offset;
	}

	public bool isCorrect
	{
		get { 
			if (hasItem) {
				return (correctItemID==selectedItems[index]); 
			} else {
				return false;
			}
		}
	}

	void OnMouseDown() {
		if (!hasItem) {
			TryAddItem();
		} else {
			RemoveItem();
		}
	}

	private void TryAddItem() {
		int selectedItem = FsmVariables.GlobalVariables.GetFsmInt("selectedItem").Value;
		Debug.Log("Selected item is: " + selectedItem);
		bool isMatch = false;
		for (int i = 0; i < selectedItems.Length; i++) {
			if (selectedItems[i] == selectedItem) {
				index = i;
				Debug.Log("Index is : " + i);
				isMatch = true;
				break;
			}
		}
		if (isMatch) {
			Helper.PlayAudioIfSoundOn(pieceAddedSound);

			itemGameobjects[index].GetComponent<PlayMakerFSM>().SendEvent("decrement");
			GetComponent<SpriteRenderer>().sprite = spritesToShow[index];
			hasItem = true;
			if (selectedItem==correctItemID) {
				if (manager != null) {
					manager.CheckIfAllCorrect();
				}
			}

			if (resetColliderOnSpriteSwitch) {
				StartCoroutine(ResetColliderToNewSprite());
			}
		} else {
			Debug.Log("You didn't select a possible item");
		}
	}

	private void RemoveItem() {
		itemGameobjects[index].GetComponent<PlayMakerFSM>().SendEvent("increment");
		GetComponent<SpriteRenderer>().sprite = null;
		hasItem = false;
		if (resetColliderOnSpriteSwitch) {
			// Slight delay so the tap does not go through the original first collider 
			Invoke("RestoreDefaultCollider", 0.5f);
		}
	}

	private void RestoreDefaultCollider() {
		GetComponent<BoxCollider2D>().size = cachedColliderSize;
		GetComponent<BoxCollider2D>().offset = cachedColliderOffset;
	}

	// It is not possible to reset the collider like in the Editor, so destroying and restoring is the easiest way
	// https://forum.unity3d.com/threads/components-reset-from-script-in-c.109429/
	IEnumerator ResetColliderToNewSprite()
	{
		Destroy(this.gameObject.GetComponent<BoxCollider2D>());
		yield return new WaitForSeconds(0.1f);
		this.gameObject.AddComponent<BoxCollider2D>();
	}

}
