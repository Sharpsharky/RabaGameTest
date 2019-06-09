using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Sets or toggle the visibility on a game object.")]
	public class SetColliders : FsmStateAction
	{
		[RequiredField]
		//[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;
		//[UIHint(UIHint.Variable)]
		[Tooltip("Turn colliders on or off")]
		public FsmBool collidersOn;
		[Tooltip("Get all the children colliders as well?")]
		public bool recursive;

		public override void Reset()
		{
			gameObject = null;
			collidersOn = false;
			recursive = true;
		}
		
		public override void OnEnter()
		{
			if (gameObject.OwnerOption == OwnerDefaultOption.UseOwner)
				DoSetColliders(Owner);
			else
				DoSetColliders(gameObject.GameObject.Value);
			
			Finish();
		}
		
		void DoSetColliders(GameObject go)
		{
			
			if (go == null)
				return;
			
			var colliders = go.GetComponentsInChildren<Collider2D> ();

			// if 'toggle' is not set, simply sets visibility to new value
			Collider2D ownerCollider = go.GetComponent<Collider2D> ();
			if (ownerCollider != null) {
				ownerCollider.enabled = collidersOn.Value;
			}
			
			if (!recursive)
				return;
			foreach (Collider2D childCollider in colliders) {
				childCollider.enabled = collidersOn.Value;
			}
		}
	}
}