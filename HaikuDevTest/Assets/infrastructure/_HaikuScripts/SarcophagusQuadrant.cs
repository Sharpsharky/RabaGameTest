using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum SarcophagusQuadrantType {
	topLeft, topRight, bottomLeft, bottomRight, center
};

public class SarcophagusQuadrant : MonoBehaviour {
	
	public SarcophagusQuadrantType type;
	public JarType correctJar;
	public JarType currentJar;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	// Public methods
}
