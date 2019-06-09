using UnityEngine;


namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	public class LocalizeAction : FsmStateAction {

		[RequiredField]
		[Tooltip("I2 Localization Key")]
		public FsmString localizationKey;
		[Tooltip("Minor Hint")]
		public FsmString outPutString;
		public FsmString sheet = "Sheet1";
		
		public override void Reset()
		{
			localizationKey = "";
			outPutString = "";
		}
		
		public override void OnEnter()
		{
			LocalizeText ();
			Finish();
		}
		
		void LocalizeText()
		{
			string key = sheet + "/" + localizationKey.Value; // Build key from sheet + term name
//			Debug.Log("Getting key at " + key);
			// NOTE: Remember that I2 Localization prefab must be in the scene if we are having errors with this 
			outPutString.Value = Helper.GetKey(key);
            
//			Debug.Log("Translation is " + ScriptLocalization.Get(key));
		}
	}
}