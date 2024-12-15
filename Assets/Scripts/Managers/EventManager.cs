using System;
using System.Collections.Generic;

public static class EventManager
{
    private static Dictionary<string, Action<object>> eventDictionary = new Dictionary<string, Action<object>>();

    public static void Subscribe(string eventName, Action<object> listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = delegate { };
        }
        eventDictionary[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action<object> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public static void Trigger(string eventName, object parameter = null)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName].Invoke(parameter);
        }
    }
}
