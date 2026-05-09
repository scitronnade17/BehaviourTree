using System;

public sealed class ActionNode : BtNode
{
    private readonly IAction action;

    public ActionNode(string name, IAction _action) : base(name)
    {
        action = _action ?? throw new ArgumentNullException(nameof(_action));
    }

    protected override void OnEnter(AIContext context) =>
      action.Enter(context);

    protected override StateStatus OnTick(AIContext context) =>
      action.Tick(context);

    protected override void OnExit(AIContext context, StateStatus status) =>
      action.Exit(context, status);

    protected override void OnAbort(AIContext context) =>
      action.Abort(context);

    protected override void OnReset(AIContext context) =>
      action.Reset(context);
}