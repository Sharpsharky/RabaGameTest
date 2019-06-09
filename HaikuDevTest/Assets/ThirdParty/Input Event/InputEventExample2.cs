using UnityEngine;
using System.Collections;

using InputEventNS;

public class InputEventExample2 : MonoBehaviour {

    public float pointHistoryLength = 3.0f;
    public GameObject goLineRenderer;
    
	// Register your event listener in Start
	void Start () {
        // Register the listener
        InputEvent.AddListenerTouchOrMouse(TouchOrMouseHandler, null, null, pointHistoryLength);

        // Add some pretty material to the lines
        LineRenderer lr = goLineRenderer.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended"));
        goLineRenderer.SetActive(false);
	}

    // This is the event handler function
    void TouchOrMouseHandler(InputHandler handler)
    {
        // TouchOrMouse event is of (sub-)type InputHandlerPointer
        InputHandlerPointer pointer = (InputHandlerPointer)handler;

        // Update this with the current pointHistoryLength,
        // in case you'd experiment with it in the editor
        pointer.pointHistoryLength = pointHistoryLength;
        
        // Start the coroutine which draws the line for us
        StartCoroutine(LineDrawer(pointer));
    }

    // Draw lines for as long as they last
    IEnumerator LineDrawer(InputHandlerPointer pointer)
    {
        GameObject clone = Instantiate(goLineRenderer) as GameObject;
        clone.SetActive(true);
        LineRenderer lineRenderer = clone.GetComponent<LineRenderer>();        

        int linesize = 0;
        // While the pointer is still active
        while (pointer.status != EventStatus.End)
        {
            // Update the LineRenderer
            linesize = pointer.positions.Count;
            lineRenderer.positionCount = linesize;

            int j = 0;
            for (int i = pointer.positions.Count - 1; i >= 0; i--)
            {
                lineRenderer.SetPosition(
                    j++, // vertex #i
                    Camera.main.ScreenToWorldPoint(pointer.positions[i].position + Vector3.forward) // touch or mouse is in screen space, convert to world space
                );
            }

            yield return null;
        }

        while (linesize > 0)
        {
            lineRenderer.positionCount = --linesize;
            yield return null;
        }

        Destroy(clone);
    }

    void OnGUI()
    {
        GUI.Button(new Rect(5, 5, 250, 75), "This example supports multitouch.\nConnect a unity remote to draw multiple\nlines at the same time");
    }
}
