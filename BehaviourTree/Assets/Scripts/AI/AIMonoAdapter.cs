using UnityEngine;

public class AIMonoAdapter : MonoBehaviour
{
    [SerializeField] private BtGraphConfig behaviourTreeConfig;

    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private EnemyFacade enemyFacade;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float chaseRadius = 6f;
    [SerializeField] private float cooldown = 2f;

    private AIBrain brain;

    private void Awake()
    {
        if (behaviourTreeConfig != null)
        {
            var runtimeContext = new BtRuntimeContext
            {
                Player = player,
                PatrolPoints = patrolPoints,
                EnemyFacade = enemyFacade,
                MoveSpeed = moveSpeed,
                StopDistance = stopDistance,
                ChaseRadius = chaseRadius,
                Cooldown = cooldown,
            };
            brain = new AIBrain(behaviourTreeConfig, runtimeContext);
        }
    }

    private void Update()
    {
        brain.AiTick(this);
    }
}