// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Stop an iTween action.")]
	public class iTweenStop : FsmStateAction
	{
		public FsmOwnerDefault gameObject;
		public FsmString id;
		public iTweenFSMType iTweenType = iTweenFSMType.all;
		public bool includeChildren;
		
		public override void Reset()
		{
			id = new FsmString{UseVariable = true};
			iTweenType = iTweenFSMType.all;
			includeChildren = false;
            gameObject = null;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			DoiTween();
			Finish();
		}
							
		void DoiTween()
		{
            if (id.IsNone) {
                GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
                if(go == null){
                    if (iTweenType == iTweenFSMType.all) {
                        iTween.Stop();
                    }else{
                        iTween.Stop(System.Enum.GetName(typeof(iTweenFSMType), iTweenType));
                    }
                }else{
                    if (iTweenType == iTweenFSMType.all) {
                        iTween.Stop(go,includeChildren);
                    } else {
                        iTween.Stop(go, System.Enum.GetName(typeof(iTweenFSMType), iTweenType), includeChildren);
                    }
                }
            }else{
                iTween.StopByName(id.Value);
            }
		}
	}
}