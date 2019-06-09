// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System;

namespace HutongGames.PlayMaker.Actions
{
	[Serializable]
	public class DataEvent
	{
		public FsmEventTarget target;
		public string data;
		public FsmEvent sendEvent;
	}

	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends an Event after an optional delay. NOTE: To send events between FSMs they must be marked as Global in the Events Browser.")]
	public class SendMultipleEventsWithData : FsmStateAction
	{
		[Tooltip("Events")]
		public int arrayIndex = 0;
		public bool sendAll = false;
		public DataEvent[] events;
		
		public override void Reset()
		{

		}
		
		public override void OnEnter()
		{
			if (!sendAll)
			{
				Fsm.EventData.StringData = events[arrayIndex].data;
				Fsm.Event(events[arrayIndex].target, events[arrayIndex].sendEvent);
				arrayIndex++;
				Fsm.Event("next");
			}
			else
			{
				for(int i = 0; i < events.Length; i++)
				{
					Fsm.EventData.StringData = events[i].data;
					Fsm.Event(events[i].target, events[i].sendEvent);
				}
				Finish();
			}
		}
		
		public override void OnUpdate()
		{

		}
	}
}