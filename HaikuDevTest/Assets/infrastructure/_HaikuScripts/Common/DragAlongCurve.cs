using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class DragAlongCurve : MonoBehaviour, IPointerDownHandler,
IPointerUpHandler,IDragHandler{

	
    float _progress;
	public float progress {
		get {
			return _progress;
		}
	}

    [SerializeField]
    Curve _curveComponent;

    CatmullRomCurve _curve;

    bool _isDragging;


    void Awake(){
		if (_curve == null) {
			if (_curveComponent != null) {
                SetCurve(_curveComponent.GetLocalSpaceCurve(transform.parent));
			}
		}
    }

	public void SetCurve(CatmullRomCurve pCurve, float pProgress) {
		_curve = pCurve;
        _progress = pProgress;
        transform.localPosition = _curve.GetPointOnPath(pProgress); 
	}

    public void SetCurve(CatmullRomCurve pCurve){
        _curve = pCurve;
    }

    public void OnPointerDown(PointerEventData pPointerEventData) {
        if(!enabled){
            return;
        }

        if(_curve == null){
            Debug.LogError("DragAlongCurve has no curve defined");
            return;
        }

        _isDragging = true;
	}

    public void OnDrag(PointerEventData pPointerEventData) {
		if (!enabled) {
			return;
		}

        if(_isDragging){
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 localPosition = transform.parent.InverseTransformPoint(worldPosition);
            transform.localPosition = _curve.GetClosestPointOnPath(localPosition, out _progress);

        }
	}

    public void OnPointerUp(PointerEventData pPointerEventData) {
		if (!enabled) {
			return;
		}
        _isDragging = false;

	}
}
