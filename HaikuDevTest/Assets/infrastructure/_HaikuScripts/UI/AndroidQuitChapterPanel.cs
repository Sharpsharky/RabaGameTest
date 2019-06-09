using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidQuitChapterPanel : MonoBehaviour {
    
    public bool OnAndroidBack() {
        if(gameObject.activeSelf){
            OnClose();
            return true;
        }
        return false;
    }

    public void OnClose() {
        gameObject.SetActive(false);
    }

    public void OnQuit() {
        if (ChapterUIManager.instance != null) {
            ChapterUIManager.instance.QuitChapter();
        }
    }
}
