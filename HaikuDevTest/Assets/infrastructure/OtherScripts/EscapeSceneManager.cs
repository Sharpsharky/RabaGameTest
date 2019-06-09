using UnityEngine;
using System.Collections;

public class EscapeSceneManager : MonoBehaviour {

	public GameObject SceneManager = null;

	public void Serialize ()
	{
	}

	public void DeSerialize()
	{
		Destroy (SceneManager);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
