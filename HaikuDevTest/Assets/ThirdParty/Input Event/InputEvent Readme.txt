InputEvent
--------------------------------

For any questions or comments please email assetstore@fmobg.com

The InputEvent class makes it easy to just think about the actions. 
No need to worry about checking for mouse clicks or key events each frame.

Just subscribe to the events :-)

Events supported:
	Key events (any key, specific key)
	Mouse events (move, buttons, drag)
	Touch events (start, drag, end)
	TouchAndMouse (single interface for handling both as the same)
	Axis events (start, change, end)
	Button events (down, up)

################################

	Function definitions:

	- listen for specific key
	InputHandler InputEvent.AddListenerKey(KeyCode key, InputHandlerDelegate onKeyDown, InputHandlerDelegate onKeyUp)
	
	-- listen for any key
	InputHandler InputEvent.AddListenerAnyKey(InputHandlerDelegate onKeyDown, InputHandlerDelegate onKeyUp)
	
	-- listen for mouse move
	InputHandler InputEvent.AddListenerMouseMove(InputHandlerDelegate onMove, float pointHistoryLength=5.0f)
	
	-- listen for mouse drag (mouse down -> move -> release)
	InputHandler InputEvent.AddListenerMouseDrag(int mouseButton, InputHandlerDelegate onMouseDown, InputHandlerDelegate onMouseMove, InputHandlerDelegate onMouseUp, float pointHistoryLength=5.0f)
	
	-- listen for touches (finger down -> move -> release)
	InputHandler InputEvent.AddListenerTouch(int fingerId, InputHandlerDelegate onTouchStart, InputHandlerDelegate onTouchMove, InputHandlerDelegate onTouchEnd, float pointHistoryLength=5.0f)	
	
	-- listen for mouse drag AND touches
	InputHandler InputEvent.AddListenerTouchOrMouse(InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd, float pointHistoryLength=5.0f)
	
	-- listen for named axis
	InputHandler InputEvent.AddListenerAxis(string axisName, InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd, float axisNeutral=0.0f)
	
	-- listen for named buttons
	InputHandler InputEvent.AddListenerButton(string buttonName, InputHandlerDelegate onStart, InputHandlerDelegate onChange, InputHandlerDelegate onEnd)
	
	-- remove any listener you have registered
	void InputEvent.RemoveListener(InputHandler toRemove)
	





