using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Curve))]
public class CurveEditor : Editor
{
	Curve _target;
	GUIStyle style = new GUIStyle();

    CatmullRomCurve _curve;

	void OnEnable(){
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;
		_target = (Curve)target;
        _curve = _target.GetWorldSpaceCurve();
        Undo.undoRedoPerformed += OnUndo;
	}
	
    void OnDisable(){
       Undo.undoRedoPerformed -= OnUndo;
    }

    void OnUndo(){
        _curve = _target.GetWorldSpaceCurve();
    }

	public override void OnInspectorGUI(){

        _target.pathColor = EditorGUILayout.ColorField("Path Color", _target.pathColor);
        _target.isLocal = EditorGUILayout.Toggle("Is Local", _target.isLocal);
        _target.smoothAmount = EditorGUILayout.IntSlider("Smooth Amount", _target.smoothAmount, 1, 20);
        int newCount  = EditorGUILayout.IntField("Node Count", _target.nodes.Count);

		//add node?
		if(newCount > _target.nodes.Count){
			for (int i = 0; i < newCount - _target.nodes.Count; i++) {
				_target.nodes.Add(Vector3.zero);	
			}
		}
	
		//remove node?
		if(newCount < _target.nodes.Count){
			if(EditorUtility.DisplayDialog("Remove path node?","Shortening the node list will permanently destroy parts of your path. This operation cannot be undone.", "OK", "Cancel")){
				int removeCount = _target.nodes.Count - newCount;
				_target.nodes.RemoveRange(_target.nodes.Count-removeCount,removeCount);
			}
		}
				
		//node display:
		for (int i = 0; i < _target.nodes.Count; i++) {
			_target.nodes[i] = EditorGUILayout.Vector3Field("Node " + (i+1), _target.nodes[i]);
		}

		
		if(GUI.changed){
            _curve = _target.GetWorldSpaceCurve();
			EditorUtility.SetDirty(_target);			
		}
	}
	
	void OnSceneGUI(){
		if(_target.enabled) { 
			if(_target.nodes.Count > 0){
                bool dirty = false;

				//allow path adjustment undo:
                Undo.RecordObject(_target,"Adjust Curve");
				
                //node handle display:
                Vector3 newWorldPosition;
                Vector3 oldWorldPosition;

				for (int i = 0; i < _target.nodes.Count; i++) {
                    
                    if(_target.isLocal){
                        oldWorldPosition = _target.transform.TransformPoint(_target.nodes[i]);  
                    }else{
                        oldWorldPosition = _target.nodes[i];
                    }

                    if(i == 0){
                        Handles.Label(oldWorldPosition, "Begin", style);
                    }

                    if(i == _target.nodes.Count -1){
                        Handles.Label(oldWorldPosition, "End", style);
                    }

                    newWorldPosition = Handles.PositionHandle(oldWorldPosition, Quaternion.identity);

                    if(newWorldPosition != oldWorldPosition){
                        dirty = true;
                    }

					if (_target.isLocal) {
                        _target.nodes[i] = _target.transform.InverseTransformPoint(newWorldPosition);
					} else {
                        _target.nodes[i] = newWorldPosition;
					}
				}

				if (dirty){
                    _curve = _target.GetWorldSpaceCurve();
                }

				Vector3 prevPt = _curve.GetPointOnPath(0);
                Handles.color = _target.pathColor;
				int numLines = _target.nodes.Count * _target.smoothAmount;
				for (int i = 1; i <= numLines; i++) {
					float pm = (float)i / numLines;
					Vector3 currPt = _curve.GetPointOnPath(pm);
                    Handles.DrawLine(currPt,prevPt);
					prevPt = currPt;
				}
			}


		}
	}
}