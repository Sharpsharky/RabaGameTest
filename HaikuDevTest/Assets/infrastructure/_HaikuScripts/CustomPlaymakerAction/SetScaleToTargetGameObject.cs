// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved. Upgraded by RabaGames for Haiku 2018

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Sets the Scale of a Game Object. To leave any axis unchanged, set variable to 'None'.")]
	public class SetScaleToTargetGameObject : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to scale.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Use stored Vector3 value, and/or set each axis below.")]
		public FsmVector3 vector;

        [Tooltip("Target gameobject")]
        public FsmOwnerDefault gameObjectTarget;

        public FsmFloat x;
		public FsmFloat y;
		public FsmFloat z;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;	

		public override void Reset()
		{
			gameObject = null;
            gameObjectTarget = null;
            vector = null;
			// default axis to variable dropdown with None selected.
			x = new FsmFloat { UseVariable = true };
			y = new FsmFloat { UseVariable = true };
			z = new FsmFloat { UseVariable = true };
			everyFrame = false;
			lateUpdate = false;
		}

		public override void OnEnter()
		{
			DoSetScale();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}

		public override void OnUpdate()
		{
			if (!lateUpdate)
			{
				DoSetScale();
			}
		}

		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
				DoSetScale();
			}

			if (!everyFrame)
			{
				Finish();
			}
		}

		void DoSetScale()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
            var tgo = Fsm.GetOwnerDefaultTarget(gameObjectTarget);

            Vector3 scale;
            if (vector.IsNone)
            {
                if (tgo != null)
                {
                    scale = tgo.transform.localScale;
                }
                else
                {
                    scale = go.transform.localScale;
                }
            }
            else
            {
                scale = vector.Value;
            }
			
			if (!x.IsNone) scale.x = x.Value;
			if (!y.IsNone) scale.y = y.Value;
			if (!z.IsNone) scale.z = z.Value;
			
			go.transform.localScale = scale;
		}
	}
}