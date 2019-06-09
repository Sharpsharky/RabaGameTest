
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Effects)]
    [Tooltip("Turns a Game Object on/off in a regular repeating pattern.")]
    public class BlinkFor : ComponentAction<Renderer>
    {
        [RequiredField]
        [Tooltip("The GameObject to blink on/off.")]
        public FsmOwnerDefault gameObject;

        [Tooltip("Time to stay off in seconds.")]
        public FsmFloat timeOff;

        [Tooltip("Time to stay on in seconds.")]
        public FsmFloat timeOn;

        [Tooltip("How long it will stay")]
        public FsmFloat actionTimeInterval;

        [Tooltip("Should the object start in the active/visible state?")]
        public FsmBool startOn;

        [Tooltip("Only effect the renderer, keeping other components active.")]
        public bool rendererOnly;

        [Tooltip("Ignore TimeScale. Useful if the game is paused.")]
        public bool realTime;

        [Tooltip("You can also use FINISH and leave this empty.")]
        public FsmEvent eventDone;

        private float startTime;
        private float timer;
        private float timerAction;
        private bool blinkOn;

        public override void Reset()
        {
            gameObject = null;
            timeOff = 0.5f;
            timeOn = 0.5f;
            actionTimeInterval = 1f;
            rendererOnly = true;
            startOn = false;
            realTime = false;
            eventDone = null;
        }

        public override void OnEnter()
        {
            startTime = FsmTime.RealtimeSinceStartup;
            timer = 0f;
            timerAction = 0f;

            UpdateBlinkState(startOn.Value);
        }

        public override void OnUpdate()
        {
            // update time
            timerAction += Time.deltaTime;

            if (realTime)
            {
                timer = FsmTime.RealtimeSinceStartup - startTime;
            }
            else
            {
                timer += Time.deltaTime;
            }

            // update blink

            if (blinkOn && timer > timeOn.Value)
            {
                UpdateBlinkState(false);
            }

            if (blinkOn == false && timer > timeOff.Value)
            {
                UpdateBlinkState(true);
            }

            if (timerAction > actionTimeInterval.Value)
            {
                if (eventDone != null)
                    Fsm.Event(eventDone);
                Finish();
            }
        }

        void UpdateBlinkState(bool state)
        {
            var go = gameObject.OwnerOption == OwnerDefaultOption.UseOwner ? Owner : gameObject.GameObject.Value;
            if (go == null)
            {
                return;
            }

            if (rendererOnly)
            {
                if (UpdateCache(go))
                {
                    renderer.enabled = state;
                }
            }
            else
            {
#if UNITY_3_5 || UNITY_3_4
                go.active = state;
#else          
                go.SetActive(state);
#endif
            }

            blinkOn = state;

            // reset timer

            startTime = FsmTime.RealtimeSinceStartup;
            timer = 0f;
        }
    }
}

