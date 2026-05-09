using System;

public abstract class BtNode
{
    public string Name { get; }
    private bool isActive;

    protected BtNode(string name)
    {
        Name = name;
    }

    public StateStatus Tick(AIContext context)
    {
        if (!isActive)
        {
            isActive = true;
            OnEnter(context);
        }

        var status = OnTick(context);

        switch (status)
        {
            case StateStatus.Running:
                return StateStatus.Running;

            case StateStatus.Success:
                OnExit(context, StateStatus.Success);
                isActive = false;
                OnReset(context);
                return StateStatus.Success;

            case StateStatus.Failure:
                OnExit(context, StateStatus.Failure);
                isActive = false;
                OnReset(context);
                return StateStatus.Failure;

            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown status");
        }
    }

    public void Abort(AIContext context)
    {
        if (!isActive)
            return;

        OnAbort(context);
        isActive = false;
        OnReset(context);
    }

    protected virtual void OnEnter(AIContext context) { }
    protected abstract StateStatus OnTick(AIContext context);
    protected virtual void OnExit(AIContext context, StateStatus status) { }
    protected virtual void OnAbort(AIContext context) { }
    protected virtual void OnReset(AIContext context) { }
}