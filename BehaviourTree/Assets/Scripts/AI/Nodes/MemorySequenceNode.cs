using System;

public class MemorySequenceNode : BtNode
{
    private readonly BtNode[] children;
    private int childIndex;

    public MemorySequenceNode(string name, params BtNode[] _children) : base(name)
    {
        children = _children ?? Array.Empty<BtNode>();
        childIndex = 0;
    }

    protected override StateStatus OnTick(AIContext context)
    {
        for (var i = childIndex; i < children.Length; i++)
        {
            var status = children[i].Tick(context);

            switch (status)
            {
                case StateStatus.Failure:
                    childIndex = 0;
                    return StateStatus.Failure;

                case StateStatus.Running:
                    childIndex = i;
                    return StateStatus.Running;

                case StateStatus.Success:
                    childIndex = i + 1;
                    continue;
            }
        }

        childIndex = 0;
        return StateStatus.Success;
    }

    protected override void OnAbort(AIContext context)
    {
        AbortRunning(context);
        childIndex = 0;
    }

    protected override void OnReset(AIContext context)
    {
        childIndex = 0;
    }

    private void AbortRunning(AIContext context)
    {
        if (childIndex < 0 || childIndex >= children.Length)
            return;

        children[childIndex].Abort(context);
    }
}