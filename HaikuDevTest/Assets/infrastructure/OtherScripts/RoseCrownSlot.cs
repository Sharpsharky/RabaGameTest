using UnityEngine;
using System.Collections;

public class RoseCrownSlot : MonoBehaviour {
	public RoseAndCrownManager manager;
	public GameObject piece;
	public int slot;

	// Use this for initialization
	void Start () {
		string name = gameObject.name.Substring(0,1);
		slot = int.Parse (name);
		Debug.Log ("Rose Crown Int " + slot);
	}


	void OnMouseDown() {
		manager.SlotTapped (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
