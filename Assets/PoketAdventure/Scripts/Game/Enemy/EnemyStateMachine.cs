using CloudeDev.Game.Enemy.State;
using CloudeDev.Game.StateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CloudeDev.Game.Enemy
{
    public class EnemyStateMachine : IStateSwitcher
    {
        public IState _currentState;
        private List<IState> _states;
        private Animator _animator;

        public EnemyStateMachine(Animator animator, AttackType attackType,Transform enemyTransform, float distance, int damage)
        {
            _animator = animator;

            _states = new List<IState>()
            {
                new IdleEnemyState(_animator, this, attackType, enemyTransform, distance),
                new AttackEnemyState(_animator, attackType, enemyTransform, distance, damage),
                new DeadEnemyState(_animator),
                new BlockEnemyState(_animator)
            };
            _currentState = _states[0];
            _currentState.Enter();
        }
        public void Update()
        {
            _currentState?.Update();
        }

        public void SwitchState<T>() where T : IState
        {
            var state = _states.FirstOrDefault(state => state is T);
            _currentState.Exit();
            _currentState = state;
            _currentState?.Enter();
        }
    }
}
