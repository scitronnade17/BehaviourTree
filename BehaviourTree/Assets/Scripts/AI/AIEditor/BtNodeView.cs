using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BTNodeView : Node
{
    public BTNodeData Data;

    public Port InputPort;
    public Port OutputPort;
    public Port CondChildPort;

    public event Action<BTNodeView> OnNodeSelected;
    public event Action<BTNodeView> OnNodeDeselected;

    private static readonly Color colorSelector = new(0.40f, 0.25f, 0.75f, 1f);
    private static readonly Color colorSequence = new(0.15f, 0.55f, 0.75f, 1f);
    private static readonly Color colorMemSequence = new(0.10f, 0.65f, 0.55f, 1f);
    private static readonly Color colorRepeater = new(0.70f, 0.45f, 0.10f, 1f);
    private static readonly Color colorCondition = new(0.80f, 0.65f, 0.10f, 1f);
    private static readonly Color colorAction = new(0.20f, 0.60f, 0.30f, 1f);

    public BTNodeView(BTNodeData data)
    {
        Data = data;
        title = data.Name;
        name = data.Id;

        SetPosition(new Rect(data.Position, Vector2.zero));
        ApplyStyle();
        CreatePorts();
        RefreshExpandedState();
        RefreshPorts();
    }

    private void ApplyStyle()
    {
        var color = Data.NodeType switch
        {
            BTNodeType.Selector => colorSelector,
            BTNodeType.Sequence => colorSequence,
            BTNodeType.MemorySequence => colorMemSequence,
            BTNodeType.Repeater => colorRepeater,
            BTNodeType.Condition => colorCondition,
            BTNodeType.Action => colorAction,
            _ => Color.gray
        };

        var titleBar = this.Q("title");
        if (titleBar != null)
            titleBar.style.backgroundColor = color;

        var badge = new Label(Data.NodeType.ToString());
        badge.style.fontSize = 9;
        badge.style.color = new StyleColor(new Color(1, 1, 1, 0.55f));
        badge.style.paddingLeft = 6;
        badge.style.paddingBottom = 2;
        titleContainer.Add(badge);
        
        RefreshExpandedState();
    }

    private void CreatePorts()
    {
        InputPort = Port.Create<Edge>(
            Orientation.Vertical,
            Direction.Input,
            Port.Capacity.Single,
            typeof(bool));
        InputPort.portName = "parent";
        inputContainer.Add(InputPort);

        switch (Data.NodeType)
        {
            case BTNodeType.Selector:
            case BTNodeType.Sequence:
            case BTNodeType.MemorySequence:
                OutputPort = Port.Create<Edge>(
                    Orientation.Vertical,
                    Direction.Output,
                    Port.Capacity.Multi,
                    typeof(bool));
                OutputPort.portName = "children";
                outputContainer.Add(OutputPort);
                break;

            case BTNodeType.Repeater:
                OutputPort = Port.Create<Edge>(
                    Orientation.Vertical,
                    Direction.Output,
                    Port.Capacity.Single,
                    typeof(bool));
                OutputPort.portName = "child";
                outputContainer.Add(OutputPort);
                break;

            case BTNodeType.Condition:
                CondChildPort = Port.Create<Edge>(
                    Orientation.Vertical,
                    Direction.Output,
                    Port.Capacity.Single,
                    typeof(bool));
                CondChildPort.portName = "if true";
                outputContainer.Add(CondChildPort);
                break;

            case BTNodeType.Action:
                break;
        }
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }

    public override void OnUnselected()
    {
        base.OnUnselected();
        OnNodeDeselected?.Invoke(this);
    }

    public void SyncPosition()
    {
        Data.Position = GetPosition().position;
    }
}