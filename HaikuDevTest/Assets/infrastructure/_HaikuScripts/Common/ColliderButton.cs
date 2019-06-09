using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class ColliderButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,
IPointerExitHandler{

	public delegate void TapHandler(ColliderButton pButton);

	[SerializeField]
    protected SpriteRenderer _spriteRenderer;

	public Sprite upState;

	public Sprite downState;

	public bool stayDownOnTap = false;

    protected List<TapHandler> _handlers;

    protected bool _isDown = false;

    public bool interactable = true;

	public void AddTapListener(TapHandler pHandler){
		if (_handlers == null) {
			_handlers = new List<TapHandler> ();
		}
		_handlers.Add (pHandler);
	}

	public void RemoveTapListener(TapHandler pHandler){
		if (_handlers == null) {
			return;
		}

		_handlers.Remove (pHandler);
	}

	public void RemoveAllListeners(){
		if (_handlers == null) {
			return;
		}

		_handlers.Clear ();
	}

	public void ResetButton(){
		SetUpState (true);
	}


	private void Start(){
		ResetButton();
	}


	private void OnDestroy(){
		RemoveAllListeners ();
	}

    public virtual void OnPointerDown(PointerEventData pPointerEventData) {
  
        if (!interactable) {
			return; 
		}

        SetDownState();

        if (_handlers != null && _handlers.Count > 0) {

            for (int i = 0; i < _handlers.Count; ++i) {
                _handlers[i](this);
            }
        }
	}

    public virtual void OnPointerExit(PointerEventData pPointerEventData) {

        if (_isDown && !stayDownOnTap) {
            SetUpState(interactable);
        }
	}

    public virtual void OnPointerUp(PointerEventData pPointerEventData) {
		if (_isDown && !stayDownOnTap) {
            SetUpState (interactable);
		}
	}

	protected virtual void SetDownState(){
		_isDown = true;

		if (stayDownOnTap) {
            interactable = false;
		}
		if (_spriteRenderer != null) {
			_spriteRenderer.sprite = downState;
		}
	}


    protected virtual void SetUpState(bool allowInteraction){
		_isDown = false;
        interactable = allowInteraction;
		if (_spriteRenderer != null) {
			_spriteRenderer.sprite =upState;
		}
	}
}
