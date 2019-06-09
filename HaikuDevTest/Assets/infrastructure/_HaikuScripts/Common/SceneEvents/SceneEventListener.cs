using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayMakerFSM))]
public class SceneEventListener : MonoBehaviour {


	[SerializeField]
	private string[] _eventsToListenFor;

	private PlayMakerFSM[] _fsms;

	private bool _started = false;

	#if UNITY_EDITOR
	public string[] eventsToListenFor{
		get{
			return _eventsToListenFor;
		}set{
			_eventsToListenFor = value;
		}
	}
	#endif

	private void Start(){
		_started = true;

		if (_fsms == null) {
			_fsms = GetComponents<PlayMakerFSM> ();
		}

		foreach (string eventName in _eventsToListenFor) {
			foreach (PlayMakerFSM fsm  in _fsms) {
				SceneEventDispatcher.instance.AddListener (eventName, fsm);
			}
		}
	}

	private void OnEnable(){

        if (!_started || !SceneEventDispatcher.initialized) {
			return;
		}

		if (_fsms == null) {
			_fsms = GetComponents<PlayMakerFSM> ();
		}
			
		foreach (string eventName in _eventsToListenFor) {
			foreach (PlayMakerFSM fsm  in _fsms) {
				SceneEventDispatcher.instance.AddListener (eventName, fsm);
			}
		}
	}

	private void OnDisable(){
		if (_fsms == null || !SceneEventDispatcher.initialized) {
			return;
		}

		foreach (string eventName in _eventsToListenFor) {
			foreach (PlayMakerFSM fsm  in _fsms) {
				SceneEventDispatcher.instance.RemoveListener (eventName, fsm);
			}
		}
	}
}
