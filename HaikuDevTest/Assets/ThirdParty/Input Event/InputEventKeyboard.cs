using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * The keyboard key handling part is defined in here
 * */
namespace InputEventNS
{
    public partial class InputEvent : MonoBehaviour
    {

        /// <summary>
        /// Listen for a specific key to be pressed or released
        /// </summary>
        /// <param name="key">KeyCode to listen for</param>
        /// <param name="onKeyDown">(Optional) handler function for key down event</param>
        /// <param name="onKeyUp">(Optional) handler function for key up event</param>
        /// <returns>The InputHandler object. Pass it to RemoveListener to remove the listener.</returns>
        public static InputHandler AddListenerKey(KeyCode key, InputHandlerDelegate onKeyDown, InputHandlerDelegate onKeyUp)
        {
            InputHandlerKey ih = new InputHandlerKey(key, onKeyDown, onKeyUp);
            instance.inputHandler.Add(ih);
            return ih;
        }

        /// <summary>
        /// Listen for any key
        /// </summary>
        /// <param name="onKeyDown">(Optional) handler function for key down event</param>
        /// <param name="onKeyUp">(Optional) handler function for key up event</param>
        /// <returns>The InputHandler object. Pass it to RemoveListener to remove the listener.</returns>
        public static InputHandler AddListenerAnyKey(InputHandlerDelegate onKeyDown, InputHandlerDelegate onKeyUp)
        {
            InputHandlerAnyKey ih = new InputHandlerAnyKey(onKeyDown, onKeyUp);
            instance.inputHandler.Add(ih);
            return ih;
        }

    }
    /// <summary>
    /// All the data you need related to key events
    /// </summary>
    public class InputHandlerKey : InputHandler
    {
        /// <summary>
        /// The KeyCode we are listening for
        /// </summary>
        public KeyCode key;

        /// <summary>
        /// When the key was (last) pressed
        /// </summary>
        public float timePressed;

        /// <summary>
        /// When the key was (last) released
        /// </summary>
        public float timeReleased;

