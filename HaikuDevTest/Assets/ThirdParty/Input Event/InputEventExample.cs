using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using InputEventNS;

/*
 * Very simple example just meant to show how the event driven input system works
 * 
 * => just register a function to be called when some input happens :-)
 * */

public class InputEventExample : MonoBehaviour {

    // holds the any key listener so we can un-register it when user clicks any key
    private InputHandler anyKeyListener;

	// Use this for initialization
	void Start () {

        // Register a listener for any key to start the game
        anyKeyListener = InputEvent.AddListenerAnyKey(PushStart, null);
        
        // "game" related: Make the sphere wait to be woken (click, touch or key)
        sphere.GetComponent<Rigidbody>().Sleep();

        // create a bouncy physics material
        PhysicMaterial pm = new PhysicMaterial("Bouncy");
        pm.bounceCombine = PhysicMaterialCombine.Maximum;
        pm.bounciness = 0.8f;
        pm.staticFriction = 0.4f;
        pm.dynamicFriction = 0.4f;
        sphere.GetComponent<Collider>().material = pm;
	}

    // wake the sphere
    void PushStart(InputHandler input)
    {
        // Remove the any key listener
        InputEvent.RemoveListener(anyKeyListener);

        // Register a listener for more from the same keyboard key
        key = ((InputHandlerKey)input).key;
        InputEvent.AddListenerKey(key, PushUp, null);

        // Register a listener function for pointer event (click or touch start)
        InputEvent.AddListenerTouchOrMouse(PushUp, null, null);

        // wake the ball
        started = true;
        sphere.GetComponent<Rigidbody>().AddForce(Vector3.right * 5, ForceMode.VelocityChange);
    }

    /*
     * Non InputEvent example related stuff comes afte this line
     * */

    public Transform sphere;
    public Transform cube;
    public Transform capsule;
    private bool started = false;
    public float upForce = 10;
    private KeyCode key;

    // just simple alias functions
    private void PushUp(InputHandler p) { PushUp(); }
    // private void PushUp(InputEvent.InputHandler k) { PushUp(); }

    // this handles the upward motion
    private void PushUp()
    {
        // Push the ball up
        sphere.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce, ForceMode.VelocityChange);                    
    }

    // "game" gui
    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 200, 50), started ? "Key: "+ key+", or mouse click, or touch to go up" : "Press any key to start");        
    }

    // "game" code
    void FixedUpdate()
    {
        Vector3 p = cube.position;
        p.x = sphere.position.x;

        cube.position = p;

        p = Camera.main.transform.position;
        p.x = sphere.position.x;

        Camera.main.transform.position = p;

        if (sphere.GetComponent<Rigidbody>().velocity.x < 0)
        {
            //Application.LoadLevel(Application.loadedLevel);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        p = Camera.main.WorldToViewportPoint(capsule.position);
        if (p.x < -0.1f)
        {
            Vector3 p2 = capsule.position;
            p.x = 1.1f;
            p2.x = Camera.main.ViewportToWorldPoint(p).x;
            capsule.position = p2;
        }
    }
}
