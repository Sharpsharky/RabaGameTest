/*
The MIT License (MIT)

Copyright (c) 2016 GuyQuad

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

You can contact me by email at guyquad27@gmail.com or on Reddit at https://www.reddit.com/user/GuyQuad
*/


using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(ArcCollider2D))]
public class ArcCollider_Editor : Editor {
    
    ArcCollider2D ac;
	GameObject edgeHolder = null;

    void OnEnable()
    {
        ac = (ArcCollider2D)target;
		if (ac.gameObject.transform.childCount == 0) {
			edgeHolder = new GameObject ();
			edgeHolder.transform.SetParent (ac.transform, false);
		} else {
			edgeHolder = ac.gameObject.transform.GetChild (0).gameObject;
		}
			
		Vector2[] edges = ac.getPoints(Vector2.zero);
		if (edgeHolder.GetComponents<EdgeCollider2D> ().Length == 0) {

			for (int i = 0; i < edges.Length - 1; i++) {
				EdgeCollider2D ec = edgeHolder.AddComponent<EdgeCollider2D> ();
				Vector2[] points = new Vector2[2];
				points [0] = edges [i];
				points [1] = edges [i + 1];
				ec.points = points;
			}
		}
    }

	void OnDisable()
	{
		if (target == null) {
			DestroyImmediate(edgeHolder);
		}
	}


    public override void OnInspectorGUI()
    {
		GUI.changed = false;
		DrawDefaultInspector();

		Vector2[] edges = ac.getPoints(Vector2.zero);

		//Check if we need to add/remove colliders
		if (edgeHolder.GetComponents<EdgeCollider2D> ().Length != edges.Length - 1) {

			EdgeCollider2D[] oldColliders = edgeHolder.GetComponents<EdgeCollider2D> ();
			for (int k = 0; k < oldColliders.Length; k++) {
				DestroyImmediate(oldColliders[k]);
			}

			for (int i = 0; i < edges.Length - 1; i++) {
				EdgeCollider2D ec = edgeHolder.AddComponent<EdgeCollider2D> ();

				Vector2[] points = new Vector2[2];
				points [0] = edges [i];
				points [1] = edges [i + 1];
				ec.points = points;

			}
		} 
		else {
			EdgeCollider2D[] ecs = edgeHolder.GetComponents<EdgeCollider2D> ();

			for (int i = 0; i < ecs.Length; i++) {

				EdgeCollider2D ec = ecs [i];

				Vector2[] points = new Vector2[2];
				points [0] = edges [i];
				points [1] = edges [i + 1];
				ec.points = points;

			}
		}
   }
}