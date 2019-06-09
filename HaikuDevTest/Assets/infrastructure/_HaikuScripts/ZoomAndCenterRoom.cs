using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class ZoomAndCenterRoom : MonoBehaviour {
	private GameObject room {
		get {
			return ChapterSceneManager.instance.currentRoom.gameObject;
		}
	}
	private Vector3 originalRoomScale;
	public GameObject itemToCenter = null;
	private Vector3 moveToMakeCenter;
	private Vector3 originalRoomPosition;
	private ToggleOffChildColliders toggleOffScript;
	public float zoomInTime = 0.5f;
	public float moveToTime = 0.5f;
	// Use this for initialization
	void Start () {
		// Default center the object you are focused on
		if (itemToCenter == null) {
			itemToCenter = gameObject;
		}


		if (gameObject.GetComponent<ToggleOffChildColliders>() == null) {
			gameObject.AddComponent<ToggleOffChildColliders>();
		}
		toggleOffScript = gameObject.GetComponent<ToggleOffChildColliders>();
	}

	void ZoomRoomIn(float scaleFactor) {
		originalRoomScale = room.transform.localScale;
		originalRoomPosition = room.transform.position;
		toggleOffScript.turnOffColliders();
		iTween.ScaleBy(room, iTween.Hash("amount", new Vector3(scaleFactor, scaleFactor, scaleFactor), 
		                                 "OnCompleteTarget", gameObject, 
		                                 "OnComplete", "CallbackZoomedIn", 
										 "time", zoomInTime,
		                                 "easetype", iTween.EaseType.linear));
	}
		
	void CallbackZoomedIn() {
		moveToMakeCenter = room.transform.position - itemToCenter.transform.position;
		iTween.MoveTo(room, iTween.Hash("position", moveToMakeCenter, "time", moveToTime, "easetype", iTween.EaseType.linear));
	}

	void CallbackZoomedOut() {
		iTween.ScaleTo(room, iTween.Hash("scale", originalRoomScale, "OnCompleteTarget", gameObject, "OnComplete", "ZoomOutComplete"));
	}

	void ZoomOutComplete() {
		toggleOffScript.turnOnColliders();
	}

	void ZoomRoomOut() {
		iTween.MoveTo(room, iTween.Hash("position", originalRoomPosition, 
		                                "time", 1.0, 
		                                "OnCompleteTarget", gameObject, 
		                                "OnComplete", "CallbackZoomedOut"));
	}
}
