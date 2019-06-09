using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("_Common")]
    [Tooltip("Fades gameobject with sprite component")]
    public class FadeSpriteColorToValue : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;

        public FsmColor toValue;

        public FsmColor fromValue;

        [Tooltip("true = fade from current alpha value of a sprite renderer, false = fade from 1 or 0")]
        public FsmBool fadeFromCurrentColor;

        public FsmFloat fadeTime;

        public FsmFloat delay;

        public FsmEvent finishedEvent;

        GameObject go;
        private SpriteRenderer sprite;  // This will be taken via GetComponent from a given gameobject

        private Coroutine fadeCoroutine;    // Used to stop a previous coroutine if any to don't have two fadings going on on the same object

        public override void Reset()
        {
            fromValue = null;
            toValue = null;
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
                Color startColor;
                Color stopColor = toValue.Value;


                stopColor = toValue.Value;

                if (fadeFromCurrentColor.Value)
                {
                    startColor = sprite.color;
                }
                else
                {
                    startColor = fromValue.Value;
                }

                // Start fade in/out
                fadeCoroutine = StartCoroutine(FadeColor(startColor, stopColor));
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

        IEnumerator FadeColor(Color startColor, Color stopColor)
        {
            // Calculate values
            var timeStep = 0.01f;
            var wait = new WaitForSeconds(timeStep);
            //var steps = fadeTime.Value / timeStep;
            var color = startColor;

            //var oneStepValueR = (startColor.r - stopColor.r) / -steps;
            //var oneStepValueG = (startColor.g - stopColor.g) / -steps;
            //var oneStepValueB = (startColor.b - stopColor.b) / -steps;

            // Delay
            if (delay.Value > 0)
                yield return new WaitForSeconds(delay.Value);

            // Fade
            //for (int i = 0; i < steps; i++)
            //{
            //    yield return wait;
            //    color = new Color(color.r + oneStepValueR, color.g + oneStepValueG, color.b + oneStepValueB);
            //    UpdateAlpha(color);
            //}


            var oneStepValueR = (stopColor.r - startColor.r);
             var oneStepValueG = (stopColor.g - startColor.g);
             var oneStepValueB = (stopColor.b - startColor.b);

             color.r = startColor.r;
             color.g = startColor.g;
             color.b = startColor.b;

            float lastTime = Time.realtimeSinceStartup;
            float startTime = lastTime;
            bool finish = false;
            while (true)
            {
                float progress = Time.realtimeSinceStartup - lastTime;
                lastTime = Time.realtimeSinceStartup;

                color.r += (progress / fadeTime.Value) * oneStepValueR;
                color.g += (progress / fadeTime.Value) * oneStepValueG;
                color.b += (progress / fadeTime.Value) * oneStepValueB;

                //r
                if (oneStepValueR > 0)
                {
                    if (color.r > stopColor.r)
                    {
                        color.r = stopColor.r;
                         color.g = stopColor.g;
                        color.b = stopColor.b;
                        finish = true;
                    }
                }
                else
                {
                    if (color.r < stopColor.r)
                    {
                        color.r = stopColor.r;
                         color.g = stopColor.g;
                        color.b = stopColor.b;
                        finish = true;
                    }
                }

                //g
                if (oneStepValueG > 0)
                {
                    if (color.g > stopColor.g)
                    {
                        color.r = stopColor.r;
                        color.g = stopColor.g;
                        color.b = stopColor.b;
                        finish = true;
                    }
                }
                else
                {
                    if (color.g < stopColor.g)
                    {
                        color.r = stopColor.r;
                        color.g = stopColor.g;
                        color.b = stopColor.b;
                        finish = true;
                    }
                }

                //b
                if (oneStepValueB > 0)
                {
                    if (color.b > stopColor.b)
                    {
                        color.r = stopColor.r;
                        color.g = stopColor.g;
                        color.b = stopColor.b;
                        finish = true;
                    }
                }
                else
                {
                    if (color.b < stopColor.b)
                    {
                        color.r = stopColor.r;
                        color.g = stopColor.g;
                        color.b = stopColor.b;
                        finish = true;
                    }
                }

                if ((lastTime - startTime) > fadeTime.Value)
                {
                    color.r = stopColor.r;
                    color.g = stopColor.g;
                    color.b = stopColor.b;
                    finish = true;
                }

                // Update color
                color = new Color(color.r, color.g, color.b);
                UpdateAlpha(color);

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
        public void UpdateAlpha(Color color)
        {
            sprite.color = color;
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
