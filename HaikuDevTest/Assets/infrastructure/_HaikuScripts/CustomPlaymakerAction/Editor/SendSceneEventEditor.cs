using UnityEngine;
using UnityEditor;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using HutongGames.PlayMaker;
using System.Collections.Generic;
using System;

[CustomActionEditor(typeof(SendSceneEvent))]
public class SendSceneEventEditor: CustomActionEditor{


	private SceneEventDispatcher _eventDispatcher;


	public override bool OnGUI(){

		if (_eventDispatcher == null) {
			_eventDispatcher = GameObject.FindObjectOfType<SceneEventDispatcher> ();
		}

		if (_eventDispatcher == null) {
			EditorGUILayout.HelpBox ("No SceneEventDispatcher in scene", MessageType.Error);
			return false;
		}

		string[] possibleEventNames = _eventDispatcher.eventNames;

		if (possibleEventNames == null || possibleEventNames.Length == 0) {
			EditorGUILayout.HelpBox ("SceneEventDispatcher has events no events defined", MessageType.Error);
			return false;
		}

		SendSceneEvent sendSceneEvent = target as SendSceneEvent;

		if (sendSceneEvent == null) {
			return false;
		}

		string eventName = sendSceneEvent.eventName == null ? "" : sendSceneEvent.eventName.Value;
		int index = Array.IndexOf (possibleEventNames, eventName);
		if (index < 0) {
			index = 0;
		}

		index = EditorGUILayout.Popup ("Event Name", index, possibleEventNames);
		sendSceneEvent.eventName = possibleEventNames [index];

		return GUI.changed;
	}

}