        /// <summary>
        /// Run once per frame (this is handled by the InputEvent class)
        /// </summary>
        public override void Update()
        {
            if (Input.GetKeyDown(key))
            {
                timePressed = Time.time;
                Start();
            }
            else if (Input.GetKeyUp(key))
            {
                timeReleased = Time.time;
                End();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">KeyCode to listen for</param>
        /// <param name="onStart">Function to call on key press</param>
        /// <param name="onEnd">Function to call on key release</param>
        public InputHandlerKey(KeyCode key, InputHandlerDelegate onStart, InputHandlerDelegate onEnd)
            : base((int)key, onStart, null, onEnd)
        {
            this.key = key;
            if (Input.GetKey(key))
            {
                timePressed = Time.time;
                Start();
            }
        }
    }

    /// <summary>
    /// Handler for any key press. Delegates proper handling to InputHandlerKey when a key is pressed.
    /// </summary>
    public class InputHandlerAnyKey : InputHandler
    {
        private List<InputHandler> inputHandler;
        private KeyCode[] allKeys = new KeyCode[]{
	          KeyCode.None,KeyCode.Backspace,KeyCode.Delete,KeyCode.Tab,KeyCode.Clear,KeyCode.Return,KeyCode.Pause,KeyCode.Escape,KeyCode.Space,KeyCode.Keypad0,KeyCode.Keypad1,KeyCode.Keypad2,KeyCode.Keypad3,KeyCode.Keypad4,KeyCode.Keypad5,KeyCode.Keypad6,KeyCode.Keypad7,KeyCode.Keypad8,KeyCode.Keypad9,KeyCode.KeypadPeriod,KeyCode.KeypadDivide,KeyCode.KeypadMultiply,KeyCode.KeypadMinus,KeyCode.KeypadPlus,KeyCode.KeypadEnter,KeyCode.KeypadEquals,KeyCode.UpArrow,KeyCode.DownArrow,KeyCode.RightArrow,KeyCode.LeftArrow,KeyCode.Insert,KeyCode.Home,KeyCode.End,KeyCode.PageUp,KeyCode.PageDown,KeyCode.F1,KeyCode.F2,KeyCode.F3,KeyCode.F4,KeyCode.F5,KeyCode.F6,KeyCode.F7,KeyCode.F8,KeyCode.F9,KeyCode.F10,KeyCode.F11,KeyCode.F12,KeyCode.F13,KeyCode.F14,KeyCode.F15,KeyCode.Alpha0,KeyCode.Alpha1,KeyCode.Alpha2,KeyCode.Alpha3,KeyCode.Alpha4,KeyCode.Alpha5,KeyCode.Alpha6,KeyCode.Alpha7,KeyCode.Alpha8,KeyCode.Alpha9,KeyCode.Exclaim,KeyCode.DoubleQuote,KeyCode.Hash,KeyCode.Dollar,KeyCode.Ampersand,KeyCode.Quote,KeyCode.LeftParen,KeyCode.RightParen,KeyCode.Asterisk,KeyCode.Plus,KeyCode.Comma,KeyCode.Minus,KeyCode.Period,KeyCode.Slash,KeyCode.Colon,KeyCode.Semicolon,KeyCode.Less,KeyCode.Equals,KeyCode.Greater,KeyCode.Question,KeyCode.At,KeyCode.LeftBracket,KeyCode.Backslash,KeyCode.RightBracket,KeyCode.Caret,KeyCode.Underscore,KeyCode.BackQuote,KeyCode.A,KeyCode.B,KeyCode.C,KeyCode.D,KeyCode.E,KeyCode.F,KeyCode.G,KeyCode.H,KeyCode.I,KeyCode.J,KeyCode.K,KeyCode.L,KeyCode.M,KeyCode.N,KeyCode.O,KeyCode.P,KeyCode.Q,KeyCode.R,KeyCode.S,KeyCode.T,KeyCode.U,KeyCode.V,KeyCode.W,KeyCode.X,KeyCode.Y,KeyCode.Z,KeyCode.Numlock,KeyCode.CapsLock,KeyCode.ScrollLock,KeyCode.RightShift,KeyCode.LeftShift,KeyCode.RightControl,KeyCode.LeftControl,KeyCode.RightAlt,KeyCode.LeftAlt,KeyCode.LeftCommand,KeyCode.LeftApple,KeyCode.LeftWindows,KeyCode.RightCommand,KeyCode.RightApple,KeyCode.RightWindows,KeyCode.AltGr,KeyCode.Help,KeyCode.Print,KeyCode.SysReq,KeyCode.Break,KeyCode.Menu
        };
        private InputHandlerDelegate OnStart;
        private InputHandlerDelegate OnEnd;

        /// <summary>
        /// Run once per frame (this is handled by the InputEvent class)
        /// </summary>
        public override void Update()
        {
            for (int i = inputHandler.Count - 1; i >= 0; i--)
            {
                if (inputHandler[i].status == EventStatus.End)
                    inputHandler.RemoveAt(i);
                else
                    inputHandler[i].Update();
            }

            if (Input.anyKey)
            {
                for (int i = allKeys.Length - 1; i >= 0; i--)
                {
                    if (Input.GetKeyDown(allKeys[i]))
                    {
                        InputHandlerKey ih = new InputHandlerKey(allKeys[i], OnStart, OnEnd);
                        inputHandler.Add(ih);
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onStart">Function to call on key press</param>
        /// <param name="onEnd">Function to call on key release</param>
        public InputHandlerAnyKey(InputHandlerDelegate onStart, InputHandlerDelegate onEnd)
            : base(-1, null, null, null)
        {
            inputHandler = new List<InputHandler>();

            this.OnStart = onStart;
            this.OnEnd = onEnd;
        }
    }

}