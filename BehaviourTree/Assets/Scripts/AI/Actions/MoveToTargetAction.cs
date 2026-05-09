using UnityEngine;

public sealed class MoveToTargetAction : IAction
{
    private readonly string key;
    private readonly float speed;
    private readonly float stopDistance;

    private bool isMoving;

    public MoveToTargetAction(string _key, float _speed, float _stopDistance)
    {
        key = _key;
        speed = _speed;
        stopDistance = _stopDistance;
    }

    public void Enter(AIContext context)
    {
        isMoving = true;
    }

    public StateStatus Tick(AIContext context)
    {
        var self = context.SelfTransform;

        var target = context.Blackboard.GetOrDefault(key, self.position);
        var dist = Vector3.Distance(self.position, target);

        if (dist <= stopDistance)
            return StateStatus.Success;

        self.position = Vector3.MoveTowards(self.position, target, speed * context.DeltaTime);
        return StateStatus.Running;
    }

    public void Exit(AIContext context, StateStatus status)
    {
        isMoving = false;
    }

    public void Abort(AIContext context)
    {
        if (!isMoving)
            return;

        isMoving = false;
    }

    public void Reset(AIContext context) { }
}