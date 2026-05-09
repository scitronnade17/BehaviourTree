using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BtGraphLoader
{
    public static BtNode Load(BtGraphConfig config, BtRuntimeContext ctx)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        if (config.Nodes.Count == 0) throw new InvalidOperationException("Config has no nodes.");

        var nodeMap = config.Nodes.ToDictionary(n => n.Id);

        var childMap = new Dictionary<string, List<string>>();
        foreach (var n in config.Nodes)
            childMap[n.Id] = new List<string>();

        foreach (var edge in config.Edges.OrderBy(e => e.ChildIndex))
        {
            if (!childMap.ContainsKey(edge.OutputNodeId))
                childMap[edge.OutputNodeId] = new List<string>();
            childMap[edge.OutputNodeId].Add(edge.InputNodeId);
        }

        string rootId = config.RootNodeId;
        if (string.IsNullOrEmpty(rootId) || !nodeMap.ContainsKey(rootId))
        {
            var childIds = new HashSet<string>(config.Edges.Select(e => e.InputNodeId));
            rootId = config.Nodes.FirstOrDefault(n => !childIds.Contains(n.Id))?.Id;
        }

        if (rootId == null)
            throw new InvalidOperationException("Cannot determine root node of the behaviour tree.");

        return BuildNode(rootId, nodeMap, childMap, ctx);
    }

    private static BtNode BuildNode(
        string id,
        Dictionary<string, BTNodeData> nodeMap,
        Dictionary<string, List<string>> childMap,
        BtRuntimeContext ctx)
    {
        var data = nodeMap[id];
        var children = childMap.TryGetValue(id, out var ch)
            ? ch.Select(childId => BuildNode(childId, nodeMap, childMap, ctx)).ToArray()
            : Array.Empty<BtNode>();

        return data.NodeType switch
        {
            BTNodeType.Selector => new SelectorNode(data.Name, children),
            BTNodeType.Sequence => new SequenceNode(data.Name, children),
            BTNodeType.MemorySequence => new MemorySequenceNode(data.Name, children),
            BTNodeType.Repeater => BuildRepeater(data, children),
            BTNodeType.Condition => BuildCondition(data, children, ctx),
            BTNodeType.Action => BuildAction(data, ctx),
            _ => throw new NotSupportedException($"Unknown node type: {data.NodeType}")
        };
    }

    private static BtNode BuildRepeater(BTNodeData data, BtNode[] children)
    {
        if (children.Length != 1)
            throw new InvalidOperationException($"Repeater '{data.Name}' must have exactly one child.");
        return new RepeatNode(data.Name, children[0]);
    }

    private static BtNode BuildCondition(BTNodeData data, BtNode[] children, BtRuntimeContext ctx)
    {
        if (children.Length != 1)
            throw new InvalidOperationException($"Condition '{data.Name}' must have exactly one child.");

        var condition = BuildConditionImpl(data, ctx);
        return new ConditionNode(data.Name, condition, children[0]);
    }

    private static ICondition BuildConditionImpl(BTNodeData data, BtRuntimeContext ctx)
    {
        return data.ConditionType switch
        {
            "PlayerNearCondition" => new PlayerNearCondition(
                ctx.Player,
                data.GetFloat("chaseRadius", ctx.ChaseRadius)),

            _ => throw new NotSupportedException(
                $"Unknown condition type: '{data.ConditionType}'. " +
                $"Register it in BTGraphLoader.BuildConditionImpl().")
        };
    }

    private static BtNode BuildAction(BTNodeData data, BtRuntimeContext ctx)
    {
        IAction action = data.ActionType switch
        {
            "ChaseTargetAction" => new ChaseTargetAction(
                data.GetString("targetKey", "MoveTarget"),
                ctx.Player),

            "MoveToTargetAction" => new MoveToTargetAction(
                data.GetString("targetKey", "MoveTarget"),
                data.GetFloat("moveSpeed", ctx.MoveSpeed),
                data.GetFloat("stopDistance", ctx.StopDistance)),

            "AttackTargetAction" => new AttackTargetAction(
                ctx.Player,
                ctx.EnemyFacade,
                data.GetFloat("cooldown", ctx.Cooldown)),

            "PickRandomPointAction" => new PickRandomPointAction(
                data.GetString("targetKey", "MoveTarget"),
                ctx.PatrolPoints),

            _ => throw new NotSupportedException(
                $"Unknown action type: '{data.ActionType}'. " +
                $"Register it in BTGraphLoader.BuildAction().")
        };

        return new ActionNode(data.Name, action);
    }
}

public static class BtNodeDataExtensions
{
    public static string GetString(this BTNodeData data, string key, string fallback = "")
    {
        var p = data.Params?.Find(x => x.Key == key);
        return p != null ? p.Value : fallback;
    }

    public static float GetFloat(this BTNodeData data, string key, float fallback = 0f)
    {
        var p = data.Params?.Find(x => x.Key == key);
        return (p != null && float.TryParse(p.Value, out var v)) ? v : fallback;
    }

    public static int GetInt(this BTNodeData data, string key, int fallback = 0)
    {
        var p = data.Params?.Find(x => x.Key == key);
        return (p != null && int.TryParse(p.Value, out var v)) ? v : fallback;
    }
}

public class BtRuntimeContext
{
    public Transform Player;
    public Transform[] PatrolPoints;
    public float MoveSpeed;
    public float StopDistance;
    public float ChaseRadius;
    public float Cooldown;
    public EnemyFacade EnemyFacade;
}