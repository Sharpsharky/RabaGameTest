using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Math")]
    [Tooltip("Get biggest value.")]
    public class IntMax : FsmStateAction
    {

        public FsmInt value1;
        public FsmInt value2;
        public FsmInt value3;
        public FsmInt value4;

        public FsmEvent value1TheBiggest;
        public FsmEvent value2TheBiggest;
        public FsmEvent value3TheBiggest;
        public FsmEvent value4TheBiggest;

        [Tooltip("The biggest value will be stored here")]
        public FsmInt biggestInt;

        public override void Reset()
        {
            value1 = null;
            value2 = null;
            value3 = null;
            value4 = null;
            value1TheBiggest = null;
            value2TheBiggest = null;
            value3TheBiggest = null;
            value4TheBiggest = null;
            biggestInt = null;
        }

        public override void OnEnter()
        {
            GetBiggestValue();
            Finish();
        }

        private void GetBiggestValue()
        {
            var biggestValue = 0;
            var choosenValue = 0;
            if (value1 != null && value2 != null)
            {
                if (value1.Value > value2.Value)
                {
                    biggestValue = value1.Value;
                    choosenValue = 1;
                }
                else
                {
                    biggestValue = value2.Value;
                    choosenValue = 2;
                }
            }
            if (value3 != null && value3.Value > biggestValue)
            {
                biggestValue = value3.Value;
                choosenValue = 3;
            }
            if (value4 != null && value4.Value > biggestValue)
            {
                biggestValue = value4.Value;
                choosenValue = 4;
            }

            // Store info
            if (biggestInt != null)
            {
                biggestInt.Value = biggestValue;
            }

            // Call event
            if (choosenValue == 1) CallEvent(value1TheBiggest);
            if (choosenValue == 2) CallEvent(value2TheBiggest);
            if (choosenValue == 3) CallEvent(value3TheBiggest);
            if (choosenValue == 4) CallEvent(value4TheBiggest);
        }

        private void CallEvent(FsmEvent fEvent)
        {
            if (fEvent != null)
            {
                Fsm.Event(fEvent);
            }
        }
    }
}
