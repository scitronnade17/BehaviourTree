using UnityEngine;

public class BootstrapState : IState
{
    private readonly IGameStateMachine stateMachine;
    private readonly ILoadingCurtain curtain;

    public BootstrapState(
        IGameStateMachine _stateMachine,
        ILoadingCurtain _curtain)
    {
        stateMachine = _stateMachine;
        curtain = _curtain;
    }

    public void Enter()
    {
        curtain.Show();

        Debug.Log("Game Warmup Complete");

        stateMachine.Enter<LoadLevelState, int>(1);
    }

    public void Exit() { }
}