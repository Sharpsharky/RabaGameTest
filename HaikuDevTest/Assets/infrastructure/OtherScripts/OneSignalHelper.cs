/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OneSignalPush;

public class OneSignalHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		Debug.Log ("initializing one signal");

//		OneSignal.SetLogLevel (OneSignal.LOG_LEVEL.DEBUG, OneSignal.LOG_LEVEL.DEBUG);
		OneSignal.StartInit("8e67b66a-f2e0-4384-b970-fe3ef077e9f6")
			.HandleNotificationOpened(HandleNotificationOpened)
			.EndInit();
		// Call syncHashedEmail anywhere in your app if you have the user's email.
		// This improves the effectiveness of OneSignal's "best-time" notification scheduling feature.
		// OneSignal.syncHashedEmail(userEmail);
	}

	// Gets called when the player opens the notification.
	private static void HandleNotificationOpened(OSNotificationOpenedResult result) {		
	}
}*/
