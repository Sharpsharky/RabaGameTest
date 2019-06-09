using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Input events for mouse and touch
 *  (start, move and end events)
 * 
 * */
namespace InputEventNS
{
    public partial class InputEvent : MonoBehaviour
    {
        /// <summary>
        /// Listen for mouse move events
        /// </summary>
        /// <param name="onMove">Function to call when mouse moves</param>
        /// <param name="pointHistoryLength">The number of seconds history that should be kept</param>
        /// <returns>The InputHandler object. Pass it to RemoveListener to remove the listener.</returns>
        public static InputHandler AddListenerMouseMove(InputHandlerDelegate onMove, float pointHistoryLength = 5.0f)
        {
            InputHandlerMouseMove ih = new InputHandlerMouseMove(onMove, pointHistoryLength);
            instance.inputHandler.Add(ih);
            return ih;
        }

        /// <summary>
        /// Listen for mouse drag events (mouse down -> move -> release)
        /// </summary>
        /// <param name="mouseButton">Which mouse button are we binding to</param>
        /// <param name="onMouseDown">(Optional) handler for mouse down event</param>
        /// <param name="onMouseMove">(Optional) handler for mouse move event</param>
        /// <param name="onMouseUp">(Optional) handler for mouse up event</param>
        /// <param name="pointHistoryLength">The number of seconds history that should be kept</param>
        /// <returns>The InputHandler object. Pass it to RemoveListener to remove the listener.</returns>
        public static InputHandler AddListenerMouseDrag(int mouseButton, InputHandlerDelegate onMouseDown, InputHandlerDelegate onMouseMove, InputHandlerDelegate onMouseUp, float pointHistoryLength = 5.0f)
        {
            InputHandlerMouseDrag ih = new InputHandlerMouseDrag(mouseButton, onMouseDown, onMouseMove, onMouseUp, pointHistoryLength);
            instance.inputHandler.Add(ih);
            return ih;
        }

        /// <summary>
        /// Listen for touch events (finger down -> move -> release)
        /// </summary>
        /// <param name="fingerId">Zero based index for which finger id to bind to</param>
        /// <param name="onTouchStart">(Optional) handler function for touch begin event</param>
        /// <param name="onTouchMove">(Optional) handler function for touch move event</param>
        /// <param name="onTouchEnd">(Optional) handler function for touch end event</param>
        /// <param name="pointHistoryLength">The number of seconds history that should be kept</param>
        /// <returns>The InputHandler object. Pass it to RemoveListener to remove the listener.</returns>
        public static InputHandler AddListenerTouch(int fingerId, InputHandlerDelegate onTouchStart, InputHandlerDelegate onTouchMove, InputHandlerDelegate onTouchEnd, float pointHistoryLength = 5.0f)
        {
            InputHandlerTouch ih = new InputHandlerTouch(fingerId, onTouchStart, onTouchMove, onTouchEnd, pointHistoryLength);
            instance.inputHandler.Add(ih);
            return ih;
        }

        /// <summary>
        /// Listen for touch or mouse events (down -> move -> release)
        /// </summary>
        /// <param name="onStart">(Optional) handler function for * down event</param>
        /// <param name="onChange">(Optional) handler function for * move event</param>
        /// <param name="onEnd">(Optional) handler function for * up event</param>
        /// <param name="pointHistoryLength">The number of seconds history that should be kept</param>
        /// <returns>The InputHandler object. Pass it to RemoveListener to remove the listener.</returns>
        public static InputHandler AddListenerTouchOrMouse(InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd, float pointHistoryLength = 5.0f)
        {
            InputHandlerTouchOrMouse ih = new InputHandlerTouchOrMouse(onStart, onChange, onEnd, pointHistoryLength);
            instance.inputHandler.Add(ih);
            return ih;
        }
    }

    /// <summary>
    /// Common ancestor for MouseDrag, MouseMove and Touch handlers
    /// </summary>
    public abstract class InputHandlerPointer : InputHandler
    {
        /// <summary>
        /// Space and time for the pointer position
        /// </summary>
        public class Vector3Time
        {
            public Vector3 position;
            public float time;

            public Vector3Time(Vector3 position)
            {
                this.position = position;
                this.time = Time.time;
            }
        }

        /// <summary>
        /// List of points the pointer makes
        /// </summary>
        public List<Vector3Time> positions;

        /// <summary>
        /// The time and position of the pointer when the event first started
        /// </summary>
        public Vector3Time initialPosition;

        /// <summary>
        /// Shorthand for earliest position within pointHistoryLength timeframe
        /// </summary>
        public Vector3 startPosition { get { return positions[0].position; } }

        /// <summary>
        /// Shorthand for current pointer position
        /// </summary>
        public Vector3 currentPosition { get { return positions[positions.Count - 1].position; } }

        /// <summary>
        /// The number of seconds of pointer history to keep in the list
        /// </summary>
        public float pointHistoryLength;

        /// <summary>
        /// Add a position+current time to the list of positions
        /// </summary>
        /// <param name="position"></param>
        protected void AddPosition(Vector3 position)
        {
            if (initialPosition == null)
            {
                initialPosition = new Vector3Time(position);
                positions.Add(initialPosition);
            }
            else 
                positions.Add(new Vector3Time(position));
        }

        /// <summary>
        /// Updates the InputHandlerPointer. Automatically done by the InputEvent system.
        /// </summary>
        public override void Update()
        {
            // Trim
            Trim();
        }

