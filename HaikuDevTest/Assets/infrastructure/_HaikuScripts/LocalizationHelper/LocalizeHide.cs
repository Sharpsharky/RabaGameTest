using UnityEngine;
using System.Collections;


public class LocalizeHide : MonoBehaviour {
	public bool hideIfEnglish = false;

	// Use this for initialization
	void Start () {

	}

	private void CheckIfHide() {

	}

	private void Hide(bool isHide) {
		this.transform.GetComponent<Renderer>().enabled = !isHide;
	}

	void OnDisable() {
		
	}
	

}
