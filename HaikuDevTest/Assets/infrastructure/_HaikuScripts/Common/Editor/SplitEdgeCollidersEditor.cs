using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SplitEdgeColliders))]
public class SplitEdgeCollidersEditor : Editor {

	public override void OnInspectorGUI ()
	{
		SplitEdgeColliders scriptRef = (SplitEdgeColliders) target;

		if (GUILayout.Button ("Split colliders")) {
			scriptRef.SplitColliders ();
		}

		DrawDefaultInspector ();
	}
}
