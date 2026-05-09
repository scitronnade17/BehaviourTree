using System;

public sealed class RepeatNode : BtNode
{
    private readonly BtNode child;

    public RepeatNode(string name, BtNode _child) : base(name)
    {
        child = _child ?? throw new ArgumentNullException(nameof(_child));
    }

    protected override StateStatus OnTick(AIContext context)
    {
        var status = child.Tick(context);

        switch (status)
        {
            case StateStatus.Running:
                return StateStatus.Running;

            case StateStatus.Success:
                return RepeatFinished(context, StateStatus.Success);

            case StateStatus.Failure:
                return RepeatFinished(context, StateStatus.Failure);

            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown StateStatus");
        }
    }

    protected override void OnAbort(AIContext context)
    {
        child.Abort(context);
    }

    protected override void OnReset(AIContext context)
    {
    }

    private StateStatus RepeatFinished(AIContext context, StateStatus finishedStatus)
    {
        child.Abort(context);

        return StateStatus.Running;
    }
}