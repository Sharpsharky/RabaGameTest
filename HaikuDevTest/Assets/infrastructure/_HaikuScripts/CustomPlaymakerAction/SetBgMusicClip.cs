using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{

	[ActionCategory("Music")]
	[Tooltip("Force a change to MusicPlayer clip.")]
	public class SetBgMusicClip : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Clip")]

		[ObjectType(typeof(AudioClip))]
		public FsmObject clip;

		public override void Reset()
		{
			clip = null;
		}

		public override void OnEnter()
		{
			SetClip ();
			Finish();
		}

		void SetClip()
		{
            MusicPlayer musicPlayer = MusicPlayer.instance;

			if (musicPlayer != null) {
                musicPlayer.SetMusicTrack(clip.Value as AudioClip);
			} else {
				Debug.Log("No music player in scene!");
			}
		}
	}
}
