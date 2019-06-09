using UnityEngine;
using System.Collections;

public class RoomHelper : MonoBehaviour {
	public const string kStartingRoomPlatformSpecifics = "StartingRoomPlatformSpecifics";
	public const string kOtherRoomPlatformSpecifics = "OtherRoomsPlatformSpecifics";
	public bool isStartingRoom = false;

    [SerializeField]
	bool _startPuzzleImmediately = true;

	private Vector3 _outPosition;

	public Vector3 outPosition{ get { return _outPosition; } }

    [SerializeField, Tooltip("This puzzle will get activated when the room is entered")]
    PuzzleController _puzzleController;


	void Awake () {
		//save out position so that positioning doesn't get messed up by HaikuSaveableObjects on rooms
		_outPosition = transform.position;
        string firstTwoChars = name.Substring(0, 2);

        int roomId;
        if (!int.TryParse(firstTwoChars, out roomId)) {
            // Set RoomID to a two digit number. 
            // If it isn't two digits, just grab the first number.
            roomId = int.Parse(name[0].ToString());
        }

        ChapterSceneManager sceneManager = ChapterSceneManager.instance;
        sceneManager.RegisterRoom(roomId, this);
	}

	public void ActivateRoom () {
        if(_puzzleController == null){
            return;
        }

        bool puzzleHasBeenActivated =  _puzzleController.hasBeenActivated;

        if(puzzleHasBeenActivated){
             _puzzleController.Activate();
        }else{
            if(_startPuzzleImmediately){
               _puzzleController.Activate();
            }
        }
	}

	public void DeActivateRoom () {

		if (_puzzleController == null) {
			return;
		}

        _puzzleController.Deactivate();
	}
}
