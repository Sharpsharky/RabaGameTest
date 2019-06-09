using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SarcophagusPuzzleManagerV2 : MonoBehaviour {

	public List<SarcophagusJarV2> jars;

	void Start() { }

	void OnEnable() {
		SarcophagusNode.LockBoxNodeOccupiedAction += LockBoxNodeOccupied;
	}

	void OnDisable() {
		SarcophagusNode.LockBoxNodeOccupiedAction -= LockBoxNodeOccupied;

	}

	private void checkWon() {
		bool didWin = true;
		foreach (SarcophagusJarV2 jar in this.jars)  {
			Debug.Log("Jar " + jar + " in node " + jar.currentNode + ", Target: " + jar.targetNode);
			if (jar.currentNode != jar.targetNode) {
				didWin = false;
			}
		}

		if (didWin) {
			gameObject.GetComponent<PlayMakerFSM>().SendEvent("won");
			this.DisableAllColliders();
		}
	}

	#region Subscribed Action callbacks
	void LockBoxNodeOccupied(SarcophagusNode node) {
		this.checkWon();
	}
	#endregion

	#region Puzzle End Methods
	void DisableAllColliders() {
		Collider2D[] colliders = GetComponentsInChildren<Collider2D> ();
		for (int i = 0; i < colliders.Length; i++) {
			colliders [i].enabled = false;
		}
	}
	#endregion

	#region Public Methods
	public void RestartGame() {
		foreach (SarcophagusJarV2 jar in this.jars)  {
			jar.Reset();
		}
	}
	#endregion

}
