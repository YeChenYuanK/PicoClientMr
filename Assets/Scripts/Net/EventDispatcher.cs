using System;
using System.Collections.Generic;
using LekeNet.Room;

namespace LekeNet
{
	public class Event {

		private string name ;

		private Object data ;

		public string Name {
			get { return this.name; }
			set { this.name = value; }
		}

		public Object Data {
			get { return this.data; }
			set { this.data = value;}
		}

        public static Event ValueOf(string eventName, Object data)
        {
            Event ev = new Event();
            ev.name = eventName;
            ev.data = data;
            return ev;
        }

    }

	public class EventDispatcher
	{
		public EventDispatcher ()
		{
		}

		public delegate void EventHandler(Event ev);

		private Dictionary<string,EventHandler> eventDict = new Dictionary<string, EventHandler>();

		public void AddEventListener(string eventName, EventHandler eventHandler) {
			this.eventDict [eventName] = eventHandler;
		}

		public void RemoveEventLisener(string eventName) {
			if (this.eventDict.ContainsKey (eventName)) {
				this.eventDict.Remove (eventName);
			}
		}

		public void Dispatch(Event ev) {
			if (this.eventDict.ContainsKey (ev.Name)) {
                try
                {
                    this.eventDict[ev.Name](ev);
                } catch(Exception e)
                {
                    LogUtil.Log("net", "event handle fail, eventName : " + ev.Name + " , e : " + e.Message + " , targetType : " + this.GetType());
                }
				
			}
		}

	}
}

