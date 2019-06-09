using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Zoom in on a part of an object.")]
	public class ZoomInOnGameobject : FsmStateAction
	{

		public FsmOwnerDefault objectToZoomInto;
		public FsmVector3 scaleUpBy;
		public FsmFloat timeToHoldZoom;
		public FsmFloat animationTime;

		public iTween.EaseType easeType = iTween.EaseType.linear;

		private float startTime;
		
		public override void Reset()
		{

		}

		public override void OnEnter()
		{
			ZoomIn ();
			Finish();
		}

		void ZoomIn()
		{
			GameObject background = ChapterSceneManager.instance.currentRoom.gameObject;

			iTween.ScaleBy(background, iTween.Hash("amount", scaleUpBy.Value, "time", animationTime.Value, "easeType", easeType));

			Vector3 distanceToMove = background.transform.position -Fsm.GetOwnerDefaultTarget(objectToZoomInto).transform.position;

			distanceToMove.x *= scaleUpBy.Value.x;
			distanceToMove.y *= scaleUpBy.Value.y;
			distanceToMove.z *= scaleUpBy.Value.z;

			iTween.MoveBy(background, iTween.Hash("amount", distanceToMove, "time", animationTime.Value, "easeType", easeType));
		}

	}
}