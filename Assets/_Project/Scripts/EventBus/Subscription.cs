using System;

public class Subscription
{
    public Delegate HandlerDelegate { get; }
    public object Subscriber { get; }

    public Subscription(Delegate handlerDelegate, object subscriber)
    {
        HandlerDelegate = handlerDelegate;
        Subscriber = subscriber;
    }
}