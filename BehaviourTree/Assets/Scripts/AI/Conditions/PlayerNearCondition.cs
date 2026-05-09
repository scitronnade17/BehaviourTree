using UnityEngine;

public class PlayerNearCondition : ICondition
{
    private readonly Transform player;
    private readonly float radius;

    public PlayerNearCondition(Transform _player, float _radius)
    {
        player = _player;
        radius = _radius;
    }

    public bool Check(AIContext context)
    {
        return Vector3.Distance(context.SelfTransform.position, player.position) <= radius;
    }
}