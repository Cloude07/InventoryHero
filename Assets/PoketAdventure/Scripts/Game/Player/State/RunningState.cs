using CloudeDev.Game.Enemy;
using CloudeDev.Game.StateMachine;
using UnityEngine;


namespace CloudeDev.Game.Player.State
{
    public class RunningState : PlayerStateBase
    {

        public RunningState(IPlayerStats statsModel, IStateSwitcher stateSwitcher, IPlayerView playerView)
        : base(statsModel, stateSwitcher, playerView) 
        {
        }

        public override void Enter()
        {
            _playerView.SetRunningFlag(true);      // ← страховка
            _playerView.PlayAnimation("Running");
        }

        public override void Update()
        {
            if (_playerView == null) return;
            MovePlayer();
            TryFindEnemy();
        }

        private void TryFindEnemy()
        {
            var hit = Physics2D.Raycast(
                _playerView.DamagePoint.position,
                Vector2.right,
                _playerView.DetectionDistance);

            if (hit.collider?.CompareTag("Enemy") != true) return;

            var enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy == null || enemy.IsDead) return;   // ← не идём в бой с трупом

            _stateSwitcher.SwitchState<FightState>();
        }

        private void MovePlayer()
        {
            Vector2 movement = Vector2.right * _statsModel.Speed * Time.deltaTime;
            _playerView.PlayerTransform.Translate(movement);
        }

        public override void Exit()
        {
            _playerView.SetRunningFlag(false);     // ← вместо IsFight()
        }
    }
}
