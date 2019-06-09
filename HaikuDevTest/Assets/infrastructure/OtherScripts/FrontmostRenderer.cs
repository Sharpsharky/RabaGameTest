using UnityEngine;
using System.Collections;

public class FrontmostRenderer : MonoBehaviour {
	public int sortingLayerOrder = 0;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer>().sortingLayerName = "Frontmost";
		gameObject.GetComponent<Renderer>().sortingOrder = sortingLayerOrder;
	}
}
