using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Sends a task complete to the hint panel.")]
	public class FocusRoom : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Room ID")]
		public FsmInt roomID;

		public override void Reset()
		{
			roomID = 0;
		}
		
		public override void OnEnter()
		{
			ChapterSceneManager.instance.FocusRoom(roomID.Value);
			Finish();
		}
	}
}