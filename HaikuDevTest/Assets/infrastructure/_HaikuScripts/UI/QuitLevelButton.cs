using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitLevelButton : MonoBehaviour,IPointerClickHandler{

    [SerializeField]
    AudioClip _tapSound;

    public void OnPointerClick(PointerEventData pEventData) {
        if (_tapSound != null) {
            Helper.PlayAudioIfSoundOn(_tapSound);
        }

        ChapterUIManager.instance.QuitChapter();
    }
}
