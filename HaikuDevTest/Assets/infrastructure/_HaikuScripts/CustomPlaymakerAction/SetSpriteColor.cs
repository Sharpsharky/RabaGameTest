
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Color)]
    [Tooltip("Sets the RGBA channels of a Color Variable. To leave any channel unchanged, set variable to 'None'.")]
    public class SetSpriteColor : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;

        [HasFloatSlider(0, 1)]
        public FsmFloat red;

        [HasFloatSlider(0, 1)]
        public FsmFloat green;

        [HasFloatSlider(0, 1)]
        public FsmFloat blue;

        [HasFloatSlider(0, 1)]
        public FsmFloat alpha;

        public bool everyFrame;

        public override void Reset()
        {
            gameObject = null;

            // default to variable dropdown with None selected.

            red = 0;
            green = 0;
            blue = 0;
            alpha = 1;

            everyFrame = false;
        }

        public override void OnEnter()
        {
            DoSetColorRGBA();

            if (!everyFrame)
                Finish();
        }

        public override void OnUpdate()
        {
            DoSetColorRGBA();
        }

        void DoSetColorRGBA()
        {
            // Get user choice
            GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
            // Check
            if (go == null) return;

            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            if (!spriteRenderer)
            {
                Debug.LogWarning("SpriteRenderer not found for " + go.name);
                return;
            }

            var newColor = spriteRenderer.color;

            if (!red.IsNone)
            {
                newColor.r = red.Value;
            }

            if (!green.IsNone)
            {
                newColor.g = green.Value;
            }

            if (!blue.IsNone)
            {
                newColor.b = blue.Value;
            }

            if (!alpha.IsNone)
            {
                newColor.a = alpha.Value;
            }

            spriteRenderer.color = newColor;
        }
    }
}