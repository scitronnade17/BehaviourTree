using UnityEngine;

public class AIContext
{
    public float DeltaTime;

    public Transform SelfTransform;
    public Blackboard Blackboard { get; }

    public AIContext(Blackboard btBlackboard)
    {
        Blackboard = btBlackboard;
    }
}