using UnityEngine;
using UnityEditor;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using HutongGames.PlayMaker;
using System.Collections.Generic;

[CustomActionEditor(typeof(MultipleBoolSwitch))]
public class MultipleBoolSwitchEditor: CustomActionEditor{

	private List<FsmEvent> _events;

	public override void OnEnable (){
		base.OnEnable ();
		if (target != null && target.Fsm != null) {
			_events = new List<FsmEvent> (target.Fsm.Events);
		}

	}

	public override bool OnGUI(){
	
		if (_events == null) {
			if (target != null && target.Fsm != null) {
				_events = new List<FsmEvent> (target.Fsm.Events);
			}
		}

		MultipleBoolSwitch boolSwitch = target as MultipleBoolSwitch;

		EditField ("bools");

		if (boolSwitch.intValues == null) {
			boolSwitch.intValues = new int[0];
			boolSwitch.optionEvents = new FsmEvent[0];
		}

		int oldOptionCount = boolSwitch.intValues.Length;
		int newOptionCount = EditorGUILayout.IntField ("Number of Options", oldOptionCount);

		int[] oldIntValues = boolSwitch.intValues;
		FsmEvent[] oldOptionEvents = boolSwitch.optionEvents;
		int[] newIntValues;
		FsmEvent[] newOptionEvents;

		if (oldOptionCount != newOptionCount) {
			int min = Mathf.Min (oldOptionCount, newOptionCount);
			newIntValues = new int[newOptionCount];
			newOptionEvents = new FsmEvent[newOptionCount];
			for (int i = 0; i < min; ++i) {
				newIntValues [i] = oldIntValues [i];
				newOptionEvents [i] = oldOptionEvents [i];
			}
		} else {
			newIntValues = oldIntValues;
			newOptionEvents = oldOptionEvents;
		}

		FsmEditorGUILayout.Divider ();

		bool[] bools;

		for (int i = 0; i < newOptionCount; ++i) {
			EditorGUILayout.LabelField ("Option " + (i + 1), EditorStyles.boldLabel);
			newOptionEvents [i] = FsmEditorGUILayout.EventPopup (new GUIContent ("Event"), _events, newOptionEvents [i]);
			bools = boolSwitch.GetBoolArrayFromInt (newIntValues [i]);

			for (int j = 0; j < bools.Length; ++j) {
				bools [j] = EditorGUILayout.Toggle (boolSwitch.bools [j].GetDisplayName (), bools [j]);
			}

			newIntValues [i] = boolSwitch.GetIntFromBoolArray (bools);
			FsmEditorGUILayout.Divider ();
		}

		FsmEditorGUILayout.Divider ();

		EditField ("noMatchEvent");

		if (GUI.changed) {
			boolSwitch.optionEvents = newOptionEvents;
			boolSwitch.intValues = newIntValues;
		}

		return GUI.changed;
	}

}
