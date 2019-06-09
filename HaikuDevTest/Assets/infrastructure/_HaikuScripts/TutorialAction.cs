using UnityEngine;

using HutongGames.PlayMaker;
using HutongGames.Utility;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	public class TutorialAction : FsmStateAction {
		
		[Tooltip("Game Object for tutorial")]
		public FsmGameObject targetGO;
		[Tooltip("Key for tutorial")]
		public FsmString localizationKey;
		public FsmString sheet = "Sheet1";
		public FsmBool isLeft;
		[Tooltip("If turn back on is set to true, ignore the collider and turn on all previously disabled colliders")]
		public GameObject tutorialObject;
		public float fixedZ;

		public override void Reset()
		{
			targetGO = null;
		}
		
		public override void OnEnter()
		{
			Vector3 targetPosition = new Vector3(targetGO.Value.transform.position.x, targetGO.Value.transform.position.y, fixedZ);
			GameObject tutorialObjectCopy = (GameObject)GameObject.Instantiate(tutorialObject, new Vector3(0f, 0f, 0f), Quaternion.identity);

			string key = sheet + "/" + localizationKey;
			string translation = Helper.GetKey(key);

			Dictionary<string, object> dict = new Dictionary<string, object> () {
				{ "tutorialText", (object) translation},
				{ "isLeft", (object)isLeft.Value},
				{ "targetPosition", (object)targetPosition},
			};
			SetEventProperties.properties = dict;
			tutorialObjectCopy.GetComponent<PlayMakerFSM>().SendEvent("activate");
		}
	}
}
