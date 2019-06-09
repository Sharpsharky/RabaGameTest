using UnityEngine;
using System.Collections;

public class SetSortingOrder : MonoBehaviour {
	public int sortingLayerOrder = 0;
	public string sortingLayerName = "Default";
	
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer>().sortingLayerName = sortingLayerName;
		gameObject.GetComponent<Renderer>().sortingOrder = sortingLayerOrder;
	}

}
