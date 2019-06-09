using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEventDispatcher: MonoBehaviour{

	private Dictionary<string,List<PlayMakerFSM>> _fsmListeners;

	private static SceneEventDispatcher s_instance;

	[SerializeField]
	private string[] _eventNames;

	public string[] eventNames{
		get{
			return _eventNames;
		}
	}

	public static SceneEventDispatcher instance{
		get{
			if (s_instance == null) {
				Debug.LogError ("No SceneEventDispatcher component in scene");
			}
			return s_instance;
		}
	}

	public static bool initialized{
		get{
			return s_instance != null;
		}
	}

	private void Awake(){
		s_instance = this;
		_fsmListeners = new Dictionary<string,List<PlayMakerFSM>> ();
	}

	private void OnDestroy(){
		if (_fsmListeners != null) {
			_fsmListeners.Clear ();
		}

	}
		
	public void AddListener(string pEventName, PlayMakerFSM pFSM){
		if (_fsmListeners.ContainsKey (pEventName)) {
			_fsmListeners [pEventName].Add (pFSM);
		} else {
			List<PlayMakerFSM> fsms = new List<PlayMakerFSM> ();
			fsms.Add (pFSM);
			_fsmListeners.Add (pEventName, fsms);
		}
	}

	public void RemoveListener(string pEventName,PlayMakerFSM pFSM){
		if (_fsmListeners.ContainsKey (pEventName)) {
			_fsmListeners [pEventName].Remove (pFSM);
		}
	}

	public void SendSceneEvent(string pEventName){
		List<PlayMakerFSM> listeners;

		if (_fsmListeners.TryGetValue (pEventName, out listeners)) {
			foreach (PlayMakerFSM listener in listeners) {
				listener.SendEvent (pEventName);
			}
		}
	}
}
