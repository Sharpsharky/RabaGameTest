using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {
	public GameObject menuButtonCamera;
	public AudioClip menuSound;

    // Use this for initialization
    void Start () {
		HideOrShowUI ();
	}

    void OnEnable(){
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
		HideOrShowUI();
	}
	
	// Update is called once per frame
	private void HideOrShowUI() {
        string sceneName = SceneManager.GetActiveScene().name;
		if (sceneName == "Title" ||
//		    sceneName == "Temp Select Act" ||
//		    sceneName == "Level1b Barbican" ||
		    sceneName == "ActComplete" ||
		    sceneName == "leaveReview" ||
		    sceneName == "cultIntro" ||
		    sceneName == "SelectAct" ||
		    sceneName == "Credits") {
			HideUI();
		} else {
			ShowUI ();
		}
	}
	
	private void HideUI() {
		SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer renderer in renderers) {
			renderer.enabled = false;
		}
		Collider2D collider = gameObject.GetComponent<Collider2D> ();
		collider.enabled = false;
	}
	
	private void ShowUI() {
		Collider2D collider = gameObject.GetComponent<Collider2D> ();
		collider.enabled = true;
		
		SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer renderer in renderers) {
			renderer.enabled = true;
		}
	}

	void OnMouseUp() {
		Debug.Log("on mouse down in menu button");
		Helper.PlayAudioIfSoundOn(menuSound);
		Instantiate (menuButtonCamera);
	}
}