        /// <summary>
        /// Trims the list of point based on the duration of pointHistoryLength. Is automatically applied to any active InputHandlerPointer
        /// </summary>
        public void Trim()
        {
            // dont keep forever.. last 5 seconds (edit if you like more or less points to hang around)
            if (positions.Count > 1)
            {
                float last5 = Time.time - pointHistoryLength;
                if (positions[0].time < last5)
                {
                    int i = 1;
                    while (positions[i].time < last5 && i < positions.Count - 1) i++;
                    positions.RemoveRange(0, i - 1);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The id of the pointer</param>
        /// <param name="onStart">(Optional) handler for start event</param>
        /// <param name="onChange">(Optional) handler for change/move event</param>
        /// <param name="onEnd">(Optional) handler for end event</param>
        public InputHandlerPointer(int id, InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd, float pointHistoryLength)
            : base(id, onStart, onChange, onEnd)
        {
            this.pointHistoryLength = pointHistoryLength;
            positions = new List<Vector3Time>();
        }
    }

    /// <summary>
    /// Mouse drag handler
    /// </summary>
    public class InputHandlerMouseDrag : InputHandlerPointer
    {
        /// <summary>
        /// Run once per frame (this is handled by the InputEvent class)
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (status == EventStatus.End)
                status = EventStatus.None;

            if (status == EventStatus.None)
            {
                if (Input.GetMouseButton(id))
                {
                    AddPosition(Input.mousePosition);
                    Start();
                }
            }
            else
            {
                if (Input.GetMouseButton(id))
                {
                    if (Input.mousePosition != currentPosition)
                    {
                        AddPosition(Input.mousePosition);
                        Change();
                    }
                }
                else
                {
                    End();
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Mouse button to bind to</param>
        /// <param name="onStart">(Optional) handler for start event</param>
        /// <param name="onChange">(Optional) handler for change/move event</param>
        /// <param name="onEnd">(Optional) handler for end event</param>
        public InputHandlerMouseDrag(int id, InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd, float pointHistoryLength = 5.0f)
            : base(id, onStart, onChange, onEnd, pointHistoryLength)
        {
            if (Input.GetMouseButton(id))
            {
                AddPosition(Input.mousePosition);
                Start();
            }
        }
    }

    /// <summary>
    /// Mouse move handler
    /// </summary>
    public class InputHandlerMouseMove : InputHandlerPointer
    {
        /// <summary>
        /// Run once per frame (this is handled by the InputEvent class)
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (Input.mousePosition != currentPosition)
            {
                AddPosition(Input.mousePosition);
                Change();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onChange">The function to call when mouse moves</param>
        public InputHandlerMouseMove(InputHandlerDelegate onChange, float pointHistoryLength = 5.0f)
            : base(-1, null, onChange, null, pointHistoryLength)
        {
            AddPosition(Input.mousePosition);
        }
    }

    /// <summary>
    /// Touch handler
    /// </summary>
    public class InputHandlerTouch : InputHandlerPointer
    {
        /// <summary>
        /// Run once per frame (this is handled by the InputEvent class)
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (Input.touchCount == 0) return;

            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == id)
                {
                    switch (Input.touches[i].phase)
                    {
                        case TouchPhase.Began:
                            AddPosition(Input.touches[i].position);
                            Start();
                            break;
                        case TouchPhase.Moved:
                            AddPosition(Input.touches[i].position);
                            Change();
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            End();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Finger id to bind to</param>
        /// <param name="onStart">(Optional) handler for start event</param>
        /// <param name="onChange">(Optional) handler for change/move event</param>
        /// <param name="onEnd">(Optional) handler for end event</param>
        public InputHandlerTouch(int id, InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd, float pointHistoryLength = 5.0f)
            : base(id, onStart, onChange, onEnd, pointHistoryLength)
        {
            if (Input.touchCount != 0)
            {
                for (int i = 0; i < Input.touches.Length; i++)
                {
                    if (Input.touches[i].fingerId == id && Input.touches[i].phase != TouchPhase.Canceled && Input.touches[i].phase != TouchPhase.Ended)
                    {
                        AddPosition(Input.touches[i].position);
                        Start();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handler for the combined mouse and touch handler
    /// </summary>
    public class InputHandlerTouchOrMouse : InputHandler
    {
        /// <summary>
        /// Holds all handlers started by touch or mouse
        /// </summary>
        private List<InputHandler> inputHandler;

        /// <summary>
        /// Duration of tracking point history
        /// </summary>
        public float pointHistoryLength;

        InputHandlerDelegate OnStart;
        InputHandlerDelegate OnChange;
        InputHandlerDelegate OnEnd;

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

            if (Input.touchCount == 0 || !Input.simulateMouseWithTouches)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        InputHandlerMouseDrag ih = new InputHandlerMouseDrag(i, OnStart, OnChange, OnEnd, pointHistoryLength);
                        inputHandler.Add(ih);
                    }
                }
            }

            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.touches[i].phase == TouchPhase.Began)
                    {
                        InputHandlerTouch ih = new InputHandlerTouch(Input.touches[i].fingerId, OnStart, OnChange, OnEnd, pointHistoryLength);
                        inputHandler.Add(ih);
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onStart">(Optional) handler for start event</param>
        /// <param name="onChange">(Optional) handler for change/move event</param>
        /// <param name="onEnd">(Optional) handler for end event</param>
        public InputHandlerTouchOrMouse(InputHandlerDelegate OnStart, InputHandlerDelegate OnChange, InputHandlerDelegate OnEnd, float pointHistoryLength = 5.0f)
            : base(-1, null, null, null)
        {
            this.OnStart = OnStart;
            this.OnChange = OnChange;
            this.OnEnd = OnEnd;
            inputHandler = new List<InputHandler>();
        }
    }

}