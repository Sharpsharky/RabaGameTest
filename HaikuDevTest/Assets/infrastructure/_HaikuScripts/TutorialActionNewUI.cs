using UnityEngine;

using HutongGames.PlayMaker;
using HutongGames.Utility;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	public class TutorialActionNewUI : FsmStateAction {

		[Tooltip("Game Object for tutorial")]
		public FsmGameObject targetGO;
		[Tooltip("Key for tutorial")]
		public FsmString localizationKey;
		public FsmString sheet = "Sheet1";
		public FsmBool isLeft;
		[Tooltip("If turn back on is set to true, ignore the collider and turn on all previously disabled colliders")]
		public float fixedZ;

		public override void Reset()
		{
			targetGO = null;
		}

		public override void OnEnter()
		{
			GameObject tutorialObject = GameObject.FindGameObjectWithTag ("TutorialPartialScreenBlocker");
			string key = sheet + "/" + localizationKey;
			string translation = Helper.GetKey(key);

			Vector3 targetPosition = targetGO.Value.transform.position;

			Dictionary<string, object> dict = new Dictionary<string, object> () {
				{ "tutorialText", (object) translation},
				{ "isLeft", (object)isLeft.Value},
				{"targetPosition", (object)targetPosition}
			};
			SetEventProperties.properties = dict;
			tutorialObject.GetComponent<PlayMakerFSM>().SendEvent("activate");
		}
	}
}
