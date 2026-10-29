using CloudeDev.Game.Combat;
using CloudeDev.Game.Player.State;
using CloudeDev.Game.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CloudeDev.Game.Player
{
    public class PlayerStateMachine : IStateSwitcher, IPlayerStateMachine
    {
        private IPlayerView _playerView;
        private readonly IPlayerStats _statsModel;
        private readonly TurnBasedCombatController _combat;
        private IState _currentState;
        private List<IState> _states;
        private bool _isInitialized;

        public PlayerStateMachine(IPlayerStats statsModel, TurnBasedCombatController combat)
        {
            _statsModel = statsModel;
            _combat = combat;
        }

        public void Initialize(IPlayerView playerView)
        {
            _playerView = playerView;
            _states = new List<IState>
        {
            new IdleState(_statsModel, this, _playerView),
            new RunningState(_statsModel, this, _playerView),
            new FightState(_statsModel, this, _playerView, _combat),
            new DeadState(_statsModel, this, _playerView)
        };
            _currentState = _states[0];
            _currentState.Enter();
            _isInitialized = true;
        }

        public void Update()
        {
            if (!_isInitialized) return;
            _currentState?.Update();
        }

        public void SwitchState<T>() where T : IState
        {
            if (!_isInitialized) return;
            _currentState?.Exit();
            _currentState = _states.OfType<T>().FirstOrDefault();

            if (_currentState == null)
                Debug.LogError($"State {typeof(T)} not found");
            else
                _currentState?.Enter();
        }

        public bool CurrentStateIs<T>() where T : IState
        {
            return _currentState is T;
        }

        public IState GetCurrentState() => _currentState;
    }
}
