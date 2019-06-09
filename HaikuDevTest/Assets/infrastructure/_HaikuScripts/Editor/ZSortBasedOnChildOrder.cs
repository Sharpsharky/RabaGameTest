using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ZSortBasedOnChildOrder{

	[MenuItem("GameObject/Haiku/Z-Sort children based on child order", false, 10)]
	public static void ZSortChildren(){
		Transform[] selectedTransforms = Selection.GetTransforms (SelectionMode.TopLevel | SelectionMode.Editable);

		SpriteRenderer[] spriteRenderers;



		const float SPACE_BETWEEN_LAYERS = 0.1f;

	
		float z;
		Vector3 localPos;

		foreach (Transform transform in selectedTransforms) {
			z = -SPACE_BETWEEN_LAYERS;
			spriteRenderers = transform.gameObject.GetComponentsInChildren<SpriteRenderer> (true); 

			foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
				localPos = transform.InverseTransformPoint (spriteRenderer.transform.position);
				localPos.z = z;
				spriteRenderer.transform.position = transform.TransformPoint (localPos);
				z -= SPACE_BETWEEN_LAYERS;
			}
		}
	}


}
