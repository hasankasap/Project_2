using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    private static EventManager eventManager;

    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    private Dictionary<string, Action<object[]>> eventDictionary;

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<object[]>>();
        }
    }

    public static void StartListening(string eventName, Action<object[]> listener)
    {
        Action<object[]> thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<object[]> listener)
    {
        if (eventManager == null) return;
        Action<object[]> thisEvent;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
    }
    public static bool ArrayNullOrEmtptyCheck(object[] obj)
    {
        if (obj == null || obj != null && obj.Length == 0)
        {
            Debug.LogError("Array empty or null. Please check your code. You must resgister first GamePlayParticles then target position as a vector3!!");
            return true;
        }
        return false;
    }
    public static void TriggerEvent(string eventName, object[] parameters)
    {
        Action<object[]> thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            if (thisEvent != null)
            {
                thisEvent.Invoke(parameters);
            }
        }
    }
}