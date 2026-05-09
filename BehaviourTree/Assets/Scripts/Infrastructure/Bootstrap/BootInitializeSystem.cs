using UnityEngine;
using Zenject;

public class BootInitializeSystem: IInitializable
{
    private readonly IGameStateMachine stateMachine;

    public BootInitializeSystem(IGameStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }
    public void Initialize()
    {
        stateMachine.Enter<BootstrapState>();
    }
}