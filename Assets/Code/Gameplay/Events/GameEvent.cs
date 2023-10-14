using System;

public class GameEvent
{
    private event Action action = delegate { };

    public void Dispatch()
    {
        action?.Invoke();
    }

    public void Subscribe(Action subscriber)
    {
        action += subscriber;
    }

    public void Unsubscribe(Action subscriber)
    {
        action -= subscriber;
    }
}

public class GameEvent<T>
{
    private event Action<T> action = delegate { };

    public void Dispatch(T param)
    {
        action?.Invoke(param);
    }

    public void Subscribe(Action<T> subscriber)
    {
        action += subscriber;
    }

    public void Unsubscribe(Action<T> subscriber)
    {
        action -= subscriber;
    }
}

public class GameEvent<S,T>
{
    private event Action<S,T> action = delegate { };

    public void Dispatch(S param1, T param2)
    {
        action?.Invoke(param1, param2);
    }

    public void Subscribe(Action<S,T> subscriber)
    {
        action += subscriber;
    }

    public void Unsubscribe(Action<S,T> subscriber)
    {
        action -= subscriber;
    }
}