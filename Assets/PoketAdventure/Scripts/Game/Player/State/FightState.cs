using CloudeDev.Game.Combat;
using CloudeDev.Game.Enemy;
using CloudeDev.Game.StateMachine;
using UnityEngine;

namespace CloudeDev.Game.Player.State
{
    public class FightState : PlayerStateBase
    {
        private readonly TurnBasedCombatController _combat;
        private bool _started;

        public FightState(IPlayerStats statsModel, IStateSwitcher stateSwitcher,
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
                _playerView.IsRunning();  
                return;
            }

            var enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy == null || enemy.IsDead)
            {
                _playerView.IsRunning();   
                return;
            }

            _combat.OnCombatEnded += HandleCombatEnded;
            _combat.StartCombat(new EnemyCombatant(enemy));
            _started = true;
        }

        public override void Update()
        {
            if (!_started && !_combat.IsActive)
                _playerView.IsRunning();   // ← было SwitchState<RunningState>()
        }

        public override void Exit()
        {
            _combat.OnCombatEnded -= HandleCombatEnded;
            _playerView.PlayAnimation("Idle"); 
        }

        private void HandleCombatEnded()
        {
            if (_playerView.Health <= 0) return;
            _playerView.IsRunning();       // ← было SwitchState<RunningState>()
        }
    }
}