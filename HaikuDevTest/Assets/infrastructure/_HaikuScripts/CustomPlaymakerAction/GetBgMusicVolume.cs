using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Music")]
	[Tooltip("Gets float volume of MusicPlayer.")]
	public class GetBgMusicVolume : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeValue;

		public override void Reset()
		{
			storeValue = 0f;
		}

		public override void OnEnter()
		{
			GetVolume ();
			Finish();
		}

		void GetVolume()
		{
            MusicPlayer musicPlayer = MusicPlayer.instance;

            if (musicPlayer != null) {

                if (musicPlayer != null) {
                    storeValue.Value = musicPlayer.volume;
                }
            } else {
                Debug.Log("No music player in scene!");
            }

			
		}
	}
}
