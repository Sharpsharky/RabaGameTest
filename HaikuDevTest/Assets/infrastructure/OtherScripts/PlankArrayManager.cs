using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
using HutongGames.Utility;

public class PlankArrayManager : MonoBehaviour {
	public ArrayList weightSlots = new ArrayList();
	public Stack slabs = new Stack();
	private Dictionary<GameObject, GameObject> childToParent = new Dictionary<GameObject, GameObject>();

	int currentEmptyIndex = 0;
	int maxIndex;
	int totalWeight = 0;
	int targetWeight = 2132;
	GameObject plank;

	// Use this for initialization
	void Start () {
		plank = this.transform.parent.gameObject; // Plank is the parent of the target area

		weightSlots.Add (GameObject.Find ("1_weight_slot"));
		weightSlots.Add (GameObject.Find ("2_weight_slot"));
		weightSlots.Add (GameObject.Find ("3_weight_slot"));
		weightSlots.Add (GameObject.Find ("4_weight_slot"));
		weightSlots.Add (GameObject.Find ("5_weight_slot"));
		weightSlots.Add (GameObject.Find ("6_weight_slot"));
		weightSlots.Add (GameObject.Find ("7_weight_slot"));
		weightSlots.Add (GameObject.Find ("8_weight_slot"));
		maxIndex = weightSlots.Count;
		WeightCompare ();
	}

	void AddWeightToPlank(GameObject slab) {
		if (currentEmptyIndex >= maxIndex) {
			PlayMakerFSM slabFsm = slab.GetComponent<PlayMakerFSM>();
			slabFsm.SendEvent("snap back");
		} else {
			GameObject nextEmpty = (GameObject) weightSlots[currentEmptyIndex];
			Vector3 slotPosition = nextEmpty.transform.position;
			iTween.MoveTo(slab,iTween.Hash("position", slotPosition, "onComplete", "RotateSlab", "onCompleteTarget", gameObject, "time", 0.2));
			
			SpriteRenderer slabRenderer = slab.GetComponent<SpriteRenderer>();
			slabRenderer.sortingOrder = currentEmptyIndex + 2; // Hacky	

			BoxCollider2D slabCollider = slab.GetComponent<BoxCollider2D>();
			slabCollider.enabled = false;
			slabs.Push(slab);

			// Update the total weight of the right side
			int slabWeight = int.Parse(slab.tag);
			totalWeight += slabWeight;
			currentEmptyIndex++;

			// Add the slab to the plank so it rotates with it.  Plus, save the original parent for if the slab is returned.
			if (!childToParent.ContainsKey(slab)) {
				childToParent.Add (slab, slab.transform.parent.gameObject);
			}
			slab.transform.parent = plank.transform;
		}
	}

	void RotateSlab() {
		Util.Log ("Rotate slab");
		GameObject lastSlab = (GameObject)slabs.Peek ();
		iTween.RotateTo (lastSlab, iTween.Hash("z", 0, "onComplete", "WeightCompare", "onCompleteTarget", gameObject, "islocal", true, "time", 0.2));
	}

	void WeightCompare() {
		Util.Log ("Weight compare");
		int difference = targetWeight - totalWeight;
		float timeToRotate = 6 / Mathf.Max(Mathf.Pow(Mathf.Abs (difference), 0.25f), 1.0f);

		if (difference > 0) {
			iTween.RotateTo (plank, iTween.Hash ("z", 20, "time", timeToRotate, "onComplete", "PlankRotated", "onCompleteTarget", gameObject));
		} else if (difference < 0) {
			iTween.RotateTo (plank, iTween.Hash ("z", -20, "time", timeToRotate, "onComplete", "PlankRotated", "onCompleteTarget", gameObject));
		} else {
			iTween.RotateTo (plank, iTween.Hash ("z", 0, "time", timeToRotate, "onComplete", "PlankRotated", "onCompleteTarget", gameObject));
		}
	}

	void PlankRotated() {
		Util.Log ("plank rotated");
		PlayMakerFSM fsm = gameObject.GetComponent<PlayMakerFSM> ();
		FsmBool isDrag = fsm.FsmVariables.GetFsmBool ("isDrag");
		isDrag.Value = false;
	}
	
	void RemoveTopPlank() {
		if (slabs.Count > 0) {
			GameObject slab = (GameObject)slabs.Pop ();

			PlayMakerFSM slabFsm = slab.GetComponent<PlayMakerFSM> ();
			GameObject originalParent;
			childToParent.TryGetValue(slab, out originalParent);
			slab.transform.parent = originalParent.transform;

			slabFsm.SendEvent ("remove");
			int slabWeight = int.Parse(slab.tag);
			totalWeight -= slabWeight;
			if (currentEmptyIndex >= 1) {
				currentEmptyIndex--;
			}
			WeightCompare();
		}
	}
}
