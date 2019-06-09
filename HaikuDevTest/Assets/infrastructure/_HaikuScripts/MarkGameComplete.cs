using UnityEngine;
using System.Collections;
//using Escape13;


public class MarkGameComplete : MonoBehaviour {


	// Use this for initialization
	void Start () {

		GameComplete();
	}

	private void GameComplete() {
		//AchievementManager.CompleteAchievement(GPGSIds.achievement_heroes_heading_home);

		PlayerPrefs.SetInt("GameComplete", 1);

		// Mark mailchimp game complete
		Debug.Log("User Completed Game");


		//OneSignal.SendTag("GameComplete", "true");

	}
}
