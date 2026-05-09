using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BtGraphView : GraphView
{
    public BtGraphConfig Config { get; private set; }

    private readonly Dictionary<string, BTNodeView> nodeViews = new();

    private static readonly string[] ActionTypes =
    {
        "ChaseTargetAction",
        "MoveToTargetAction",
        "AttackTargetAction",
        "PickRandomPointAction",
    };

    private static readonly string[] ConditionTypes =
    {
        "PlayerNearCondition",
    };

    public BtGraphView()
    {
        InitManipulators();
        InitGridBackground();
        InitStyles();
        graphViewChanged = OnGraphChanged;
    }

    private void InitStyles()
    {
        style.flexGrow = 1;
    }

    private void InitManipulators()
    {
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContextualMenuManipulator(BuildContextMenu));
    }

    private void InitGridBackground()
    {
        var grid = new GridBackground();
        grid.StretchToParentSize();
        Insert(0, grid);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports
            .Where(p => p.direction != startPort.direction && p.node != startPort.node)
            .ToList();
    }

    private GraphViewChange OnGraphChanged(GraphViewChange change)
    {
        if (change.movedElements != null)
        {
            foreach (var el in change.movedElements)
            {
                if (el is BTNodeView nv)
                    nv.SyncPosition();
            }
        }

        if (change.elementsToRemove != null)
        {
            foreach (var el in change.elementsToRemove)
            {
                if (el is BTNodeView nv)
                {
                    Config.Nodes.RemoveAll(n => n.Id == nv.Data.Id);
                    nodeViews.Remove(nv.Data.Id);
                }
                else if (el is Edge edge)
                {
                    RemoveEdgeFromConfig(edge);
                }
            }
        }

        if (change.edgesToCreate != null)
        {
            foreach (var edge in change.edgesToCreate)
                AddEdgeToConfig(edge);
        }

        return change;
    }

    private void BuildContextMenu(ContextualMenuPopulateEvent evt)
    {
        if (Config == null) return;

        var mousePos = contentViewContainer.WorldToLocal(evt.mousePosition);

        evt.menu.AppendAction("Selector", _ => CreateNode(BTNodeType.Selector, "Selector", mousePos));
        evt.menu.AppendAction("Sequence", _ => CreateNode(BTNodeType.Sequence, "Sequence", mousePos));
        evt.menu.AppendAction("MemorySequence", _ => CreateNode(BTNodeType.MemorySequence, "MemorySequence", mousePos));

        evt.menu.AppendAction("Repeater", _ => CreateNode(BTNodeType.Repeater, "Repeater", mousePos));
        foreach (var ct in ConditionTypes)
        {
            var captured = ct;
            evt.menu.AppendAction($"Condition/{captured}", _ => CreateConditionNode(captured, mousePos));
        }
        foreach (var at in ActionTypes)
        {
            var captured = at;
            evt.menu.AppendAction($"Action/{captured}", _ => CreateActionNode(captured, mousePos));
        }
    }

    public BTNodeView CreateNode(BTNodeType type, string nodeName, Vector2 position)
    {
        var data = new BTNodeData
        {
            Id = Guid.NewGuid().ToString(),
            NodeType = type,
            Name = nodeName,
            Position = position,
        };

        Config.Nodes.Add(data);
        return AddNodeView(data);
    }

    private BTNodeView CreateConditionNode(string conditionType, Vector2 position)
    {
        var data = new BTNodeData
        {
            Id = Guid.NewGuid().ToString(),
            NodeType = BTNodeType.Condition,
            Name = conditionType,
            ConditionType = conditionType,
            Position = position,
        };

        Config.Nodes.Add(data);
        return AddNodeView(data);
    }

    private BTNodeView CreateActionNode(string actionType, Vector2 position)
    {
        var data = new BTNodeData
        {
            Id = Guid.NewGuid().ToString(),
            NodeType = BTNodeType.Action,
            Name = actionType,
            ActionType = actionType,
            Position = position,
        };

        Config.Nodes.Add(data);
        return AddNodeView(data);
    }

    private BTNodeView AddNodeView(BTNodeData data)
    {
        var view = new BTNodeView(data);
        nodeViews[data.Id] = view;
        AddElement(view);
        return view;
    }

    private void AddEdgeToConfig(Edge edge)
    {
        if (edge.output?.node is not BTNodeView outNode) return;
        if (edge.input?.node is not BTNodeView inNode) return;

        int childIndex = Config.Edges.Count(e => e.OutputNodeId == outNode.Data.Id);

        Config.Edges.Add(new BTEdgeData
        {
            OutputNodeId = outNode.Data.Id,
            InputNodeId = inNode.Data.Id,
            ChildIndex = childIndex,
        });
    }

    private void RemoveEdgeFromConfig(Edge edge)
    {
        if (edge.output?.node is not BTNodeView outNode) return;
        if (edge.input?.node is not BTNodeView inNode) return;

        Config.Edges.RemoveAll(e =>
            e.OutputNodeId == outNode.Data.Id &&
            e.InputNodeId == inNode.Data.Id);
    }

    public void SetGraph(BtGraphConfig config)
    {
        Config = config;
        ClearGraph();

        foreach (var nodeData in config.Nodes)
            AddNodeView(nodeData);

        foreach (var edgeData in config.Edges)
        {
            if (!nodeViews.TryGetValue(edgeData.OutputNodeId, out var outView)) continue;
            if (!nodeViews.TryGetValue(edgeData.InputNodeId, out var inView)) continue;

            var outPort = outView.OutputPort ?? outView.CondChildPort;
            var inPort = inView.InputPort;

            if (outPort == null || inPort == null) continue;

            var edge = outPort.ConnectTo(inPort);
            AddElement(edge);
        }
    }

    private void ClearGraph()
    {
        nodeViews.Clear();
        foreach (var edge in edges.ToList()) RemoveElement(edge);
        foreach (var node in nodes.ToList()) RemoveElement(node);
    }
}