using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[DisallowMultipleComponent]
public class GameEventManager : Singleton<GameEventManager>
{
    private Dictionary<string, GameEventSO> Events;

    private void Awake()
    {
        if(!Implement(this, out var err))
        {
            Debug.LogError(err);
        }
    }


    public void Initialize()
    {
        Events = new Dictionary<string, GameEventSO>();
        var evs = Resources.LoadAll<GameEventSO>("Events");

        foreach (var e in evs)  
        {
            if(!Events.TryAdd(e.EventName.ToLower(), e))
            {
                Debug.LogError($"Event already exists with name: {e.EventName}, by object: {e.name}");
            }
        }
    }


    public void RaiseEvent(string eventName, object value)
    {
        if(Events.TryGetValue(eventName, out var ev))
        {
            ev.Raise(this, value);
        }
        else
        {
            Debug.LogWarning($"No event with name: {eventName}");
        }
    }

    public void RaiseEventPropagate(string eventName, Component comp, object value)
    {
        if (Events.TryGetValue(eventName, out var ev))
        {
            ev.Raise(comp, value);
        }
        else
        {
            Debug.LogWarning($"No event with name: {eventName}");
        }
    }

    public GameEventSO GetEvent(string eventName)
    {
        if (Events.TryGetValue(eventName, out var ev))
        {
            return ev;
        }
        else
        {
            Debug.LogWarning($"No event with name: {eventName}");
            return null;
        }
    }
}
