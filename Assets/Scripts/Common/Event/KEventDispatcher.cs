using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KEventDispatcher<T>{

    Dictionary<T, List<CallBack>> events = new Dictionary<T, List<CallBack>>();

    public delegate void CallBack(KBaseEvent<T, KEventArgs> args);

    public void AddEventListener(T t, CallBack callback)
    {
        List<CallBack> callList = null;
        if(events.ContainsKey(t))
        {
            callList = events[t];
            if (callList.IndexOf(callback) != -1)
            {
                return;
            }
            
        }
        else
        {
            callList = new List<CallBack>();
            events.Add(t , callList);
        }
        callList.Add(callback);
    }

    public void RemoveAllEventListener(T t)
    {
        //List<CallBack> callList = null;
        if (events.ContainsKey(t))
        {
            events[t].Clear();
            events.Remove(t);
        }
    }

    public void RemoveAllEventListener()
    {
        events.Clear();
    }

    public void RemoveEventListener(T t ,CallBack callback)
    {
        List<CallBack> callList = null;
        if (events.ContainsKey(t))
        {
            callList = events[t];
            if (callList.Contains(callback))
            {
                callList.Remove(callback);
            }
        }
    }

    public bool HasEventListener(T t , CallBack callback)
    {
        List<CallBack> callList = null;
        if (events.ContainsKey(t))
        {
            callList = events[t];
            foreach (CallBack tempCallback in callList)
            {
                if (tempCallback == callback)
                {
                    return true;
                }
                
            }
        }
        return false;
    }

    public void Dispach(T t , KEventArgs args)
    {
        if (events.ContainsKey(t))
        {
                //暂时未作是否派发事件的其他验证
                List<CallBack> eventList = events[t];
                foreach (CallBack callback in eventList.ToArray())
                {
                    //暂时未作是否派发事件的其他验证
                    KBaseEvent<T, KEventArgs> kevent = new KBaseEvent<T, KEventArgs>(t, args);
                    callback(kevent);
                }
        }
    }

    public void Dispach(T t)
    {
        Dispach(t, null);
    }

    public void OnDesotry()
    {
        if (events != null && events.Count > 0)
        {
            foreach (KeyValuePair<T,List<CallBack>> kvPair in events)
            {
                if(kvPair.Value!=null)
                {
                    kvPair.Value.Clear();
                }
            }
            events.Clear();
        }
    }
}
