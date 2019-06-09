using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.SceneManagement;

public class DebugShortcutKeys : MonoBehaviour {
    private ChapterSceneManager _sceneManager;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (_sceneManager == null) {
            _sceneManager = ChapterSceneManager.instance;
            return;
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            _sceneManager.FocusRoom(1);
        } else if (Input.GetKeyDown(KeyCode.F2)) {
            _sceneManager.FocusRoom(2);
        } else if (Input.GetKeyDown(KeyCode.F3)) {
            _sceneManager.FocusRoom(3);
        } else if (Input.GetKeyDown(KeyCode.F4)) {
            _sceneManager.FocusRoom(4);
        } else if (Input.GetKeyDown(KeyCode.F5)) {
            _sceneManager.FocusRoom(5);
        } else if (Input.GetKeyDown(KeyCode.F6)) {
            _sceneManager.FocusRoom(6);
        } else if (Input.GetKeyDown(KeyCode.F7)) {
            _sceneManager.FocusRoom(7);
        } else if (Input.GetKeyDown(KeyCode.F8)) {
            _sceneManager.FocusRoom(8);
        } else if (Input.GetKeyDown(KeyCode.F9)) {
            _sceneManager.FocusRoom(9);
        } else if (Input.GetKeyDown(KeyCode.F10)) {
            _sceneManager.FocusRoom(10);
        } else if (Input.GetKeyDown(KeyCode.F11)) {
            _sceneManager.FocusRoom(11);
        } else if (Input.GetKeyDown(KeyCode.F12)) {
            _sceneManager.FocusRoom(12);
        } else if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ChapterUIManager.instance.ForceSelectItemId(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ChapterUIManager.instance.ForceSelectItemId(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            ChapterUIManager.instance.ForceSelectItemId(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            ChapterUIManager.instance.ForceSelectItemId(4);
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            ChapterUIManager.instance.ForceSelectItemId(5);
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            ChapterUIManager.instance.ForceSelectItemId(6);
        } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            ChapterUIManager.instance.ForceSelectItemId(7);
        } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            ChapterUIManager.instance.ForceSelectItemId(8);
        } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            ChapterUIManager.instance.ForceSelectItemId(9);
        } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
            ChapterUIManager.instance.ForceSelectItemId(10);
        } else if (Input.GetKeyDown(KeyCode.Minus)) {
            ChapterUIManager.instance.ForceSelectItemId(11);
        } else if (Input.GetKeyDown(KeyCode.Equals)) {
            ChapterUIManager.instance.ForceSelectItemId(12);
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject.Find("ConversationCanvas").GetComponent<PlayMakerFSM>().SendEvent("deactivate");
        } else if (Input.GetKeyDown(KeyCode.L)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }else if (Input.GetKeyDown(KeyCode.K)) {

        }else if (Input.GetKeyDown(KeyCode.S)) {

        }
	} 
}
