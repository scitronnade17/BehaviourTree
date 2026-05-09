public interface IStateFactory
{
    TState GetState<TState>() where TState : class, IExitableState;
}

public class StateFactory : IStateFactory
{
    private readonly IDIService di;

    public StateFactory(IDIService _di)
    {
        di = _di;
    }

    public TState GetState<TState>() where TState : class, IExitableState =>
      di.Resolve<TState>();
}
