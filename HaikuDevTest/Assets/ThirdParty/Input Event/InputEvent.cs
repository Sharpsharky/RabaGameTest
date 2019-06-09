using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * InputEvent lets you handle input when it occurs; no more polling for changes manually.
 * 
 * Usage: 
 *      See the many InputEvent.AddListener* methods
 *      (they are defined in InputEventKeyboard and InputEventMouseTouch)
 * */

namespace InputEventNS
{
    // Input event statuses
    public enum EventStatus
    {
        /// <summary>
        /// Uninitialized state
        /// </summary>
        None,
        /// <summary>
        /// The event has started: for pointers this is MouseDown or TouchStart, for key presses this is KeyDown
        /// </summary>
        Start,
        /// <summary>
        /// The event has changed: for pointers this is MouseMove or TouchMove. For key presses this means the key was pressed in a previous frame and is still down.
        /// </summary>        
        Change,
        /// <summary>
        /// The event has stopped: for pointers this is MouseUp or TouchEnd. For key presses this is KeyUp
        /// </summary>
        End
    }

    /// <summary>
    /// Callback function for input event
    /// </summary>
    /// <param name="input">Holds the data about the input event</param>
    public delegate void InputHandlerDelegate(InputHandler input);

    /// <summary>
    /// Base class for all types of input handlers
    /// </summary>
    public abstract class InputHandler
    {
        /// <summary>
        /// The id of the input
        /// </summary>
        public int id;

        /// <summary>
        /// The status of the input
        /// </summary>
        public EventStatus status;

        /// <summary>
        /// Run once per frame (this is handled by the InputEvent class)
        /// </summary>
        public abstract void Update();

        protected event InputHandlerDelegate onStart;
        protected event InputHandlerDelegate onChange;
        protected event InputHandlerDelegate onEnd;

        protected void Start()
        {
            status = EventStatus.Start;
            if (onStart != null)
                onStart(this);
        }

        protected void Change()
        {
            status = EventStatus.Change;
            if (onChange != null)
                onChange(this);

        }

        protected void End()
        {
            status = EventStatus.End;
            if (onEnd != null)
                onEnd(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">(Not unique) id of the input being handled</param>
        /// <param name="onStart">(Optional) handler for start event</param>
        /// <param name="onChange">(Optional) handler for change/move event</param>
        /// <param name="onEnd">(Optional) handler for end event</param>
        public InputHandler(int id, InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd)
        {
            this.id = id;
            this.status = EventStatus.None;

            if (onStart != null)
                this.onStart += onStart;
            if (onChange != null)
                this.onChange += onChange;
            if (onEnd != null)
                this.onEnd += onEnd;
        }
    }

    public partial class InputEvent : MonoBehaviour
    {
        

        /// <summary>
        /// Property for having a simple singleton pattern
        /// </summary>
        private static InputEvent me;

        /// <summary>
        /// Public static reference to an instance of InputEvent
        /// </summary>
        public static InputEvent instance
        {
            get
            {
                if (me == null)
                {
                    GameObject go = new GameObject("InputEvent Manager");
                    me = go.AddComponent<InputEvent>();
                }
                return me;
            }
        }

        /// <summary>
        /// Register me on enable if nobody else has yet
        /// </summary>
        void OnEnable()
        {
            if (InputEvent.me == null)
                InputEvent.me = this;
        }

        protected List<InputHandler> inputHandler = new List<InputHandler>();

        /// <summary>
        /// Remove a listener you have previously added
        /// </summary>
        /// <param name="toRemove">The event handler you got as return value from any of the AddListener* function</param>
        public static void RemoveListener(InputHandler toRemove)
        {
            //instance.inputHandler.Remove(toRemove);

			//don't use instance because the app might be closing down when RemoveListener is called
			//and we don't want to spawn new objects
			if (me != null) { 
				me.inputHandler.Remove (toRemove);
			}
        }

        /// <summary>
        /// On update, loop through all handlers and run them
        /// </summary>
        void Update()
        {
			
            for (int i = inputHandler.Count - 1; i >= 0; i--)
            {
				
				if (i >= inputHandler.Count) {
					//this can happen if the Update causes a call to RemoveListener
					continue;
				}
                inputHandler[i].Update();
            }
        }
    }
}