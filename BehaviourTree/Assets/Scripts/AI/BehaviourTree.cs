using System;
using UnityEngine;

public sealed class BehaviourTree
{
    private readonly BtNode root;
    private readonly AIContext context;

    public BehaviourTree(BtNode _root, AIContext _context)
    {
        root = _root ?? throw new ArgumentNullException(nameof(_root));
        context = _context;
    }

    public StateStatus Tick(float deltaTime, MonoBehaviour agent)
    {
        context.DeltaTime = deltaTime;
        context.SelfTransform = agent != null ? agent.transform : null;

        return root.Tick(context);
    }

    public Blackboard Blackboard => context.Blackboard;

    public void Abort()
    {
        root.Abort(context);
    }
}