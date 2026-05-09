public static class AIBuilder
{
    public static SelectorNode Selector(string name, params BtNode[] children) =>
      new SelectorNode(name, children);

    public static BtNode Sequence(string name, params BtNode[] children) =>
      new SequenceNode(name, children);

    public static BtNode MemorySequence(string name, params BtNode[] children) =>
      new MemorySequenceNode(name, children);

    public static BtNode Repeater(string name, BtNode child) =>
      new RepeatNode(name, child);

    public static BtNode Condition(string name, ICondition condition, BtNode child) =>
      new ConditionNode(name, condition, child);

    public static BtNode Action(string name, IAction action) =>
      new ActionNode(name, action);
}