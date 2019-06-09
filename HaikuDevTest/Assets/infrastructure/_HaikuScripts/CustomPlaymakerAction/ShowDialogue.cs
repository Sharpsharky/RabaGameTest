using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("_Common")]
	[Tooltip("Starts a conversation sequence")]
	public class ShowDialogue : FsmStateAction
	{
		[RequiredField]
		[Tooltip("SceneName")]
		public FsmString SceneName;


		[Tooltip("The fsm that will receive the conversation system events like won, failed events.")]
		public PlayMakerFSM EventHandlerFSM;

		[Tooltip("The back button that will be disabled when the conversation starts.")]
		public GameObject PuzzleBackButton;

		[Tooltip("That room that will be focused when back button is clicked.")]
		public int BackButtonRoom;

        [CompoundArray("Locked Choices", "Id", "Is Unlocked")]
        public FsmString[] lockedChoiceIds;
        public FsmBool[] isUnlocked;


		public override void Reset()
		{
			SceneName = "temp";
		}

		public override void OnEnter()
		{

            string[] lockedChoices = null;

            if(lockedChoiceIds.Length > 0){
                List<string> lockedChoicesList = new List<string>();
                for (int i = 0; i < lockedChoiceIds.Length; ++i) {
                    if (!isUnlocked[i].Value) {
                        lockedChoicesList.Add(lockedChoiceIds[i].Value);
                    }
                }
                lockedChoices = lockedChoicesList.ToArray();
            }
           
            Helper.TriggerConversation (SceneName.Value, EventHandlerFSM, PuzzleBackButton, BackButtonRoom, lockedChoices);
			Finish ();
		}
	}
}
