using UnityEngine;
using System.Collections;

public class TraceToDraw : MonoBehaviour {
	public Transform DotPrefab;
	Vector3 lastDotPosition;
	bool lastPointExists;
	void Start()
	{
		lastPointExists = false;
	}
	void Update()
	{
		Debug.Log("Update check");
		if (Input.GetMouseButton(0))
		{
			Vector3 newDotPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Debug.Log("New dot position " + newDotPosition);
			if (newDotPosition != lastDotPosition)
			{
				Debug.Log("mak a dot");
				MakeADot(newDotPosition);
			}
		}
	}
	void MakeADot(Vector3 newDotPosition)
	{
		Instantiate(DotPrefab, newDotPosition, Quaternion.identity); //use random identity to make dots looks more different
		if (lastPointExists)
		{
			GameObject colliderKeeper = new GameObject("collider");
			BoxCollider bc = colliderKeeper.AddComponent<BoxCollider>();
			colliderKeeper.transform.position = Vector3.Lerp(newDotPosition, lastDotPosition, 0.5f);
			colliderKeeper.transform.LookAt(newDotPosition);
			bc.size = new Vector3(0.1f, 0.1f, Vector3.Distance(newDotPosition, lastDotPosition));
		}
		lastDotPosition = newDotPosition;
		lastPointExists = true;
	}
}
