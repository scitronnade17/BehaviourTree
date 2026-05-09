public class LoadLevelState : IPayloadedState<int>
{
    private readonly ISceneLoader sceneLoader;
    private readonly IGameStateMachine stateMachine;

    public LoadLevelState(ISceneLoader _sceneLoader,
      IGameStateMachine stateMachine)
    {
        sceneLoader = _sceneLoader;
        this.stateMachine = stateMachine;
    }

    public void Enter(int sceneIndex)
    {
        sceneLoader.LoadScene(sceneIndex, OnLoaded);
    }

    private void OnLoaded()
    {
        stateMachine.Enter<LevelLoopState>();
    }

    public void Exit() { }
}