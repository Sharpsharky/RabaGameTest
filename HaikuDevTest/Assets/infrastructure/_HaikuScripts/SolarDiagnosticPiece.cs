using UnityEngine;
using System.Collections;
using InputEventNS;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SolarDiagnosticPiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	private bool isTouched = false;
	public int column;
	public int row;
	public bool isSource;
	public bool isObstacle;
	public List<SolarDiagnosticPiece> neighbors;
	public List<SolarDiagnosticPiece> siblings;
	public SolarDiagnosticManager manager;
	
	public Sprite neutralColor;
	public Sprite activeColor;

	private InputHandler touchOrMouseListener;

    private SolarDiagnosticPiece previousPiece;
    public SolarDiagnosticPiece PreviousPiece { get { return previousPiece; } set { previousPiece = value; } }

    [SerializeField]
    private bool isGoal;
    public bool IsGoal { get { return isGoal; } }

    [SerializeField]
    private bool useIsGoalKey;

    private bool _activated;
	
	public bool activated
	{
		get { return _activated; }
		set
		{
			_activated = value;
			if (value) {
            // Debug.Log("Setting sprite: " + isObstacle);
				GetComponent<SpriteRenderer>().sprite = activeColor;
			} else {
				GetComponent<SpriteRenderer>().sprite = neutralColor;
			}
		}
	}
	
	// Use this for initialization
	void Awake ()
    {
		int.TryParse(name[0].ToString(), out column);
		int.TryParse(name[1].ToString(), out row);
	}

    /*
	private void TouchOrMouseStart(InputHandler handler) {
        
        InputHandlerPointer pointer = (InputHandlerPointer)handler;
		Vector3 currentPos = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
		
		// Make sure you are touching this object
		Vector2 touchPos = new Vector2(currentPos.x, currentPos.y);
		if (GetComponent<Collider2D>().OverlapPoint(touchPos)) {
			Debug.Log("Touch or mouse start: " + name);


            try { isTouched = manager.StartLineFromPieceTap(this); } catch { }

            if (isObstacle && _activated)
            {
                Helper.LocalizeKeyToTopBar(manager.ObstacleTxtKey);
            }

            if (isGoal && useIsGoalKey) Helper.LocalizeKeyToTopBar(manager.GoalTxtKey);
        } 
	}

	private void TouchOrMouseChange(InputHandler handler) {
		if (isTouched) {
			InputHandlerPointer pointer = (InputHandlerPointer)handler;
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointer.currentPosition);
			
			foreach (SolarDiagnosticPiece sibling in siblings) {
				if (sibling.GetComponent<Collider2D>().OverlapPoint(worldPosition)) {
					manager.SiblingCollided(sibling);
				}
			}
		}
	}
	
	
	
	private void TouchOrMouseEnd(InputHandler handler) {
		EndTouch();
	}
	*/

    public void EndTouch()
    {
        if (isTouched)
        {
            isTouched = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EndTouch();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        try { isTouched = manager.StartLineFromPieceTap(this); } catch { }

        if (isObstacle && _activated) Helper.LocalizeKeyToTopBar(manager.ObstacleTxtKey);

        if (isGoal && useIsGoalKey) Helper.LocalizeKeyToTopBar(manager.GoalTxtKey);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isTouched)
        {
            Vector3 worldPosition = eventData.pointerCurrentRaycast.worldPosition;

            foreach (SolarDiagnosticPiece sibling in siblings)
            {
                if (sibling.GetComponent<Collider2D>().OverlapPoint(worldPosition))
                {
                    manager.SiblingCollided(sibling);
                }
            }
        }
    }
}
