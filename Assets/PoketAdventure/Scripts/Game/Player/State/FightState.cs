using CloudeDev.Game.Enemy;
using CloudeDev.Game.StateMachine;
using UnityEngine;

namespace CloudeDev.Game.Player.State
{
    public class FightState : PlayerStateBase
    {
        private float _attackInterval = 0.5f;   // интервал между ударами
        private float _nextAttackTime;

        private EnemyController _currentEnemy;

        public FightState( IPlayerStats statsModel,IStateSwitcher stateSwitcher, IPlayerView playerView)
            : base(statsModel, stateSwitcher, playerView) { }

        public override void Enter()
        {
            // Ищем врага один раз при входе
            var hit = Physics2D.Raycast(
                _playerView.DamagePoint.position,
                Vector2.right,
                _playerView.DetectionDistance
            );

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                _currentEnemy = hit.collider.GetComponent<EnemyController>();

           


            if (_currentEnemy == null)
            {
                _stateSwitcher.SwitchState<IdleState>();
                return;
            }

            _nextAttackTime = Time.time + _attackInterval;
        }

        public override void Exit()
        {
            _currentEnemy = null;
        }

        public override void Update()
        {
            if (_currentEnemy == null || _currentEnemy.IsDead)
            {
                _stateSwitcher.SwitchState<RunningState>();
                return;
            }

            if (Time.time >= _nextAttackTime)
            {
                _currentEnemy.Damage(_statsModel.Strength);
                _nextAttackTime = Time.time + _attackInterval;
                _playerView.PlayAnimation("Fight");
            }
        }
    }
}
