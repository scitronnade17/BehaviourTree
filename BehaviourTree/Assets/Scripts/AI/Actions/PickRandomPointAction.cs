using UnityEngine;

public sealed class PickRandomPointAction : IAction
{
    private readonly string key;
    private readonly Transform[] points;

    public PickRandomPointAction(string _key, Transform[] _points)
    {
        key = _key;
        points = _points;
    }

    public void Enter(AIContext context)
    {
    }

    public StateStatus Tick(AIContext context)
    {
        if (points == null || points.Length == 0)
            return StateStatus.Failure;

        var index = Random.Range(0, points.Length);
        var point = points[index];
        if (point == null)
            return StateStatus.Failure;

        context.Blackboard.Set(key, point.position);
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