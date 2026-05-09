public interface IAction
{
    void Enter(AIContext context);
    StateStatus Tick(AIContext context);
    void Exit(AIContext context, StateStatus status);
    void Abort(AIContext context);
    void Reset(AIContext context);
}