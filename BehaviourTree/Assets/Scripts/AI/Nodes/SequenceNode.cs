using System;

public sealed class SequenceNode : BtNode
{
    private readonly BtNode[] children;
    private int runningChildIndex = -1;

    public SequenceNode(string name, params BtNode[] _children) : base(name)
    {
        children = _children ?? Array.Empty<BtNode>();
    }

    protected override StateStatus OnTick(AIContext context)
    {
        for (var i = 0; i < children.Length; i++)
        {
            var status = children[i].Tick(context);

            switch (status)
            {
                case StateStatus.Failure:
                    AbortPreviousRunning(context, newIndex: i);
                    runningChildIndex = -1;
                    return StateStatus.Failure;

                case StateStatus.Running:
                    AbortPreviousRunning(context, newIndex: i);
                    runningChildIndex = i;
                    return StateStatus.Running;

                case StateStatus.Success:
                    continue;
            }
        }

        runningChildIndex = -1;
        return StateStatus.Success;
    }

    protected override void OnAbort(AIContext context)
    {
        AbortCurrentRunning(context);
    }

    protected override void OnReset(AIContext context)
    {
        runningChildIndex = -1;
    }

    private void AbortPreviousRunning(AIContext context, int newIndex)
    {
        if (runningChildIndex != -1 && runningChildIndex != newIndex)
            children[runningChildIndex].Abort(context);
    }

    private void AbortCurrentRunning(AIContext context)
    {
        if (runningChildIndex == -1)
            return;

        if (runningChildIndex < children.Length)
            children[runningChildIndex].Abort(context);

        runningChildIndex = -1;
    }
}