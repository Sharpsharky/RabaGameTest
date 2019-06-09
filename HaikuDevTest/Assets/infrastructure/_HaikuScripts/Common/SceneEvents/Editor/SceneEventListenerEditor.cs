using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;
using HutongGames.PlayMaker;

[CustomEditor(typeof(SceneEventListener))]
public class SceneEventListenerEditor : Editor {

	private SceneEventDispatcher _eventDispatcher;

	public override void OnInspectorGUI() {

		if (_eventDispatcher == null) {
			_eventDispatcher = GameObject.FindObjectOfType<SceneEventDispatcher> ();
		}

		if (_eventDispatcher == null) {
			EditorGUILayout.HelpBox ("No SceneEventDispatcher in scene", MessageType.Error);
			return;
		}

		string[] possibleEventNames = _eventDispatcher.eventNames;

		if (possibleEventNames.Length == 0) {
			EditorGUILayout.HelpBox ("SceneEventDispatcher has events no events defined", MessageType.Error);
			return;
		}

		SceneEventListener sceneEventListener = (SceneEventListener)target;

		string[] oldEventNames = sceneEventListener.eventsToListenFor;

		if (oldEventNames == null) {
			oldEventNames = new string[0];
		}

		int oldCount = oldEventNames.Length;

		string[] newEventNames;
		EditorGUILayout.LabelField ("Events To Listen For", EditorStyles.boldLabel);
		int newCount = EditorGUILayout.IntField ("Count", oldCount);

		newEventNames = new String[newCount];
		int min = Mathf.Min (oldCount, newCount);
		for (int i = 0; i < min; ++i) {
			newEventNames [i] = oldEventNames [i];
		}

		int index;
		for (int i = 0; i < newCount; ++i) {
			index = Array.IndexOf<string> (possibleEventNames, newEventNames [i]);
			if (index < 0) {
				index = 0;
			}
			index = EditorGUILayout.Popup ("Event Name", index, possibleEventNames);
			newEventNames [i] = possibleEventNames [index];
		}

		if (GUI.changed) {
			IEnumerable<string> eventsToRemove = oldEventNames.Except(newEventNames);
			IEnumerable<string> eventsToAdd= newEventNames.Except(oldEventNames);

			sceneEventListener.eventsToListenFor = newEventNames;

			PlayMakerFSM[] fsms = sceneEventListener.GetComponents<PlayMakerFSM>();

			List<FsmEvent> newEventList = new List<FsmEvent> ();

			foreach (PlayMakerFSM fsm in fsms) {
				newEventList.Clear ();
				foreach (FsmEvent fsmEvent in fsm.Fsm.Events) {
					if (!eventsToAdd.Contains (fsmEvent.Name) && !eventsToRemove.Contains (fsmEvent.Name)) {
						//Debug.Log (fsmEvent.Name);
						newEventList.Add (fsmEvent); 
					}/*else {
						Debug.Log ("removing " + fsmEvent.Name);
					}*/
				}

				foreach (string eventName in eventsToAdd) {
					//Debug.Log ("adding " + eventName);
					newEventList.Add (new FsmEvent (eventName));
				}

				fsm.Fsm.Events = newEventList.ToArray ();
				EditorUtility.SetDirty (fsm);
			}

			EditorUtility.SetDirty (target);
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());


		}
	}
}
