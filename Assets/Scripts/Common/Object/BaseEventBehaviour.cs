using System.Collections;
using System.Reflection;
using UnityEngine;
using Mirror;
public class BaseEventBehaviour : MonoBehaviour
{
    public KEventDispatcher<string> kDispatcher;

    public BaseEventBehaviour()
    {
        kDispatcher = new KEventDispatcher<string>();
    }

    //~BaseEventBehaviour()
    //{
    //    OnDestory();
    //}

    public void AddKEventListener(string eventName, KEventDispatcher<string>.CallBack callBack)
    {
        kDispatcher.AddEventListener(eventName, callBack);
    }

    public void RemoveKEventListener(string eventName, KEventDispatcher<string>.CallBack callBack)
    {
        kDispatcher.RemoveEventListener(eventName, callBack);
    }

    public void RemoveAllKEventListener(string eventName)
    {
        kDispatcher.RemoveAllEventListener(eventName);
    }

    public void RemoveAllKEventListener()
    {
        kDispatcher.RemoveAllEventListener();
    }

    public bool HasKEventListener(string eventName, KEventDispatcher<string>.CallBack callBack)
    {
        return kDispatcher.HasEventListener(eventName, callBack);
    }

    public void Dispach(string eventName)
    {
        Dispach(eventName, null);
    }

    public void Dispach(string eventName, KEventArgs args)
    {
        kDispatcher.Dispach(eventName, args);
    }

    /// <summary>
    /// Called when [disable].
    /// </summary>
    private void OnDisable()
    {
        Disable();
    }

    /// <summary>
    /// Called when [destory].
    /// </summary>
    private void OnDestory()
    {
        Destroy();
    }

    /// <summary>
    /// Disables this instance.
    /// </summary>
    protected virtual void Disable()
    {
    }

    /// <summary>
    /// Destroys this instance.
    /// </summary>
    public virtual void Destroy()
    {
        if (kDispatcher != null)
        {
            kDispatcher.OnDesotry();
            kDispatcher = null;
        }
    }
}