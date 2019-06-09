using UnityEngine;
using System.Collections;

public class SortLayerTest : MonoBehaviour {
	[UnityToolbag.SortingLayer]
	public int sortLayer1;
	
	[UnityToolbag.SortingLayer]
	public string sortLayer2;
}