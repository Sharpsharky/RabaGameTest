using UnityEngine;
using System.Collections;

public class UIPositionMarkerHelper : MonoBehaviour {
	public GameObject marker;
	public string optionalMarkerName; // marker name has priority over marker game object
	public Vector3 scale = new Vector3(1, 1, 1);

	// the problem is that Bert is marked as StartsWith. InventoryItemHelper uses the Start function, so this UIPositionMarkerHelper.
	// What is happening is Bert is being moved to the inventory slot (which is currently in the horizontal position), 
	// then the inventory slot is being moved to the vertical position.

	// We can do this on Start because PlatformSpecifics executes on Awake
	void Start () {
		if (Helper.IsRightUI()) {
			if (string.IsNullOrEmpty(optionalMarkerName)) {
				gameObject.transform.position = marker.transform.position;
			} else {
				GameObject markerName = GameObject.Find(optionalMarkerName);
				gameObject.transform.position = markerName.transform.position;
			}
			gameObject.transform.localScale = scale;
		}
	}
}
