using UnityEngine;
using System.Collections;

public class CryptoCheckIfWin : MonoBehaviour {
	public GameObject parentOfCryptoChildren;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void CheckIfWin() {
		PlayMakerFSM[] fsms = parentOfCryptoChildren.GetComponentsInChildren<PlayMakerFSM> ();
		foreach (PlayMakerFSM fsm in fsms) {
			bool isCorrect = fsm.FsmVariables.GetFsmBool("isCorrect").Value;
			Debug.Log(fsm.gameObject.name + " is correct " + isCorrect);
			if (!isCorrect) {
				SendOwnerFSMEvent("isIncorrect");
				return;
			}
		}
		SendOwnerFSMEvent ("isCorrect");
	}

	private void SendOwnerFSMEvent(string eventName) {
		PlayMakerFSM fsm = this.GetComponent<PlayMakerFSM> ();
		fsm.SendEvent (eventName);
	}
}
