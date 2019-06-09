using UnityEngine;
using System.Collections;

public class SetSortingLayer : MonoBehaviour {
	public string sortingLayerID;
	public int sortingOrder;
	// Use this for initialization
	void Start () {
		if (gameObject.GetComponent<Renderer>() != null) {
			GetComponent<Renderer>().sortingLayerName = sortingLayerID;
			GetComponent<Renderer>().sortingOrder = sortingOrder;
		}	
	}
}
