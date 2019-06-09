using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public enum KateMakeup {
	none, gothic, yellow, tone, pink, normal
};

public enum KateAccessory {
	none, boa, ears, flower, glasses, bag
};

public enum KateDress {
	none, yellow, blue, red, black, green
};

public enum KateShoe {
	none, black, red, silver, pink, green
};

public class DressUpManager : MonoBehaviour {

	private KateMakeup selectedMakeup = KateMakeup.none;
	private KateAccessory selectedAccessory = KateAccessory.none;
	private KateDress selectedDress = KateDress.none;
	private KateShoe selectedShoe = KateShoe.none; 

	private KateMakeup correctMakeup = KateMakeup.normal;
	private KateDress correctDress = KateDress.red;
	private KateShoe correctShoe = KateShoe.silver; 
	private KateAccessory correctAccessory = KateAccessory.boa;

	public GameObject overlay = null;
	public PlayMakerFSM kateFSM = null;
	public TextMeshPro bubbleText = null;
	public PlayMakerFSM bubbleFSM = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void checkCorrectItems() {
		if (this.selectedMakeup != KateMakeup.none && this.selectedDress != KateDress.none &&
			this.selectedShoe != KateShoe.none && this.selectedAccessory != KateAccessory.none) {
				this.overlay.SetActive(true);

				int correctItems = 0;
				if (this.selectedMakeup == this.correctMakeup) { correctItems++; }
				if (this.selectedDress == this.correctDress) { correctItems++; }
				if (this.selectedShoe == this.correctShoe) { correctItems++; }
				if (this.selectedAccessory == this.correctAccessory) { correctItems++; }

				string speech = "Yes, I think this is the correct outfit!";
				if (correctItems == 4) { // Win game
					this.kateFSM.SendEvent("wonDressPuzzle");
				} else {
					string parts = (correctItems == 1) ? "part" : "parts";
					speech = "I like " + correctItems + " " + parts + " of this outfit.";
					this.kateFSM.SendEvent("resetDressPuzzle");

					this.selectedMakeup = KateMakeup.none;
					this.selectedAccessory = KateAccessory.none;
					this.selectedDress = KateDress.none;
					this.selectedShoe = KateShoe.none; 
				}
				this.bubbleText.text = speech;
				this.bubbleFSM.SendEvent("activate");
		}
	}

	#region Public methods
	public void setMakeup(KateMakeup makeup) {
		this.selectedMakeup = makeup;
		checkCorrectItems();
	}

	public void setAccessory(KateAccessory accessory) {
		this.selectedAccessory = accessory;
		checkCorrectItems();
	}

	public void setDress(KateDress dress) {
		this.selectedDress = dress;
		checkCorrectItems();
	}

	public void setShoe(KateShoe shoe) {
		this.selectedShoe = shoe;
		checkCorrectItems();
	}
	#endregion
}

