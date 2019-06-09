using UnityEngine;
using System.Collections;

public enum JarType {
	none, ox, snake, croc, hawk
};

public class SarcophagusJar : MonoBehaviour {

	public JarType type;
	public SarcophagusQuadrant currentQuadrant;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () { }
}
