using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("_Common")]
    [Tooltip("Fades gameobject with text mesh pro component")]
    public class FadeTextMeshProText : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;
        [Tooltip("true = fade in, false = fade out")]
        public FsmBool fadeIn;

        [Tooltip("true = fade from current alpha value of a sprite renderer, false = fade from 1 or 0")]
        public FsmBool fadeFromCurrentAlphaValue;

        public FsmFloat fadeTime;

        public FsmFloat delay;

        public FsmBool includeChildrean;

        public FsmBool includeInactive;

        public FsmEvent finishedEvent;

        GameObject go;
        private TextMeshPro sprite;  // This will be taken via GetComponent from a given gameobject

        private Coroutine fadeCoroutine;    // Used to stop a previous coroutine if any to don't have two fadings going on on the same object

        private TextMeshPro[] childrean;

        public override void Reset()
        {
            fadeIn = false;
            fadeTime = 0f;
            delay = 0f;
            finishedEvent = null;
            go = null;
            includeChildrean = false;
            includeInactive = false;
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
            sprite = go.GetComponent<TextMeshPro>();

            // Take kids
            if (includeChildrean.Value)
            {
                childrean = go.GetComponentsInChildren<TextMeshPro>(includeInactive.Value);
            }

            // Check is sprite taken
            if (sprite != null || (childrean != null && childrean.Length > 0))
            {
                // Set start and stop alpha
                float startAlpha = 0;
                float stopAlpha = 1;
                if (!fadeIn.Value)
                {
                    startAlpha = 1;
                    stopAlpha = 0;
                }
                if (fadeFromCurrentAlphaValue.Value)
                {
                    if (sprite != null)
                    {
                        startAlpha = sprite.color.a;
                    }
                }
                // Start fade in/out
                fadeCoroutine = StartCoroutine(FadeAlphaValue(startAlpha, stopAlpha));
            }
            // If there is no sprite renderer component at the gameobject
            else
            {
                Debug.LogWarning("Text Mesh pro not found at " + go.name);
                OnFadeComplete();
            }
            // Finish
            Finish();
        }

        IEnumerator FadeAlphaValue(float startAlpha, float stopAlpha)
        {
            // Calculate values
            var timeStep = 0.01f;
            var wait = new WaitForSeconds(timeStep);           
            var alpha = startAlpha;
            //var steps = fadeTime.Value / timeStep;
            //var oneStepValue = (startAlpha - stopAlpha) / -steps;

            // Delay
            if (delay.Value > 0)
                yield return new WaitForSeconds(delay.Value);

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
            if (sprite != null)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            }
            if (includeChildrean.Value && childrean != null)
            {
                if (childrean.Length > 0)
                {
                    foreach (var child in childrean)
                    {
                        if (child != null)
                        {
                            child.color = new Color(child.color.r, child.color.g, child.color.b, alpha);
                        }
                    }
                }
            }
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
