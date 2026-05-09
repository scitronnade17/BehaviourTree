using System;

public interface ICondition
{
    bool Check(AIContext context);
}

public sealed class ConditionNode : BtNode
{
    private readonly ICondition condition;
    private readonly BtNode child;

    public ConditionNode(string name, ICondition _condition, BtNode _child) : base(name)
    {
        condition = _condition ?? throw new ArgumentNullException(nameof(_condition));
        child = _child ?? throw new ArgumentNullException(nameof(_child));
    }

    protected override StateStatus OnTick(AIContext context)
    {
        if (!condition.Check(context))
        {
            child.Abort(context);
            return StateStatus.Failure;
        }

        return child.Tick(context);
    }

    protected override void OnAbort(AIContext context)
    {
        child.Abort(context);
    }
}