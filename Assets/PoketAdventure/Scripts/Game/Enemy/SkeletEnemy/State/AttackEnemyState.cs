using CloudeDev.Game.Enemy;
using CloudeDev.Game.Enemy.State;
using CloudeDev.Game.StateMachine;
using UnityEngine;

public class AttackEnemyState : IState
{
    private readonly Animator _animator;
    private readonly Transform _transform;
    private readonly float _distance;
    private readonly IStateSwitcher _stateMachine;

    public AttackEnemyState(Animator animator, AttackType attackType,
        Transform enemyTransform, float distance, int damage,
        IStateSwitcher stateMachine)
    {
        _animator = animator;
        _transform = enemyTransform;
        _distance = distance;
        _stateMachine = stateMachine;
    }

    public void Enter() => _animator.SetBool("Attack", false);

    public void Exit() => _animator.SetBool("Attack", false);

    public void Update()
    {
        // Урон теперь наносит TurnBasedCombatController.
        // Здесь только детект: если игрок ушёл — возвращаемся в Idle.
        var hit = Physics2D.Raycast(_transform.position, Vector2.left, _distance);
        if (hit.collider == null || !hit.collider.CompareTag("Player"))
            _stateMachine.SwitchState<IdleEnemyState>();
    }
}