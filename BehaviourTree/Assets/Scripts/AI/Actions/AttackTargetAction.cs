using UnityEngine;

public class AttackTargetAction : IAction
{
    private readonly Transform target;
    private readonly EnemyFacade enemy;
    private float attackDuration;
    private float timeLeft;

    public AttackTargetAction(Transform _target, EnemyFacade _enemy, float _attackDuration)
    {
        target = _target;
        enemy = _enemy;
        attackDuration = _attackDuration;
    }

    public void Enter(AIContext context)
    {
        timeLeft = attackDuration;
        enemy.Attack.Attack(target.transform);
    }

    public StateStatus Tick(AIContext context)
    {
        timeLeft -= context.DeltaTime;

        if (timeLeft > 0f)
            return StateStatus.Running;

        return StateStatus.Success;
    }

    public void Exit(AIContext context, StateStatus status) { }

    public void Abort(AIContext context)
    {
        timeLeft = 0f;
    }

    public void Reset(AIContext context)
    {
        timeLeft = 0f;
    }
}