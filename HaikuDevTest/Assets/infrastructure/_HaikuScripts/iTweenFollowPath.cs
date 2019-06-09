using UnityEngine;
using System.Collections;

public class iTweenFollowPath : MonoBehaviour {

	public Transform[] waypointArray;
	public float percentsPerSecond = 0.02f; // %2 of the path moved per second
	float currentPathPercent = 0.0f; //min 0, max 1

	void Update () 
	{
		if (currentPathPercent > 1.0f) return;
		currentPathPercent += percentsPerSecond * Time.deltaTime;
		iTween.PutOnPath(gameObject, waypointArray, currentPathPercent);
	}

}
