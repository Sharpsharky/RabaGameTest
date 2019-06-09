using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using System.Collections.Generic;

public class ChapterSceneManager: MonoBehaviour {

    int _startingRoomId = 0;

	RoomHelper _currentRoom;
	public RoomHelper currentRoom{
		get{
			return _currentRoom;
		}
	}

	Dictionary<int,RoomHelper> _rooms;

	int _currentRoomId;
	public int currentRoomId { 
		get{ 
			return _currentRoomId; 
		} 
	}


	private static ChapterSceneManager s_instance;

	public static ChapterSceneManager instance{
		get{
			if (s_instance == null) {
				//Perhaps Awake hasn't been called yet
				s_instance = GameObject.FindObjectOfType<ChapterSceneManager>();
			}

			return s_instance;
		}
	}

    public delegate void RoomFocusedEvent();
    public event RoomFocusedEvent OnRoomFocused;

	void Awake(){
		s_instance = this;
	}

	void OnDestroy(){
		s_instance = null;
	}
	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(1f);
        yield return null;
        yield return null;
        FocusStartingRoom();


    }

	public void RegisterRoom(int pRoomId, RoomHelper pRoom){
		if (_rooms == null) {
			_rooms = new Dictionary<int,RoomHelper> ();
		}

        if(pRoom.isStartingRoom){
            _startingRoomId = pRoomId;
        }
		_rooms.Add (pRoomId, pRoom);

	}

    public void FocusStartingRoom(){
        if(_startingRoomId >= 0){
            FocusRoom(_startingRoomId);
        }else{
            Debug.Log("NO STARTING ROOM");
        }
    }

	public void FocusRoom(int roomID) {
		Debug.Log ("Focus Room " + roomID);

		if (_currentRoom != null) {
			_currentRoom.DeActivateRoom ();
			_currentRoom.transform.position = _currentRoom.outPosition;
		}

        if (_rooms == null) {
			return;
		}


		RoomHelper newRoom;

		if (!_rooms.TryGetValue (roomID, out newRoom)) {
			Debug.LogError ("Room " + roomID + " doesn't exist");
			return;
		}
		Debug.Log("new room " + newRoom.name);

		_currentRoom = newRoom;
		_currentRoomId = roomID;

		ChapterUIManager.instance.AlignRoomToRoomAreaRect (_currentRoom);



        _currentRoom.ActivateRoom();
        if (OnRoomFocused != null)
        {
            OnRoomFocused();
        }
    }

    void OnLoginInfoRefreshed() {
        if(_currentRoom != null){
            _currentRoom.ActivateRoom();
            if (OnRoomFocused != null) {
                OnRoomFocused();
            }
        }


    }
}
