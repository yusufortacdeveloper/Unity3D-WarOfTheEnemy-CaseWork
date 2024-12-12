using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singleton = new GameObject(nameof(EventManager));
                instance = singleton.AddComponent<EventManager>();
            }
            return instance;
        }
    }

    private Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();
    private List<IEventListener> eventListeners = new List<IEventListener>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void StartListening(string eventName, Action listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = null;
        }
        eventDictionary[eventName] += listener;
    }

    public void StopListening(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public void TriggerEvent(string eventName)
    {
        if (eventDictionary.TryGetValue(eventName, out Action listener))
        {
            listener?.Invoke();
        }
    }

    public void RegisterListener(IEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(IEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }

    public T GetListener<T>() where T : IEventListener
    {
        foreach (var listener in eventListeners)
        {
            if (listener is T)
            {
                return (T)listener;
            }
        }
        return default;
    }

    //private void OnDestroy()
    //{
    //    instance = null;
    //    foreach (var listener in eventListeners)
    //    {
    //        if (listener is MonoBehaviour mono)
    //        {
    //            Destroy(mono);
    //        }
    //    }

    //    eventDictionary.Clear();
    //    eventListeners.Clear();
    //}
}

public interface IEventListener { }