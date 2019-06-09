using UnityEngine;

//ToDo Azhar. Replaced the SetVisibility in PlayMaker/Scripts with this one to recursively activate the colliders and renderers.
namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Sets or toggle the visibility on a game object.")]
	public class SetVisibityWithColliders : FsmStateAction
	{
//		[RequiredField]
		//[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		//[UIHint(UIHint.Variable)]
		[Tooltip("Should the object visibility be toggled?\nHas priority over the 'visible' setting")]
		public FsmBool toggle;
		//[UIHint(UIHint.Variable)]
		[Tooltip("Should the object be set to visible or invisible?")]
		public FsmBool visible;
		[Tooltip("Resets to the initial visibility once\nit leaves the state")]
		public FsmBool collidersAsWell = true;
		[Tooltip("Get colliders")]
		public bool resetOnExit;
		public bool recursive;
		private bool initialVisibility;
		
		public override void Reset()
		{
			gameObject = null;
			toggle = false;
			visible = false;
			resetOnExit = false;
			initialVisibility = false;
			recursive = true;
		}
		
		public override void OnEnter()
		{
			if (gameObject.OwnerOption == OwnerDefaultOption.UseOwner)
				DoSetVisibility(Owner);
			else
				DoSetVisibility(gameObject.GameObject.Value);
			
			Finish();
		}
		
		void DoSetVisibility(GameObject go)
		{
			
			if (go == null)
				return;
			
			var renderers = go.GetComponentsInChildren<Renderer> ();
			var colliders = go.GetComponentsInChildren<Collider2D>(); // TODO: fix collider
			
			// 'memorizes' initial visibility
			bool isRootRenderer = (go.GetComponent<Renderer> () != null);
			
			if (isRootRenderer) 
				initialVisibility = go.GetComponent<Renderer>().isVisible;
			else
				initialVisibility = false;                
			
			// if 'toggle' is not set, simply sets visibility to new value
			if (toggle.Value == false) {
				if (isRootRenderer) {
					go.GetComponent<Renderer>().enabled = visible.Value;
					if (go.GetComponent<Renderer>().GetComponent<Collider2D>() && collidersAsWell.Value) {
						go.GetComponent<Renderer>().GetComponent<Collider2D>().enabled = visible.Value;
					}
				}
				
				if (!recursive)
					return;
				foreach (Renderer childRenderer in renderers) {
					childRenderer.enabled = visible.Value;
				}
				if (collidersAsWell.Value) {
					foreach (Collider2D childCollider in colliders) {
						childCollider.enabled = visible.Value;
					}
				}
				return;
			}
			
			// otherwise, toggles the visibility
			if (isRootRenderer) {
				Debug.Log ("Go render enabled:" + go.GetComponent<Renderer>().enabled);
				go.GetComponent<Renderer>().enabled = !go.GetComponent<Renderer>().enabled;
				if (go.GetComponent<Renderer>().GetComponent<Collider2D>() && collidersAsWell.Value) {
					go.GetComponent<Renderer>().GetComponent<Collider2D>().enabled = go.GetComponent<Renderer>().enabled;
				}
			
				if (!recursive)
					return;
				foreach (Renderer childRenderer in renderers) {
					childRenderer.enabled = go.GetComponent<Renderer>().enabled;
				}
				foreach (Collider2D childCollider in colliders) {
					childCollider.enabled = go.GetComponent<Renderer>().enabled;
				}
				return;
			}
		}
		
		public override void OnExit()
		{
			if (resetOnExit)
				ResetVisibility();
		}
		
		void ResetVisibility()
		{
			// uses the FSM to get the target object and resets its visibility
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null && go.GetComponent<Renderer>() != null)
				go.GetComponent<Renderer>().enabled = initialVisibility;

		}
		
	}
}