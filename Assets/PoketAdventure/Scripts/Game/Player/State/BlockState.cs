using CloudeDev.Game.Combat;
using CloudeDev.Game.Enemy;
using CloudeDev.Game.Player.State;
using CloudeDev.Game.StateMachine;
using UnityEngine;

namespace CloudeDev.Game.Player
{
    internal class BlockState : PlayerStateBase
    {
        private readonly TurnBasedCombatController _combat;
        private bool _started;
        public BlockState(IPlayerStats statsModel, IStateSwitcher stateSwitcher,
             IPlayerView playerView, TurnBasedCombatController combat)
            : base(statsModel, stateSwitcher, playerView)
        {
            _combat = combat;
        }

        public override void Enter()
        {
            _started = false;

            var hit = Physics2D.Raycast(
                _playerView.DamagePoint.position,
                Vector2.right,
                _playerView.DetectionDistance);

            if (hit.collider == null || !hit.collider.CompareTag("Enemy"))
            {
                _stateSwitcher.SwitchState<IdleState>();
                return;
            }
            var enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy == null || enemy.IsDead)
            {
                _stateSwitcher.SwitchState<RunningState>();   // ← было IdleState
                return;
            }
            _combat.OnCombatEnded += HandleCombatEnded;

        }

        public override void Exit()
        {
            _combat.OnCombatEnded -= HandleCombatEnded;
            _playerView.PlayAnimation("Idle");
        }

        public override void Update()
        {
            // Всё делает контроллер — здесь только страховка на случай,
            // если враг умер или убежал вне боя
            if (!_started && !_combat.IsActive)
                _stateSwitcher.SwitchState<RunningState>();
        }

        private void HandleCombatEnded()
        {
            // Если игрок умер — DeadState уже переключён через OnPlayerDied.
            if (_playerView.Health <= 0) return;

            _stateSwitcher.SwitchState<RunningState>();
        }
    }
}