using UnityEngine;
using System.Collections;

/*
 * The named Axis and Button handling part is defined in here
 * */
namespace InputEventNS
{
    public partial class InputEvent : MonoBehaviour
    {

        /// <summary>
        /// Listen for input axis value changes
        /// </summary>
        /// <param name="axisName">Name of the axis to listen for changes on</param>
        /// <param name="onAxisStart">Function to call when axis starts to change</param>
        /// <param name="onAxisChange">Function to call when axis value changes</param>
        /// <param name="onAxisStop">Function to call when axis returns to neutral</param>
        /// <param name="axisNeutral">The axis value to consider the axis as off</param>
        /// <returns>The InputHandler object. Pass it to RemoveListener to remove the listener.</returns>
        public static InputHandler AddListenerAxis(string axisName, InputHandlerDelegate onAxisStart, InputHandlerDelegate onAxisChange, InputHandlerDelegate onAxisStop, float axisNeutral = 0.0f)
        {
            InputHandlerAxis ih = new InputHandlerAxis(axisName, onAxisStart, onAxisChange, onAxisStop, axisNeutral);
            instance.inputHandler.Add(ih);
            return ih;
        }

        /// <summary>
        /// Listen for named button press/release
        /// </summary>
        /// <param name="buttonName">Name of the button to listen for</param>
        /// <param name="onButtonDown">Function to call when button is pressed</param>
        /// <param name="onButtonUp">Function to call when button is released</param>
        /// <returns>The InputHandler object. Pass it to RemoveListener to remove the listener.</returns>
        public static InputHandler AddListenerButton(string buttonName, InputHandlerDelegate onButtonDown, InputHandlerDelegate onButtonUp)
        {
            InputHandlerButton ih = new InputHandlerButton(buttonName, onButtonDown, onButtonUp);
            instance.inputHandler.Add(ih);
            return ih;
        }

    }
    /// <summary>
    /// Abstract class from which named input handlers are derived
    /// </summary>
    public abstract class InputHandlerAxisButton : InputHandler
    {
        public string inputName;
        public InputHandlerAxisButton(string inputName, InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd)
            : base(inputName.GetHashCode(), onStart, onChange, onEnd)
        {
            this.inputName = inputName;
        }
    }

    /// <summary>
    /// Named Axis handler
    /// </summary>
    public class InputHandlerAxis : InputHandlerAxisButton
    {
        /// <summary>
        /// The "off" value of the axis
        /// </summary>
        public float axisNeutral;

        /// <summary>
        /// The current axis value
        /// </summary>
        public float axis
        {
            get { return Input.GetAxis(inputName); }
        }

        /// <summary>
        /// The last value of the axis
        /// </summary>
        private float lastAxis;

        /// <summary>
        /// Run once per frame (this is handled by the InputEvent class)
        /// </summary>
        public override void Update()
        {
            // get the current value
            float axis = Input.GetAxis(inputName);

            // has it changed
            if (axis == lastAxis) return;
            lastAxis = axis;

            // check which change message to send
            switch (status)
            {
                case EventStatus.None:
                case EventStatus.End:
                    // start
                    Start();
                    Change();
                    break;
                case EventStatus.Start:
                case EventStatus.Change:
                    Change();
                    if (axis == axisNeutral)
                    {
                        End();
                    }
                    break;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="axisName">Name of the axis to listen for changes on</param>
        /// <param name="onStart">Function to call when axis goes from "off" to "on"</param>
        /// <param name="onChange">Function to call when the axis value changes</param>
        /// <param name="onEnd">Function to call when the axis goes from "on" to "off"</param>
        /// <param name="axisNeutral">The value at which the axis is considered "off"</param>
        public InputHandlerAxis(string axisName, InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd, float axisNeutral)
            : base(axisName, onStart, onChange, onEnd)
        {
            this.axisNeutral = axisNeutral;
            lastAxis = Input.GetAxis(axisName);

            if (lastAxis != axisNeutral)
                Start();
        }
    }

    /// <summary>
    /// Named Button handler
    /// </summary>
    public class InputHandlerButton : InputHandlerAxisButton
    {
        /// <summary>
        /// Run once per frame (this is handled by the InputEvent class)
        /// </summary>
        public override void Update()
        {
            if (status == EventStatus.Start)
            {
                if (!Input.GetButton(inputName))
                {
                    End();
                }
            }
            else if (Input.GetButton(inputName))
                Start();
        }

        /// <summary>
        /// Constructot
        /// </summary>
        /// <param name="buttonName">Name of the button to listen for press/release on</param>
        /// <param name="onButtonDown">Function to call when button is pressed</param>
        /// <param name="onButtonUp">Function to call when button is released</param>
        public InputHandlerButton(string buttonName, InputHandlerDelegate onButtonDown, InputHandlerDelegate onButtonUp)
            : base(buttonName, onButtonDown, null, onButtonUp)
        {
            if (Input.GetButton(buttonName))
                Start();
        }
    }
}
