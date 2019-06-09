using UnityEngine;
using System.Collections;
using UnityEditor;

public class TakeScreenShot : MonoBehaviour {

	[MenuItem("Window/Take screenshot")]
	static void Screenshot()
	{
		ScreenCapture.CaptureScreenshot("test.png");
	}
}
