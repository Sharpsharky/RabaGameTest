using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class TapToAddPieceSaveable {
	[SerializeField]
	private int[] selectedItems;

	[SerializeField]
	private bool shouldBeEmpty;

	[SerializeField]
	private int[] correctItemIDs;

	[SerializeField]
	private GameObject[] itemGameobjects;

	[SerializeField]
	private GameObject[] spriteGameobjects;

	// If an item is here already, set hasItem as true and activate the item in the Hierarchy view
	public bool hasItem; 
	public int itemIndex;
	public bool puzzleWasWon;

	[SerializeField]
	private TapToAddPieceManagerSaveable manager;

//	[SerializeField]
//	private bool resetColliderOnSpriteSwitch;
	public AudioClip pieceAddedSound;

	// Todo; unique per instance of script
	private const string kHasItemTag = "kHasItemTag";
	private const string kIndexTag = "kIndexTag";
	private const string kPuzzleWon = "kPuzzleWon";



	protected  void SaveScript(string fileName) {

	}

	protected  void LoadScript(string fileName) {

	}

	public bool isCorrect
	{
		get { 
			if (hasItem) {
				if (shouldBeEmpty) return false;
				for (int i = 0; i < correctItemIDs.Length; i++) {					
					if (correctItemIDs[i] == selectedItems[itemIndex]) {
//						Debug.Log("Correct item: " + correctItemIDs[i] + " selectedItems[Index: " + selectedItems[itemIndex]);
						return true;
					}
				}
				return false;
			} else {
				return shouldBeEmpty;
			}
		}
	}

	// Call this function from the sprite game object's if you want to use their colliders
	// For example, set up a Playmaker FSM on the sprite game object and have them call this function when tapped
	// Useful for when the collider will be different shapes for different sprites
	public void SpriteGameobjectTapped() {
		OnMouseDown();
	}

	void OnMouseDown() {
		if (puzzleWasWon) return;

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
				itemIndex = i;
				Debug.Log("Index is : " + i);
				isMatch = true;
				break;
			}
		}
		if (isMatch) {
			Helper.PlayAudioIfSoundOn(pieceAddedSound);

			itemGameobjects[itemIndex].GetComponent<PlayMakerFSM>().SendEvent("decrement");
			spriteGameobjects[itemIndex].SetActive(true);
			hasItem = true;
			manager.CheckIfAllCorrect();

//			if (resetColliderOnSpriteSwitch) {
//				StartCoroutine(ResetColliderToNewSprite());
//			}
		} else {
			Debug.Log("You didn't select a possible item");
		}
	}


	private void RemoveItem() {
		spriteGameobjects[itemIndex].SetActive(false);
		itemGameobjects[itemIndex].GetComponent<PlayMakerFSM>().SendEvent("increment");
		hasItem = false;
//		if (resetColliderOnSpriteSwitch) {
//			// Slight delay so the tap does not go through the original first collider 
//			Invoke("RestoreDefaultCollider", 0.5f);
//		}
	}

//	private void RestoreDefaultCollider() {
//	}
//
//	// It is not possible to reset the collider like in the Editor, so destroying and restoring is the easiest way
//	// https://forum.unity3d.com/threads/components-reset-from-script-in-c.109429/
//	IEnumerator ResetColliderToNewSprite()
//	{
//		Destroy(this.gameObject.GetComponent<BoxCollider2D>());
//		yield return new WaitForSeconds(0.1f);
//		this.gameObject.AddComponent<BoxCollider2D>();
//	}
//
}
