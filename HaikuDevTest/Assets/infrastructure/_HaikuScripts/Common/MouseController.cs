using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// Mouse controller.
/// 
/// This is generic mouse controller. Attach it to any gameobject to make the object draggable using mouse and touch.
/// Subscribe to the public static events of this class to extend its functionality in your own class.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class MouseController : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler
{
	//-- Events that are triggered with different Mouse/Touch behaviors.
	public Action MouseDownEvent; 
	public Action MouseDragEvent; 
	public Action MouseUpEvent;
	public Action MouseClickEvent;

	private bool controlEnabled = true;

	[Tooltip("The object cannot be dragged if this is set to true.")]
	public bool isStatic = false;

	[Tooltip("Click event will only be triggered if this is true.")]
	public bool allowClick = false;

	[Tooltip("Set this if you do not want to move the object on x-axis.")]
	public bool lockXAxis = false;

	[Tooltip("Set this if you do not want to move the object on y-axis.")]
	public bool lockYAxis = false;

	[Tooltip("Greater than this time is a drag, otherwise it is eligible to be a click.")]
	public double maxTimeForClick = 0.25;

	[Tooltip("Greater than this distance is a drag, otherwise click")]
	public float dragThreshold = 0.2f;

	private DateTime mouseDownStartTime;

	[HideInInspector]
	public bool isTouched = false;

	/// <summary>
	/// The moved distance. Used to find the distance that the object has moved on mouse drag.
	/// </summary>
	[HideInInspector]
	public float movedDistance = 0f;

	[HideInInspector]
	public Vector3 movedVector;

	[HideInInspector]
	public Vector2 lastMousePosition;

	private Vector3 offset;

	[SerializeField,Tooltip("Confines dragging within the specified rectangle.")]
	private RectTransform draggableArea;

	[SerializeField,Tooltip("Offset from the edge of draggableAreaRect, if assigned")]
	private float draggableAreaOffset;

	[SerializeField]
	private bool dragInFrontSortingLayer = false;

	private SpriteRenderer[] spriteRenderers;
	private string[] cachedSortingLayers;

	private Rect draggableAreaRect;

    private const string FRONT_SORTING_LAYER_NAME = "Front FX";

    [SerializeField]
    private string mouseClickFSMEvent;

    [SerializeField, Tooltip("After releasing object send event to fsm. Doesnt trigger if click triggers.")]
    private bool sendEventOnRelease = false;

    [SerializeField]
    private string relaseFSMEvent;

    private PlayMakerFSM fsm;

	private void Awake(){
		if (draggableArea != null) {
			draggableAreaRect = draggableArea.rect;
			draggableAreaRect.xMin += draggableAreaOffset;
			draggableAreaRect.xMax -= draggableAreaOffset;
			draggableAreaRect.yMin += draggableAreaOffset;
			draggableAreaRect.yMax -= draggableAreaOffset;
		}

        fsm = GetComponent<PlayMakerFSM>();

		if (dragInFrontSortingLayer) {
			spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer> ();

			cachedSortingLayers = new string[spriteRenderers.Length];

			for (int i = 0; i < spriteRenderers.Length; ++i) {
				cachedSortingLayers [i] = spriteRenderers [i].sortingLayerName;
			}
		}
	}

	void OnDestroy () {
		MouseDownEvent = null; 
		MouseDragEvent = null; 
		MouseUpEvent = null;
		MouseClickEvent = null;
	}

	void OnEnable(){
		controlEnabled = true;
	}

	void OnDisable(){
		controlEnabled = false;
	}

	/// <summary>
	/// The mouse clicked flag.
	/// By default it is assumed that the mouse is clicked.
	/// If the mouse drag distance is greater than the 'dragThreshold', then this is set to false.
	/// </summary>
	private bool mouseClicked = false;

    public void OnPointerDown(PointerEventData pPointerEventData) {

		if (controlEnabled) {
			Vector3 wp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 touchPos = new Vector2 (wp.x, wp.y);

			lastMousePosition = touchPos;

			// Make sure you are touching this object
			if (!isStatic) {
				isTouched = true;
				mouseDownStartTime = DateTime.Now;
				offset = gameObject.transform.position - wp;

				if (allowClick) {
					mouseClicked = true;
				}

				SetSortingLayers (true);
			}

			if (MouseDownEvent != null) {
				MouseDownEvent ();
			}
		}
	}
		
	private void SetSortingLayers(bool pUseDefault){
		if (dragInFrontSortingLayer && spriteRenderers != null) {
			for (int i = 0; i < spriteRenderers.Length; ++i) {
				spriteRenderers [i].sortingLayerName = pUseDefault ? FRONT_SORTING_LAYER_NAME : cachedSortingLayers [i];
			}
		}
	}

    public void OnDrag(PointerEventData pPointerEventData) {
		if (controlEnabled) {
			if (isTouched) {
				Vector3 worldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 newPos = worldPosition + offset;

				Vector3 position;
				if (lockXAxis) {
					position = new Vector3 (gameObject.transform.position.x, newPos.y, gameObject.transform.position.z);
				} else if (lockYAxis) {
					position = new Vector3 (newPos.x, gameObject.transform.position.y, gameObject.transform.position.z);
				} else {
					position = new Vector3 (newPos.x, newPos.y, gameObject.transform.position.z);
				}

				if (IsOutOfBounds (position)) {
					position = gameObject.transform.position;
				}

				movedVector = position - gameObject.transform.position;

				gameObject.transform.position = position;

				movedDistance = Vector2.Distance (lastMousePosition, worldPosition);
				lastMousePosition = worldPosition;

				TimeSpan timeFromStartOfClick = DateTime.Now - mouseDownStartTime;
				if (timeFromStartOfClick.TotalSeconds > maxTimeForClick) {
					mouseClicked = false;
				}

				if (mouseClicked == true) {

					if (movedDistance > dragThreshold) {
						mouseClicked = false;
					}
				}

				if (MouseDragEvent != null) {
					MouseDragEvent ();
				}
			}
		}
	}

    public void OnPointerUp(PointerEventData pPointerEventData) {
		if (controlEnabled) {
			if (isTouched) {
				isTouched = false;
				if (mouseClicked == true)
                {
					//-- Send on click event.
					if (MouseClickEvent != null) {
						MouseClickEvent ();
					}
					
                    if(fsm != null && !String.IsNullOrEmpty(mouseClickFSMEvent)){
                        fsm.SendEvent(mouseClickFSMEvent);
                    }
					return;
				}
                else
                {
                    if(sendEventOnRelease) fsm.SendEvent(relaseFSMEvent);
                }

				SetSortingLayers (false);

				if (MouseUpEvent != null) {
					MouseUpEvent ();
				}

			}
		}
		
		mouseClicked = false;
	}

	private bool IsOutOfBounds(Vector3 pPosition){
		if (draggableArea == null) {
			return false;
		}

		Vector2 localRectPosition = draggableArea.InverseTransformPoint (pPosition);
		return !draggableAreaRect.Contains (localRectPosition);
	}
		
}