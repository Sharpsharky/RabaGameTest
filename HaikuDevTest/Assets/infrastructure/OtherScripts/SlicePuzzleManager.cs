using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class SlicePuzzleManager : MonoBehaviour {

	public const float kRows = 8.0f;
	public const float kColumns = 6.0f;
	public const float kPhotoScaleFactor = 1.0f;
	private List<Transform> transformList;

	// Use this for initialization
	void OnMouseDown () {
		LayoutTiles ();
		PickRandom ();
	}

	private void LayoutTiles() {
		GameObject background = GameObject.Find ("0_Tower Basement");
		Sprite sp = background.GetComponent<SpriteRenderer>().sprite;
		Vector3 [] array = SpriteLocalToWorld(sp);

		float width = (array [1].x - array [0].x) * background.transform.localScale.x; // Take the length of the image in world coordinates and multiply by scale
		float height = (array [1].y - array [0].y) * background.transform.localScale.y;

		Debug.Log ("top left x " + array[0].x + " top left y " + array[0].y + " bottomright x: " + array[1].x + " bottom right y: " + array[1].y);

		Transform[] transforms = gameObject.GetComponentsInChildren<Transform> ();
		transformList = transforms.ToList ();
		transformList.Remove (gameObject.transform); // Remove this gameobject (the parent of the tiles)
//		transformList.Sort (CompareTransformByName);
		int row = 0;
		int column = (int)kColumns;
		for (int i = 0; i < transformList.Count; i++) {
			if (row >= kRows) {
				row = 0;
				column--;
			}
			Transform childTransform = transformList[i];

			// row/kRows is the percent of the way you have to move each tile.  Multiply by width since that is the full size of the image
			// If you just had this, you will start drawing right where the camera is since you are in world coordinates and starting at 0,0 so we need to offset.
			// + background.transform.position.x offsets the width from position of the background sprite to the camera 
			// - width /2 : moves you to the leftmost edge of the background sprite.
			childTransform.position = new Vector3(width * row / kRows + background.transform.position.x - width/2,
			                                      height * column/kColumns + background.transform.position.y - height / 2, 1);
			// layout the rows and columns.
//			Debug.Log("Transform name " + childTransform.name);
			row++;

//			Debug.Log(" Row " + row + " column " + column);
		}
	}

	Vector3[] SpriteLocalToWorld(Sprite sp) 
	{
		Vector3 pos = transform.position;
		Debug.Log("Sprite position " + pos);
		Vector3 [] array = new Vector3[2];
		//top left
		array[0] = pos + sp.bounds.min;
		// Bottom right
		array[1] = pos + sp.bounds.max;
		return array;
	}

	private void PickRandom() {
		int randomInt = UnityEngine.Random.Range (0, transformList.Count ());
		Transform randomTile = transformList[randomInt];
		Debug.Log ("RandomTile " + randomTile.name);
	}

	private static int CompareTransformByName(Transform i1, Transform i2)
	{
		char[] seperator = {'_'};
		string[] splitString1 = i1.name.Split (seperator);
		Debug.Log ("Splitstring " + splitString1 [0] + " " + splitString1 [1]);
		int item1int = Int32.Parse (splitString1 [1]);
		string[] splitString2 = i2.name.Split (seperator);
		int item2int = Int32.Parse (splitString2 [1]);
		return (item1int > item2int ? 1 : 0);
	}
}
