using UnityEngine;
using System.Collections;
using InputEventNS;
using TMPro;

public class VendingMachineManager : MonoBehaviour {
	private Collider2D[] colliders;
	private InputHandler touchOrMouseListener;
	public TextMeshPro codeTMPro;

	public Collider2D resetCollider;
	public Collider2D enterCollider;

	public string[] codesThatSendEvent;
	public GameObject[] targetOfEvent;

	private bool hasCoin; // TODO this later
	public int maxPasscodeLength;

	public string codeActiveDefaultText = "";
	public SpriteRenderer redLight;

	public AudioClip tapButton;
	public AudioClip errorSound;

	// Use this for initialization
	void Start () {
		touchOrMouseListener = InputEvent.AddListenerTouchOrMouse(TouchOrMouseStart, null, null, 1.0f) ;
		colliders = GetComponentsInChildren<Collider2D>();
	}

	void TouchOrMouseStart (InputHandler handler) {

		InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 wp = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		Vector2 touchPos = new Vector2(wp.x, wp.y);

		// Make sure you are touching this object
		foreach (Collider2D targetCollider in colliders) {
			if (targetCollider == Physics2D.OverlapPoint(touchPos)) {
				Debug.Log("Collider pressed: " + targetCollider.gameObject.name);
				if (targetCollider == resetCollider) {
					ResetCode();
				} else if (targetCollider == enterCollider) {
					CheckIfEvent();
				} else {
					NumberPressed(targetCollider.gameObject.name);
				}
			}
		}
	}

	private void ResetCode() {
		codeTMPro.text = codeActiveDefaultText;
	}
		
	public void NumberPressed(string number) {
		Helper.PlayAudioIfSoundOn(tapButton);
		string currentText = codeTMPro.text;
		if (codeTMPro.text.Length > maxPasscodeLength) {
			return;
		} else {
			string newText = currentText + number;
			codeTMPro.text = newText;
//			Helper.PlayAudioIfSoundOn(itemPressedSound);
		}
	}

	public void CheckIfEvent() {
		bool eventSent = false;
		for (int i = 0; i < codesThatSendEvent.Length; i++) {
			if (codesThatSendEvent[i].Equals(codeTMPro.text)) {
				Debug.Log("Match at : " + codeTMPro.text + " and: " + i);
				targetOfEvent[i].GetComponent<PlayMakerFSM>().SendEvent("activate");
				eventSent = true;
				ResetCode();
				break;
			}
		}
		if (!eventSent) {
			if (redLight) {
				redLight.enabled = true;
				codeTMPro.renderer.enabled = false;
			}
			Debug.Log("no event sent");
			Invoke("NumberIncorrect", 0.5f);
		}
	}

	private void NumberIncorrect() {
		if (redLight) {
			redLight.enabled = false;
			codeTMPro.renderer.enabled = true;
		}
		Helper.PlayAudioIfSoundOn(errorSound);
		codeTMPro.text = codeActiveDefaultText;
	}

	void OnDisable() {
		InputEvent.RemoveListener(touchOrMouseListener);
	}

}
