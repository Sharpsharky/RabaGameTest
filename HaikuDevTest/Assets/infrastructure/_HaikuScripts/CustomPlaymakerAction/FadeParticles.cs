using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("_Common")]
    [Tooltip("Fades particles. It takes first particle system color alpha as start color for every particle systems.")]
    public class FadeParticles : FsmStateAction
    {
        [RequiredField]
        public FsmOwnerDefault gameObject;
        [Tooltip("It takes first particle system color alpha as start color for every particle systems. This is the value they all fade to.")]
        public FsmFloat toValue;

        public FsmFloat fadeTime;

        private ParticleSystem myParticleSystem;      // This will be taken via GetComponent

        public FsmFloat delay;

        public FsmBool includeInactive = true;

        public FsmEvent finishedEvent;

        GameObject go;
        private ParticleSystem[] particleSystems;  // This will be taken via GetComponent from a given gameobject

        private Coroutine fadeCoroutine;    // Used to stop a previous coroutine if any to don't have two fadings going on on the same object

        public override void Reset()
        {
            toValue = 0;
            fadeTime = 0f;
            delay = 0f;
            finishedEvent = null;
            go = null;
            includeInactive = true;
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

            // Take sprites
            particleSystems = go.GetComponentsInChildren<ParticleSystem>(includeInactive.Value);
            // Check is sprite taken
            if (particleSystems != null)
            {
                // Set start and stop alpha
                var startAlpha = particleSystems[0].main.startColor.color.a;
                var stopAlpha = toValue.Value;
                // Start fade in/out
                fadeCoroutine = StartCoroutine(FadeAlphaValue(startAlpha, stopAlpha));
            }
            // If there is no sprite renderer component at the gameobject
            else
            {
                Debug.LogWarning("ParticleSystem not found at " + go.name);
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
            if (particleSystems != null)
            {
                foreach (var particleSystem in particleSystems)
                {
                    if (particleSystem != null)
                    {
                        var main = particleSystem.main;
                        main.startColor = new Color(particleSystem.main.startColor.color.r, particleSystem.main.startColor.color.g, particleSystem.main.startColor.color.b, alpha);
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
