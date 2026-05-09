using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum BTNodeType
{
    Selector,
    Sequence,
    MemorySequence,
    Repeater,
    Condition,
    Action,
}

[Serializable]
public class BTNodeData
{
    public string Id;
    public BTNodeType NodeType;
    public string Name;
    public Vector2 Position;

    public string ActionType;
    public string ConditionType;
    public List<BTParam> Params = new();
}

[Serializable]
public class BTParam
{
    public string Key;
    public string Value;
}

[Serializable]
public class BTEdgeData
{
    public string OutputNodeId;
    public string InputNodeId;
    public int ChildIndex;
}