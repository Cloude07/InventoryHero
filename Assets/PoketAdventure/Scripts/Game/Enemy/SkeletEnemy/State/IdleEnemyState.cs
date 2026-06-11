using CloudeDev.Game.StateMachine;
using UnityEngine;

namespace CloudeDev.Game.Enemy.State
{
    public class IdleEnemyState : IState
    {
        private Animator _animator;
        private AttackType _attackType;
        private float _distance;
        private Transform _transform;
        private IStateSwitcher _StateMachine;

        public IdleEnemyState(Animator animator, IStateSwitcher stateSwitcher, AttackType attackType, Transform enemyTransform, float distance)
        {
            _animator = animator;
            _attackType = attackType;
            _transform = enemyTransform;
            _distance = distance;
            _StateMachine = stateSwitcher;
        }


        public void Enter()
        {

                
        }

        public void Exit()
        {
           
        }

        public void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(_transform.position, Vector2.left, _distance);
            if (hit.collider?.CompareTag("Player") == true)
                _StateMachine.SwitchState<AttackEnemyState>();
        }
    }
}
