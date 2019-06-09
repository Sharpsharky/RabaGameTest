using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HutongGames.PlayMaker;
using HutongGames.Utility;

public class Helper : MonoBehaviour {





    // TEMPORARY LIST OF LOCALIZATION KEYS MADE ONLY FOR TEST PURPOSES
    public static string GetKey(string localizationKey)
    {
        if (localizationKey.Contains("BBT_START")) return "Starting room - to show how text works.";
        if (localizationKey.Contains("CH6_WALL1")) return "Holes in the wall...?";
        if (localizationKey.Contains("CH6_TESTEND")) return "I'm going to the other room, bye! (END OF TEST THANK YOU).";
        if (localizationKey.Contains("CH6_CROSSBOW")) return "Why killing animals anyway...";
        if (localizationKey.Contains("CH6_GREEN_COATS")) return "I don't think there is a clue on this one.";
        if (localizationKey.Contains("CH6_AMMO")) return "I see ammo but no gun in this room...";
        if (localizationKey.Contains("CH6_CASE")) return "Nothing interesting.";
        if (localizationKey.Contains("CH6_CAMO_COAT1")) return "The pattern looks incomplete.";
        if (localizationKey.Contains("CH6_CAMO_COAT2")) return "I should probably paint it somehow.";
        if (localizationKey.Contains("CH6_CAMO_COAT3")) return "I'm almost sure it means something.";
        if (localizationKey.Contains("CH6_CAMO_TILES")) return "3 and 5, ok I see it!";
        if (localizationKey.Contains("CH6_SAFE_CLUE")) return "A coded message.";
        if (localizationKey.Contains("CH6_PHOTO_FRANK")) return "Ehhh...";
        if (localizationKey.Contains("CH6_CAMERA_LENS")) return "Green, blue, red.";
        if (localizationKey.Contains("CH6_CAMERA")) return "Nice, I'd like to borrow this camera.";
        if (localizationKey.Contains("CH6_ITEM_CAMO_PAINT")) return "Camo paint";
        if (localizationKey.Contains("CH6_ITEM_STENCIL")) return "Stencil";
        if (localizationKey.Contains("CH6_ITEM_RED_ARROW")) return "Red arrow";
        if (localizationKey.Contains("CH6_ITEM_BLUE_ARROW")) return "Blue arrow";
        if (localizationKey.Contains("CH6_ITEM_GREEN_ARROW")) return "Green arrow";
        if (localizationKey.Contains("CH6_ITEM_YELLOW_ARROW")) return "Yellow arrow";


        return "No text for this localization key.";
    }

















    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void ShowOrHideTapBlocker(bool blockerOn)
    {
        ChapterUIManager.instance.EnableTapBlocker(blockerOn);

    }

    public static void SendHintComplete(string hintString, bool loadingSavedHints = false) {


	}

	public static bool IsRightUI () {
		float aspectRatio = (float)Screen.width / (float)Screen.height;

		const float MINIMUM_RIGHT_ASPECT_RATIO = 5f / 3f; 

		if (aspectRatio >= MINIMUM_RIGHT_ASPECT_RATIO) {
			return true;
		} else {
			return false;
		}
	}

	public static void PlayAudioIfSoundOn(AudioClip clip) {
		if (clip != null) {
			PlayAudioIfSoundOn(clip, 1.0f);
		}
	}

	public static void PlayAudioIfSoundOn(AudioClip clip, float volume) {
		if (PlayerPrefs.HasKey(Constants.kSoundPref)) {
			if (PlayerPrefs.GetInt(Constants.kSoundPref) == 0) {
				Debug.Log("Not playing sound");
				return;
			}
		}
		if (clip != null) {
			AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
		}
	}

	public static bool IsEnglish() {
        return true;
	}

	public static string MatrixToString(bool[,] matrix) {
		string text = "";
		for (int j = matrix.GetLength(1)-1; j >= 0; j--) {
			for (int i = 0; i < matrix.GetLength(0); i++) {
				text = text + (matrix[i,j] ? "0" : "1") + " ";
			}
			text += "\n";
		}
		return text;
	}

	public static string MatrixToString(int[,] matrix) {
		string text = "";
		for (int i = 0; i < matrix.GetLength(0); i++) {
			for (int j = 0; j < matrix.GetLength(1); j++) {
				text = text + (matrix[i,j].ToString()) + " ";
			}
			text += "\n";
		}
		return text;
	}

	

	public static string LocalizeText(string localizationKey) {
		return LocalizeText(localizationKey, "Sheet1");
	}

	public static string LocalizeText(string localizationKey, string sheet) {
		if (string.IsNullOrEmpty(localizationKey)) {
			Debug.Log("Localization Key is empty: " + localizationKey);
			return "";
		}
		string key = sheet + "/" + localizationKey;
		string translation = GetKey(key);
		return translation;
	}


    

    public static void LocalizeKeyToTopBar(string localizationKey) {
        LocalizeKeyToTopBar(localizationKey, "Sheet1", false,false);
    }

    public static void LocalizeKeyToTopBar(string localizationKey, bool doubleWide) {
        LocalizeKeyToTopBar(localizationKey, "Sheet1", doubleWide,false);
    }


    public static void LocalizeKeyToTopBar(string localizationKey, string sheet) {
        LocalizeKeyToTopBar(localizationKey, sheet, false,false);
    }

    public static void LocalizeKeyToTopBar(string localizationKey, string sheet, bool doubleWide, bool disableTapBlocker) {
		string translation = LocalizeText(localizationKey, sheet);
		if (!string.IsNullOrEmpty(translation)) {
            ChapterUIManager.instance.ShowTopBarText (translation, doubleWide,disableTapBlocker);
		}
	}
		

	public static void PrintIntArray(int[] array) {
		string arrayString = "";
		for (int i = 0; i < array.Length; i++) {
			arrayString += array[i].ToString() + " ";
		}
		Debug.Log("Array: " + arrayString);
	}

	public static void TriggerConversation(string dialogueKey){
		TriggerConversation (dialogueKey, null, null, 0);
	}

	public static void TriggerConversation(string dialogueKey,PlayMakerFSM eventHandlerFSM, 
                                           GameObject backButton, int backRoomId, string[] lockedChoices = null){
        Debug.Log("Conversations disabled for dev test.");
	}
}
