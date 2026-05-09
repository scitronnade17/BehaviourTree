using UnityEngine;

public class AIBrain
{
    private readonly AIContext context;
    private readonly Blackboard blackboard;
    private readonly BehaviourTree tree;

    public AIBrain(BtGraphConfig config, BtRuntimeContext runtimeContext)
    {
        blackboard = new Blackboard();
        context = new AIContext(blackboard);

        var root = BtGraphLoader.Load(config, runtimeContext);
        tree = new BehaviourTree(root, context);
    }

    public void AiTick(MonoBehaviour agent)
    {
        tree.Tick(Time.deltaTime, agent);
    }
}