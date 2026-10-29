using CloudeDev.Game.Player.State;
using UnityEngine;
using VContainer;

namespace CloudeDev.Game.Player
{
    public class PlayerController : MonoBehaviour, IPlayerView
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _playerPosDamage;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private float _detectionDistance = 2f;

        private PlayerStateMachine _playerStateMachine;
        private bool _isRunn = false;
        private IPlayerHealth _playerHealth;
        public IPlayerHealth PlayerHealth => _playerHealth;

        public Transform PlayerTransform => _playerTransform;
        public Transform DamagePoint => _playerPosDamage;
        public float DetectionDistance => _detectionDistance;

        public bool isRunn => _isRunn;

        public int Health => _playerHealth.Health;
        public Animator Animator => _playerAnimator;

        [Inject]
        private void Construct(PlayerStateMachine playerStateMachine, IPlayerHealth playerHealth)
        {
            _playerHealth = playerHealth;
            _playerStateMachine = playerStateMachine;
            _playerStateMachine.Initialize(this);

            _playerHealth.OnPlayerDied += _playerHealth_OnPlayerDied;

        }

        private void _playerHealth_OnPlayerDied()
        {
            _playerStateMachine.SwitchState<DeadState>();
        }

        public void IsRunning()
        {
            _playerStateMachine.SwitchState<RunningState>();
            _isRunn=true;
        }

        public void IsFight()
        {
            _isRunn=false;
        }

        public void PlayAnimation(string trigger)
        {
            _playerAnimator.Play(trigger);
        }

        private void Update()
        {
            _playerStateMachine?.Update();
        }

        public void Win()
        {
          _playerStateMachine.SwitchState<IdleState>();
        }

        private void OnDisable()
        {
            if (_playerHealth != null)
                _playerHealth.OnPlayerDied -= _playerHealth_OnPlayerDied;
        }

        public void Lose()
        {
            _playerStateMachine.SwitchState<DeadState>();
        }
    }
}