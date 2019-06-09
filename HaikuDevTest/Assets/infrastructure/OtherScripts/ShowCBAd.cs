/*using UnityEngine;
using System.Collections;
using ChartboostSDK;

public class ShowCBAd : MonoBehaviour {
	public bool onLoad;
	public bool onClick;

	// Use this for initialization
	
	void OnEnable() {
		
		// Initialize the Chartboost plugin
		//#if UNITY_ANDROID
		// Replace these with your own Android app ID and signature from the Chartboost web portal
		//if (HaikuBuildSettings.isGooglePlay) {
			//CBBinding.init("54725b3504b01601f110f710", "92b30e49fe5be79b2f500a737cbecbbcb4962d84");
		//} //else {
			//CBBinding.init("54725b6204b01601f85997ab", "94b6ba21a39e8f5e8e07a40624f7c67431e7a4bf");
		//}
		//#elif UNITY_IPHONE
		// Replace these with your own iOS app ID and signature from the Chartboost web portal
		//CBBinding.init("54722b7504b01602088e8186", "8dbf05bd9bad4de67a8ce8f37df11f2d51d62380");
		//#endif

	}

	#if UNITY_ANDROID || UNITY_IPHONE
	void Start () {
		if (onLoad) {
			Debug.Log("Showing on load cb ad");
			Chartboost.showInterstitial (CBLocation.MainMenu);
		}
	}

	void OnMouseDown() {
		if (onClick) {
			Debug.Log("Showing on click cb ad");
			Chartboost.showInterstitial(CBLocation.GameOver);
		}
	}
#endif
}*/
