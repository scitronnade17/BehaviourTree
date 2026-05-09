using Zenject;

public interface IGameStateMachine
{
    void Enter<TState>() where TState : class, IState;
    void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
}

public class GameStateMachine : IGameStateMachine, ITickable
{
    private readonly IStateFactory stateFactory;
    private IExitableState currentState;

    public GameStateMachine(IStateFactory _stateFactory)
    {
        stateFactory = _stateFactory;
    }

    public void Enter<TState>() where TState : class, IState
    {
        IState state = ChangeState<TState>();
        state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
        TState state = ChangeState<TState>();
        state.Enter(payload);
    }

    public void Tick()
    {
        if (currentState is IUpdatable updatable)
        {
            updatable.Tick();
        }
    }

    private TState ChangeState<TState>() where TState : class, IExitableState
    {
        currentState?.Exit();

        TState state = GetState<TState>();
        currentState = state;
        return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState =>
      stateFactory.GetState<TState>();
}