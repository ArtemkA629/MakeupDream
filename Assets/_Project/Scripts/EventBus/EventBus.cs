using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, Dictionary<object, List<Subscription>>> _subscriptions = new();

    public static void Subscribe<T>(Action<T> handler, object subscriber, object publisher = null)
    {
        var eventType = typeof(T);
        
        if (!_subscriptions.TryGetValue(eventType, out var publisherSubscriptions))
        {
            publisherSubscriptions = new Dictionary<object, List<Subscription>>();
            _subscriptions[eventType] = publisherSubscriptions;
        }

        object publisherKey = publisher ?? "global";
        
        if (!publisherSubscriptions.TryGetValue(publisherKey, out var handlers))
        {
            handlers = new List<Subscription>();
            publisherSubscriptions[publisherKey] = handlers;
        }
        
        handlers.Add(new Subscription(handler, subscriber));
    }

    public static void Unsubscribe<T>(Action<T> handler, object publisher = null)
    {
        var eventType = typeof(T);
        if (!_subscriptions.TryGetValue(eventType, out var publisherSubscriptions)) 
            return;

        object publisherKey = publisher ?? "global";
        
        // 1. Попытка удаления у конкретного издателя
        if (publisherSubscriptions.TryGetValue(publisherKey, out var handlers))
        {
            RemoveHandlerFromList(handler, handlers);
            if (handlers.Count == 0) publisherSubscriptions.Remove(publisherKey);
        }
        
        // 2. Автоматическая очистка пустых коллекций
        if (publisherSubscriptions.Count == 0) _subscriptions.Remove(eventType);
    }

    public static void Publish<T>(T eventData, object publisher = null)
    {
        var eventType = typeof(T);
        if (!_subscriptions.TryGetValue(eventType, out var publisherSubscriptions)) return;

        var keysToPublish = new List<object> { publisher, "global" };

        foreach (var key in keysToPublish)
        {
            if (key != null && publisherSubscriptions.TryGetValue(key, out var handlers))
            {
                foreach (var subscription in handlers.ToList())
                {
                    try
                    {
                        ((Action<T>)subscription.HandlerDelegate)(eventData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"EventBus exception: {e}");
                    }
                }
            }
        }
    }
    
    private static void RemoveHandlerFromList<T>(Action<T> handler, List<Subscription> handlers)
    {
        for (int i = handlers.Count - 1; i >= 0; i--)
        {
            var subscription = handlers[i];
            if (subscription.HandlerDelegate.Target == handler.Target && 
                subscription.HandlerDelegate.Method == handler.Method)
            {
                handlers.RemoveAt(i);
                break;
            }
        }
    }
}