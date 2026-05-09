using UnityEngine;

public class EnemyFacade : MonoBehaviour
{
    [field: SerializeField] public EnemyAttack Attack { get; private set; }
}