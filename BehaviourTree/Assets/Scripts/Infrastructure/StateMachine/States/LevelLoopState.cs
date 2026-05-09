public class LevelLoopState : IState, IUpdatable
{
    private readonly ILoadingCurtain curtain;

    public LevelLoopState(ILoadingCurtain _curtain)
    {
        curtain = _curtain;
    }

    public void Enter()
    {
        curtain.Hide();
    }

    public void Tick()
    {
        //Debug.Log("LevelLoop state");
    }

    public void Exit()
    {
    }
}