using System;
using Zenject;

public interface IEventBus
{
    void RaiseEvent<T>(T evt);
    void Subscribe<T>(Action<T> handler);
    void Unsubscribe<T>(Action<T> handler);
}

public class ZenjectEventBus : IEventBus
{
    private SignalBus _signalBus;

    public ZenjectEventBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void RaiseEvent<T>(T evt)
    {
        _signalBus.Fire(evt);
    }

    public void Subscribe<T>(Action<T> handler)
    {
        _signalBus.Subscribe(handler);
    }

    public void Unsubscribe<T>(Action<T> handler)
    {
        _signalBus.TryUnsubscribe(handler);
    }
}