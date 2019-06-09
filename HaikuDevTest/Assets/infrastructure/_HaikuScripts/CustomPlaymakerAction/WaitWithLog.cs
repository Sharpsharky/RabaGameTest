using System.Globalization;
using TMPro;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Time)]
    [Tooltip("Delays a State from finishing by the specified time. NOTE: Other actions continue, but FINISHED can't happen before Time.")]
    public class WaitWithLog : FsmStateAction
    {
        [RequiredField]
        public FsmFloat time;
        public FsmEvent finishEvent;
        public bool realTime;

        [Tooltip("True if text must be reset to zero. False if new time elapsed must be added to the time currently displayed with text component.")]
        public bool ResetTextAtStart = true;
        public GameObject TextMeshProGameObject;
        private TextMeshPro text;

        private float startTime;
        private float timer;
        private float startValue = 0;
        private bool parseSuccess;

        public override void Reset()
        {
            time = 1f;
            finishEvent = null;
            realTime = false;
            TextMeshProGameObject = null;
            text = null;
            ResetTextAtStart = true;
        }

        public override void OnEnter()
        {
            if (time.Value <= 0)
            {
                Fsm.Event(finishEvent);
                Finish();
                return;
            }

            if (TextMeshProGameObject != null)
            {
                text = TextMeshProGameObject.GetComponent<TextMeshPro>();
                if (text == null)
                {
                    Debug.LogError("WaitWithLog: Can't find text mesh pro component!");

                    Fsm.Event(finishEvent);
                    Finish();
                    return;
                }
                if (ResetTextAtStart)
                {
                    text.text = "0";
                    startValue = 0;
                }
                else
                {
                    parseSuccess = float.TryParse(text.text, NumberStyles.Float, CultureInfo.InvariantCulture, out startValue);
                }
            }

            startTime = FsmTime.RealtimeSinceStartup;
            timer = 0f;
        }

        public override void OnUpdate()
        {
            // update time

            if (realTime)
            {
                timer = FsmTime.RealtimeSinceStartup - startTime;
            }
            else
            {
                timer += Time.deltaTime;
            }

            if (text != null)
            {
                if (parseSuccess || ResetTextAtStart)
                {
                    text.text = ((int)((startValue + timer))).ToString();
                }
                else if (!ResetTextAtStart)
                {
                    text.text = "ParseException. Time: " + ((int)(timer)).ToString();
                }
            }
            

            if (timer >= time.Value)
            {
                Finish();
                if (finishEvent != null)
                {
                    Fsm.Event(finishEvent);
                }
            }
        }

    }
}
