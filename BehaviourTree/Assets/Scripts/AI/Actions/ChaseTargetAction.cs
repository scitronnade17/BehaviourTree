using UnityEngine;

public class ChaseTargetAction : IAction
{
    private readonly string key;
    private readonly Transform source;

    public ChaseTargetAction(string _key, Transform _source)
    {
        key = _key;
        source = _source;
    }

    public void Enter(AIContext context)
    {
    }

    public StateStatus Tick(AIContext context)
    {
        if (source == null)
            return StateStatus.Failure;

        context.Blackboard.Set(key, source.position);
        return StateStatus.Success;
    }

    public void Exit(AIContext context, StateStatus status)
    {
    }

    public void Abort(AIContext context)
    {
    }

    public void Reset(AIContext context)
    {
    }
}