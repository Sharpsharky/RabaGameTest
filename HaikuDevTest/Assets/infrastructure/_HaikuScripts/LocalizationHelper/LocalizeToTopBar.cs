using UnityEngine;

using HutongGames.PlayMaker;
using HutongGames.Utility;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
		[ActionCategory(ActionCategory.String)]
		public class LocalizeToTopBar : FsmStateAction {

		[RequiredField]
		[Tooltip("I2 Localization Key")]
		public FsmString localizationKey;
		[Tooltip("Sheet if not default")]
		public FsmString sheet = "Sheet1";

        public FsmBool useDoubleWide = false;
        [Tooltip("Don't show the text if the Language is set to English (useful for translating text already in English)")]
        public FsmBool dontShowIfEnglish = false;

        [Tooltip("By default tapping anywhere closes the BBT instead of triggering interactions. Set this to true to prevent blocking taps.")]
        public FsmBool disableTapBlocker;

		public override void Reset()
		{
			localizationKey = "";
		}
		
		public override void OnEnter()
		{
			TextToTopBar ();
			Finish();
		}
		
		void TextToTopBar()
		{
			if (dontShowIfEnglish.Value) {
				if (Helper.IsEnglish()) return;
			}

            Helper.LocalizeKeyToTopBar (localizationKey.Value, sheet.Value, useDoubleWide.Value, disableTapBlocker.Value);
		}
	}
}
