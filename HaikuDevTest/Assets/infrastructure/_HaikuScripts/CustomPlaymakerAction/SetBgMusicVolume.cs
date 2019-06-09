using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Music")]
	[Tooltip("Sets volume of MusicPlayer.")]
	public class SetBgMusicVolume : FsmStateAction
	{
		[RequiredField]
		public FsmFloat volume = 0f;

		public override void Reset()
		{
			volume = 0f;
		}

		public override void OnEnter()
		{
			SetVolume ();
			Finish();
		}

		void SetVolume()
		{
            MusicPlayer musicPlayer = MusicPlayer.instance;

            if (musicPlayer != null) {
				
				if (musicPlayer != null) {
                    musicPlayer.SetVolume(volume.Value);
				}
			} else {
				Debug.Log("No music player in scene!");
			}
		}
	}
}
