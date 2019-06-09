using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("_Common")]
    [Tooltip("Fades gameobject with sprite component")]
    public class FadeSpriteToValue : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;

        [HasFloatSlider(0, 1)]
        public FsmFloat toValue;

        [HasFloatSlider(0, 1)]
        public FsmFloat fromValue;

        [Tooltip("true = fade from current alpha value of a sprite renderer, false = fade from 1 or 0")]
        public FsmBool fadeFromCurrentAlphaValue;

        public FsmFloat fadeTime;

        public FsmFloat delay;

        public FsmEvent finishedEvent;

        GameObject go;
        private SpriteRenderer sprite;  // This will be taken via GetComponent from a given gameobject

        private Coroutine fadeCoroutine;    // Used to stop a previous coroutine if any to don't have two fadings going on on the same object

        public override void Reset()
        {
            fromValue = 0f;
            toValue = 0f;
            fadeTime = 0f;
            delay = 0f;
            finishedEvent = null;
            go = null;
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        }

        public override void OnEnter()
        {
            // stop previous coroutine if any
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

            // Get user choice
            go = Fsm.GetOwnerDefaultTarget(gameObject);
            // Check
            if (go == null) return;

            // Take sprite
            sprite = go.GetComponent<SpriteRenderer>();
            // Check is sprite taken
            if (sprite != null)
            {
                // Set start and stop alpha
                float startAlpha = 0;
                float stopAlpha = 1;

                
                stopAlpha = toValue.Value;

                if (fadeFromCurrentAlphaValue.Value)
                {
                    startAlpha = sprite.color.a;
                }
                else
                {
                    startAlpha = fromValue.Value;
                }
                // Start fade in/out
                fadeCoroutine = StartCoroutine(FadeAlphaValue(startAlpha, stopAlpha));
            }
            // If there is no sprite renderer component at the gameobject
            else
            {
                Debug.LogWarning("Sprite renderer not found at " + go.name);
                OnFadeComplete();
            }
            // Finish
            Finish();
        }

        IEnumerator FadeAlphaValue(float startAlpha, float stopAlpha)
        {
            // Delay
            if (delay.Value > 0)
                yield return new WaitForSeconds(delay.Value);

            // Calculate values
            var timeStep = 0.01f;
            var wait = new WaitForSeconds(timeStep);
            var alpha = startAlpha;
            //var steps = fadeTime.Value / timeStep;
            //var oneStepValue = (startAlpha - stopAlpha) / -steps;

            // Fade
            //for (int i = 0; i < steps; i++)
            //{
            //    yield return wait;
            //    alpha += oneStepValue;
            //    UpdateAlpha(alpha);
            //}

            float addValue = stopAlpha - startAlpha;
            float lastTime = Time.realtimeSinceStartup;
            float startTime = lastTime;
            bool finish = false;
            while (true)
            {
                float progress = Time.realtimeSinceStartup - lastTime;
                lastTime = Time.realtimeSinceStartup;

                alpha += (progress / fadeTime.Value) * addValue;

                if (addValue > 0)
                {
                    if (alpha > stopAlpha)
                    {
                        alpha = stopAlpha;
                        finish = true;
                    }
                }
                else
                {
                    if (alpha < stopAlpha)
                    {
                        alpha = stopAlpha;
                        finish = true;
                    }
                }

                if ((lastTime - startTime) > fadeTime.Value)
                {
                    alpha = stopAlpha;
                    finish = true;
                }

                // Update alpha
                UpdateAlpha(alpha);

                if (finish)
                    break;

                yield return wait;
            }

            // Fade completed, sent event and finish
            OnFadeComplete();
        }

        /// <summary>
        /// Updates alpha value
        /// </summary>
        /// <param name="alpha"></param>
        public void UpdateAlpha(float alpha)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        }

        private void OnFadeComplete()
        {
            if (finishedEvent != null)
            {
                Fsm.Event(finishedEvent);
                Finish();
            }
        }
    }
}
