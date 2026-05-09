using System.Collections.Generic;
using UnityEngine;

public class BtGraphConfig : ScriptableObject
{
    public string Id;
    public string RootNodeId;
    public List<BTNodeData> Nodes = new();
    public List<BTEdgeData> Edges = new();
}
