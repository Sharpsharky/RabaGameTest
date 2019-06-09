using UnityEngine;
using System.Collections;
using InputEventNS;

[RequireComponent(typeof(MouseController))]
public class DragItemToTarget : MonoBehaviour {
	private Vector3 originalLocalPosition;

	public bool destroyAfterSuccess = true;
	public GameObject target;
	public string localizationKey; // If left empty, it will not trigger the top bar
	public string playmakerEventForSelfIfSuccess; // If left empty, no event. Sends if you do not have "destroyAfterSuccess"
	public string playmakerEventForTargetIfSuccess = "activate";
	public bool snapBackOnRelease = true;
    public bool resetRotationOnDrag = true;

	private Collider2D targetCollider;

	private MouseController mouseController;

	// Use this for initialization
	private void Awake () {
		targetCollider = target.GetComponent<Collider2D> ();
		if (mouseController == null) {
			mouseController = gameObject.GetComponent<MouseController> ();
		}
		originalLocalPosition = transform.localPosition;
	}

	private void Start(){
		mouseController.MouseUpEvent += MouseUpHandler;
		mouseController.MouseDownEvent += MouseDownHandler;
	}

	private void OnDestroy(){
		mouseController.MouseUpEvent -= MouseUpHandler;
		mouseController.MouseDownEvent += MouseDownHandler;
	}

	private void MouseDownHandler (){
        if(resetRotationOnDrag){
            gameObject.transform.rotation = Quaternion.identity;
        }
		
	}

	private void MouseUpHandler (){
        if (resetRotationOnDrag) {
            gameObject.transform.rotation = Quaternion.identity;
        }

		if (targetCollider.OverlapPoint(transform.position)) {
			PlayMakerFSM targetFSM = target.GetComponent<PlayMakerFSM> ();
			if (targetFSM != null) {
				targetFSM.SendEvent(playmakerEventForTargetIfSuccess);
			}

			if (destroyAfterSuccess) {
				Destroy(gameObject);
			} else {
				gameObject.transform.localPosition = originalLocalPosition;
				if (!string.IsNullOrEmpty(playmakerEventForSelfIfSuccess)) {
					GetComponent<PlayMakerFSM>().SendEvent(playmakerEventForSelfIfSuccess);
				}
			}
		} else {
			Helper.LocalizeKeyToTopBar(localizationKey);
			if (snapBackOnRelease) {
				gameObject.transform.localPosition = originalLocalPosition;
			}

		}
	}

	private void OnEnable() {
		if (mouseController == null) {
			mouseController = gameObject.GetComponent<MouseController> ();
		}
		mouseController.enabled = true;
	}

	private void OnDisable() {
		mouseController.enabled = false;
	}

}
