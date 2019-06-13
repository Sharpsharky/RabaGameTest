using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic puzzle controller from which other puzzle controllers can be inherited.
/// Implements the IPuzzleHandler interface which has the common puzzle methods (Win, Lose, Skip, Activate, Deactivate).
/// Contains the skipButton, backButton, wonEventFsm, and other references that are common in most puzzles.
/// </summary>
public abstract class PuzzleController : MonoBehaviour, IPuzzleHandler {

    const string HAS_BEEN_ACTIVATED_KEY = "kHasBeenActivated";

    [SerializeField]
    protected PuzzleUI _puzzleUI;

	[Tooltip("The fsm to send won event to. Set this in inspector.")]
    [SerializeField]
    protected PlayMakerFSM _wonEventFsm;

    bool _hasBeenActivated = false;
    public bool hasBeenActivated{
        get{
            return _hasBeenActivated;
        }
    }

	#region IPuzzleHandler Methods
	protected virtual void Win () {
        
        if(_puzzleUI != null){
            _puzzleUI.Deactivate();
        }
       
       
		//-- Sending won event
		if (_wonEventFsm != null) {
			_wonEventFsm.SendEvent ("won");
		}

		Debug.Log ("Puzzle Won");
	}

	public virtual void Skip () {
		Win ();
	}

    protected virtual void Lose(){
        if (_wonEventFsm != null) {
            _wonEventFsm.SendEvent("lose");
        }
    }

    public virtual void SkipPanelShown()
    {
    }

    public virtual void SkipPanelCanceled()
    {
    }

  

	public virtual void Activate () {
        _hasBeenActivated = true;
        if (_puzzleUI != null) {
            _puzzleUI.Activate();
        }
	}

	public virtual void Deactivate () {
        if (_hasBeenActivated && _puzzleUI != null) {
            _puzzleUI.Deactivate();
        }
       
	}

	public virtual void ResetPuzzle(){
	}
	#endregion
}
