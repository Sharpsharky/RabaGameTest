using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {
	public static bool IS_TUTORIAL_COMPLETE = true;

	[Header("Assign these values in inspector.")]
	public string _tutorialCompletePlayerPrefsKey;



	//-- Right now we are checking if the scene save file exists. If it does and the tutorial is not complete
	//-- then we delete the saved scene file. This happens only in the scene with the tutorial.
	void Awake () {
		if (!PlayerPrefs.HasKey (_tutorialCompletePlayerPrefsKey)) {
			IS_TUTORIAL_COMPLETE = false;
		} else {
			IS_TUTORIAL_COMPLETE = true;
		}
	}

	IEnumerator WaitForNextFrame () {
		yield return new WaitForEndOfFrame ();
	}

	public void SetTutorialCompleted () {
		IS_TUTORIAL_COMPLETE = true;
	}
}
